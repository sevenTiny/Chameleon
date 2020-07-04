using Chameleon.Bootstrapper;
using Chameleon.Entity;
using Chameleon.Infrastructure.Configs;
using Chameleon.Infrastructure.Consts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

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
                            context.Token = tokenFromRequest;

                        //如果cookie中有token，则直接从cookie获取
                        string tokenFromCookie = context.Request.Cookies[AccountConst.KEY_AccessToken];

                        if (!string.IsNullOrEmpty(tokenFromCookie))
                            context.Token = tokenFromCookie;

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();
                        //如果token验证失败，则跳转登陆地址
                        context.Response.Redirect("/UserAccount/SignIn?redirect=/Home/Index");

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
                        //这里注册登陆也没有权限了
                        if (context.HttpContext.Request.Path.HasValue)
                        {
                            if (new[] {
                                "/Home/Http403",
                                "/UserAccount/SignIn",
                                "/UserAccount/SignInLogic",
                                "/UserAccount/SignOut",
                                "/UserAccount/SignUp",
                            }.Contains(context.HttpContext.Request.Path.Value))
                                return Task.CompletedTask;
                        }

                        //这里通过了token的校验，开始校验当前登陆用户有没有权限访问Account系统权限
                        var role = context.Principal.Claims.FirstOrDefault(t => t.Type.Equals(AccountConst.KEY_ChameleonRole))?.Value;

                        if (role == null || !new[] { RoleEnum.Administrator, RoleEnum.Deveolper }.Contains((RoleEnum)int.Parse(role)))
                            context.Response.Redirect($"/Home/Http403");

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

            ///添加jwt验证
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
