using SevenTiny.Bantina.Configuration;

namespace Chameleon.Infrastructure.Configs
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
    }
}
