using SevenTiny.Bantina.Configuration;

namespace Chameleon.Infrastructure.Configs
{
    [ConfigName("Account")]
    public class AccountConfig : MySqlColumnConfigBase<AccountConfig>
    {
        /// <summary>
        /// 密钥
        /// </summary>
        [ConfigProperty]
        public string SecurityKey { get; set; }
        /// <summary>
        /// token 过期分钟数
        /// </summary>
        [ConfigProperty]
        public int TokenExpiredMinutesTimeSpan { get; set; }
        [ConfigProperty]
        public string TokenIssuer { get; set; }
        [ConfigProperty]
        public string TokenAudience { get; set; }
    }
}
