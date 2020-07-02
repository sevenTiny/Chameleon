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
    }
}
