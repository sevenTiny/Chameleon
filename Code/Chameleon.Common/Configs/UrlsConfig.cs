using SevenTiny.Bantina.Configuration;

namespace Chameleon.Common.Configs
{
    [ConfigName("Urls")]
    public class UrlsConfig : MySqlColumnConfigBase<UrlsConfig>
    {
        [ConfigProperty]
        public string DataApi { get; set; }
        [ConfigProperty]
        public string Development { get; set; }
        [ConfigProperty]
        public string Account { get; set; }
        /// <summary>
        /// 运行态站点
        /// </summary>
        [ConfigProperty]
        public string Office { get; set; }
    }
}
