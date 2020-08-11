using System;

namespace Chameleon.ValueObject
{
    /// <summary>
    /// 登陆请求体
    /// </summary>
    public class LoginRequest
    {
        public string AppId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

    public class LoginResponse
    {
        /// <summary>
        /// 是否需要重置密码
        /// </summary>
        public int IsNeedToResetPassword { get; set; }
        /// <summary>
        /// token，用于异步返回第三方登陆
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// token过期时间戳
        /// </summary>
        public long TokenExpiredTimeStamp { get; set; }
    }
}
