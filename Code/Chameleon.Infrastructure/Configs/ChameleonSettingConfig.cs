using SevenTiny.Bantina.Configuration;

namespace Chameleon.Infrastructure.Configs
{
    [ConfigName("ChameleonSetting")]
    public class ChameleonSettingConfig : MySqlColumnConfigBase<ChameleonSettingConfig>
    {
        /// <summary>
        /// Mou管理单元是否开启
        /// </summary>
        [ConfigProperty]
        public int MouEnable { get; set; }
        /// <summary>
        /// 默认接口页大小
        /// </summary>
        [ConfigProperty]
        public int DefaultInterfacePageSize { get; set; } = 15;
        /// <summary>
        /// 允许的CorsOrigins多个英文逗号分隔
        /// </summary>
        [ConfigProperty]
        public string AllowCorsOrigins { get; set; }
    }
}
