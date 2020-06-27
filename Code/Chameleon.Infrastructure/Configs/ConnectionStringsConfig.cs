using SevenTiny.Bantina.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : MySqlColumnConfigBase<ConnectionStringsConfig>
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
