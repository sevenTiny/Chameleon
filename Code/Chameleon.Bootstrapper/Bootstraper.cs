using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Infrastructure.Configs;
using Chameleon.Infrastructure.Consts;
using Chameleon.Infrastructure.Enums;
using Chameleon.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SevenTiny.Cloud.ScriptEngine;
using SevenTiny.Cloud.ScriptEngine.CSharp;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Chameleon.Bootstrapper
{
    /// <summary>
    /// 业务层依赖注入器
    /// </summary>
    public static class Bootstraper
    {
        /// <summary>
        /// 默认放行的路径
        /// </summary>
        private static string[] _AllowAnonymousPath = new[] {
                                "/Home/Http403",
                                "/UserAccount/SignIn",
                                "/UserAccount/SignInLogic",
                                "/UserAccount/SignOut",
                                "/UserAccount/SignUp",
                                };

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(ChameleonSystemEnum chameleonSystemEnum, IServiceCollection services)
        {
            //添加授权
            services.AddAuthentication(s =>
            {
                //添加JWT Scheme
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //添加jwt验证：
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        //如果request请求中有，则直接从request获取
                        string tokenFromRequest = context.Request.Query[AccountConst.KEY_AccessToken];

                        if (!string.IsNullOrEmpty(tokenFromRequest))
                        {
                            context.Token = tokenFromRequest;
                            //set token to cookie
                            context.Response.Cookies.Append(AccountConst.KEY_AccessToken, tokenFromRequest);
                        }
                        //如果url没有找到，则降级从cookie获取
                        else
                        {
                            //如果cookie中有token，则直接从cookie获取
                            string tokenFromCookie = context.Request.Cookies[AccountConst.KEY_AccessToken];

                            if (!string.IsNullOrEmpty(tokenFromCookie))
                                context.Token = tokenFromCookie;
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();
                        //如果token验证失败，则跳转登陆地址
                        context.Response.Redirect(string.Concat(AccountConst.AccountSignInAndRedirectUrl, context.Request.IsHttps ? "https://" : "http://", context.Request.Host, context.Request.Path, context.Request.QueryString));

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //为了防止跳转链接中带token会影响切换账号，如果链接中有url，则重定向一次不带token的链接
                        if (!string.IsNullOrEmpty(context.Request.Query[AccountConst.KEY_AccessToken]))
                        {
                            context.Response.Redirect(UrlHelper.RemoveUrlParam(string.Concat(context.Request.Host, context.Request.Path, context.Request.QueryString), AccountConst.KEY_AccessToken));
                            return Task.CompletedTask;
                        }

                        if (chameleonSystemEnum == ChameleonSystemEnum.Account && context.HttpContext.Request.Path.HasValue)
                        {
                            //这里默认放行路径，不走角色过滤
                            //这几个路由是account里面的
                            if (_AllowAnonymousPath.Contains(context.HttpContext.Request.Path.Value))
                                return Task.CompletedTask;
                        }

                        //这里通过了token的校验，开始校验当前登陆用户有没有权限访问Account系统权限
                        var role = context.Principal.Claims.FirstOrDefault(t => t.Type.Equals(AccountConst.KEY_ChameleonRole))?.Value;

                        if (string.IsNullOrEmpty(role))
                            context.Response.Redirect(AccountConst.Http403Url);

                        if (chameleonSystemEnum == ChameleonSystemEnum.Account)
                        {
                            if (!new[] { RoleEnum.Administrator, RoleEnum.Developer }.Contains((RoleEnum)int.Parse(role)))
                                context.Response.Redirect(AccountConst.Http403Url);
                        }
                        else if (chameleonSystemEnum == ChameleonSystemEnum.Development)
                        {
                            if ((RoleEnum)int.Parse(role) != RoleEnum.Developer)
                                context.Response.Redirect(AccountConst.Http403Url);
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateAudience = true,//是否验证Audience
                    ValidAudience = AccountConfig.Instance.TokenAudience,//Audience
                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = AccountConfig.Instance.TokenIssuer,//Issuer，这两项和前面签发jwt的设置一致
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountConfig.Instance.SecurityKey))//拿到SecurityKey
                };
            });

            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                    policy
                    .WithOrigins(new[]
                    {
                        UrlsConfig.Instance.Account,
                        UrlsConfig.Instance.DataApi,
                        UrlsConfig.Instance.Development
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    )
                );

            //session support
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //inject dbContext
            services.AddScoped<ChameleonMetaDataDbContext>();
            services.AddScoped<ChameleonDataDbContext>();
            //inject repository
            services.AddScoped(typeof(CloudApplicationRepository).Assembly);
            //inject domain
            services.AddScoped(typeof(CloudApplicationService).Assembly);
            //inject application
            services.AddScoped(typeof(MetaObjectApp).Assembly);

            //inject component
            services.AddScoped<IDynamicScriptEngine, CSharpDynamicScriptEngine>();

            //防止中文乱码
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddHttpContextAccessor();
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void Configure(ChameleonSystemEnum chameleonSystemEnum, IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            ///添加jwt验证
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseCookiePolicy();

            app.UseCors();
        }
    }
}
