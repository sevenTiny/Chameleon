using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Chameleon.Bootstrapper;
using Chameleon.Entity;
using Chameleon.Infrastructure.Configs;
using Chameleon.Infrastructure.Consts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Chameleon.Account
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加策略鉴权模式
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Role", policy => policy.RequireAssertion(context =>
            //    {
            //        return true;
            //    }));
            //});
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
                            return Task.CompletedTask;
                        }
                        //如果cookie中有token，则直接从cookie获取
                        string tokenFromCookie = context.Request.Cookies[AccountConst.KEY_AccessToken];
                        if (!string.IsNullOrEmpty(tokenFromCookie))
                        {
                            context.Token = tokenFromCookie;
                            return Task.CompletedTask;
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();
                        //re-login
                        if (context.Response.StatusCode == 401)
                        {
                            //未登录重新登陆
                            context.Response.Redirect("/UserAccount/SignIn?redirect=/Home/Index");
                        }
                        else if (context.Response.StatusCode == 403)
                        {
                            //无权限跳转到拒绝页面
                            context.Response.Redirect("/Home/Http403");
                        }
                        else
                        {
                            //无权限跳转到拒绝页面
                            context.Response.Redirect("/Home/Http403");
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
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

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //start 7tiny ---
            //session support
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //注入业务层
            DependencyInjector.Inject(services);

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddMvc().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            //app.UseDefaultFiles().UseStaticFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            //app.Use((context, next) =>
            //{
            //    var value = context.AuthenticateAsync()?.Result?.Principal?.Claims;

            //    if (value == null)
            //        context.Response.Redirect($"/UserAccount/SignIn?redirect=/Home/Index");

            //    return Task.CompletedTask;
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
