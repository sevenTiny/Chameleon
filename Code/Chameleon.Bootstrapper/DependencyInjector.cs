using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Chameleon.Bootstrapper
{
    /// <summary>
    /// 业务层依赖注入器
    /// </summary>
    public static class DependencyInjector
    {
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="services"></param>
        public static void Inject(IServiceCollection services)
        {
            //inject dbContext
            services.AddScoped<ChameleonMetaDataDbContext>();
            services.AddScoped<ChameleonDataDbContext>();
            //inject repository
            services.AddScoped(typeof(CloudApplicationRepository).Assembly);
            //inject domain
            services.AddScoped(typeof(CloudApplicationService).Assembly);
            //inject application
            services.AddScoped(typeof(MetaObjectApp).Assembly);
        }
    }
}
