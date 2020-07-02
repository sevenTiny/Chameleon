using Chameleon.Bootstrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Chameleon.Development
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
            //add jwt
            //services
            //.AddAuthentication(s =>
            //{
            //    //���JWT Scheme
            //    s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            ////���jwt��֤��
            //.AddJwtBearer(options =>
            //{
            //    options.Events = new JwtBearerEvents()
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            //���request�������У���ֱ�Ӵ�request��ȡ
            //            string tokenFromRequest = context.Request.Query[AccountConst.KEY_AccessToken];
            //            if (!string.IsNullOrEmpty(tokenFromRequest))
            //            {
            //                context.Token = tokenFromRequest;
            //                context.Response.Cookies.Append(AccountConst.KEY_AccessToken, tokenFromRequest);
            //                return Task.CompletedTask;
            //            }
            //            //���cookie����token����ֱ�Ӵ�cookie��ȡ
            //            string tokenFromCookie = context.Request.Cookies[AccountConst.KEY_AccessToken];
            //            if (!string.IsNullOrEmpty(tokenFromCookie))
            //            {
            //                context.Token = tokenFromCookie;
            //                return Task.CompletedTask;
            //            }
            //            return Task.CompletedTask;
            //        },
            //        OnChallenge = context =>
            //        {
            //            //�˴�����Ϊ��ֹ.Net CoreĬ�ϵķ������ͺ����ݽ�����������ҪŶ������
            //            context.HandleResponse();
            //            //re-login
            //            if (context.Response.StatusCode == 401)
            //            {
            //                //δ��¼���µ�½
            //                context.Response.Redirect($"{UrlsConfig.Instance.Account}/UserAccount/Login?_httpCode=401&_redirectUrl={UrlsConfig.Instance.DevelopmentWebUrl}/Home/Index");
            //            }
            //            else if (context.Response.StatusCode == 403)
            //            {
            //                throw new Exception("xxx");
            //                //��Ȩ����ת���ܾ�ҳ��
            //                context.Response.Redirect("/Home/HTTP403?_redirectUrl=/Home/Index");
            //            }
            //            else
            //            {
            //                //δ��¼���µ�½
            //                context.Response.Redirect($"{UrlsConfig.Instance.Account}/UserAccount/Login?_httpCode=401&_redirectUrl={UrlsConfig.Instance.DevelopmentWebUrl}/Home/Index");
            //            }
            //            return Task.CompletedTask;
            //        },
            //        OnAuthenticationFailed = context =>
            //        {
            //            //Token expired
            //            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            //            {
            //                context.Response.Headers.Add("Token-Expired", "true");
            //            }
            //            return Task.CompletedTask;
            //        },
            //        OnTokenValidated = context =>
            //        {
            //            return Task.CompletedTask;
            //        }
            //    };
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
            //        ClockSkew = TimeSpan.FromSeconds(30),
            //        ValidateAudience = true,//�Ƿ���֤Audience
            //        ValidAudience = AccountConfig.Instance.TokenAudience,//Audience
            //        ValidateIssuer = true,//�Ƿ���֤Issuer
            //        ValidIssuer = AccountConfig.Instance.TokenIssuer,//Issuer���������ǰ��ǩ��jwt������һ��
            //        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountConfig.Instance.SecurityKey))//�õ�SecurityKey
            //    };
            //});

            //session support
            services.AddDistributedMemoryCache();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromDays(1);
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

            //session support
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
