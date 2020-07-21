using SevenTiny.Bantina.Configuration;

namespace Chameleon.Common.Configs
{
    [ConfigName("ConnectionStrings")]
    internal class ConnectionStringsConfig : MySqlColumnConfigBase<ConnectionStringsConfig>
    {
        [ConfigProperty]
        public string mongodb39911 { get; set; }
        [ConfigProperty]
        public string MultiTenantAccount { get; set; }
        [ConfigProperty]
        public string MultiTenantPlatformWeb { get; set; }
        [ConfigProperty]
        public string mysql39901 { get; set; }
        [ConfigProperty]
        public string Chameleon { get; set; }
    }
}
