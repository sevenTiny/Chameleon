using Chameleon.Infrastructure.Configs;
using System;

namespace Chameleon.Infrastructure.Consts
{
    public static class AccountConst
    {
        /// <summary>
        /// 数值前面的盐值，建议后面再拼接
        /// </summary>
        public const string SALT_BEFORE = "seventiny.cloud.account.salt.";
        /// <summary>
        /// 数值后面的盐值
        /// </summary>
        public const string SALT_AFTER = ".25913AEE-8F27-49DB-89AA-AD730CAB58F1";

        public const string KEY_UserId = "UserId";
        public const string KEY_UserEmail = "UserEmail";
        public const string KEY_UserName = "UserName";
        public const string KEY_AccessToken = "_AccessToken";
        public const string KEY_ChameleonRole = "ChameleonRole";
        public const string KEY_Organization = "Organization";

        //默认密码
        public static string DefaultPassword = "Chameleon123456";

        //根组织
        public static Guid TopOrganization => Guid.Parse("99999999-9999-9999-9999-999999999999");

        /// <summary>
        /// 登陆Url
        /// </summary>
        public static string AccountSignInUrl = string.Concat(UrlsConfig.Instance.Account, "/UserAccount/SignIn");
        /// <summary>
        /// 登陆并跳转的url
        /// </summary>
        public static string AccountSignInAndRedirectUrl = string.Concat(UrlsConfig.Instance.Account, "/UserAccount/SignIn?redirect=");
        /// <summary>
        /// 403页面地址
        /// </summary>
        public static string Http403Url = string.Concat(UrlsConfig.Instance.Account, "/Home/Http403");
        /// <summary>
        /// 切换开发态用户地址
        /// </summary>
        public static string SwitchDevelopmentAccountUrl = string.Concat(AccountSignInAndRedirectUrl, UrlsConfig.Instance.Development);
        /// <summary>
        /// 切换Account系统用户地址
        /// </summary>
        public static string SwitchAccountAccountUrl = string.Concat(AccountSignInAndRedirectUrl, UrlsConfig.Instance.Account);
        /// <summary>
        /// 退出开发态地址
        /// </summary>
        public static string SignOutDevelopmentUrl = string.Concat(UrlsConfig.Instance.Account, "/UserAccount/SignOut?redirect=", UrlsConfig.Instance.Development);
        /// <summary>
        /// 退出account地址
        /// </summary>
        public static string SignOutAccountUrl = string.Concat(UrlsConfig.Instance.Account, "/UserAccount/SignOut?redirect=", UrlsConfig.Instance.Account);
    }
}
