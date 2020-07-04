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
            //�����Ȩ
            services.AddAuthentication(s =>
            {
                //���JWT Scheme
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //���jwt��֤��
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        //���request�������У���ֱ�Ӵ�request��ȡ
                        string tokenFromRequest = context.Request.Query[AccountConst.KEY_AccessToken];

                        if (!string.IsNullOrEmpty(tokenFromRequest))
                            context.Token = tokenFromRequest;

                        //���cookie����token����ֱ�Ӵ�cookie��ȡ
                        string tokenFromCookie = context.Request.Cookies[AccountConst.KEY_AccessToken];

                        if (!string.IsNullOrEmpty(tokenFromCookie))
                            context.Token = tokenFromCookie;

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        //�˴�����Ϊ��ֹ.Net CoreĬ�ϵķ������ͺ����ݽ�����������ҪŶ������
                        context.HandleResponse();
                        //���token��֤ʧ�ܣ�����ת��½��ַ
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
                        //����ע���½Ҳû��Ȩ����
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

                        //����ͨ����token��У�飬��ʼУ�鵱ǰ��½�û���û��Ȩ�޷���AccountϵͳȨ��
                        var role = context.Principal.Claims.FirstOrDefault(t => t.Type.Equals(AccountConst.KEY_ChameleonRole))?.Value;

                        if (role == null || !new[] { RoleEnum.Administrator, RoleEnum.Deveolper }.Contains((RoleEnum)int.Parse(role)))
                            context.Response.Redirect($"/Home/Http403");

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateAudience = true,//�Ƿ���֤Audience
                    ValidAudience = AccountConfig.Instance.TokenAudience,//Audience
                    ValidateIssuer = true,//�Ƿ���֤Issuer
                    ValidIssuer = AccountConfig.Instance.TokenIssuer,//Issuer���������ǰ��ǩ��jwt������һ��
                    ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountConfig.Instance.SecurityKey))//�õ�SecurityKey
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

            //ע��ҵ���
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

            ///���jwt��֤
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
