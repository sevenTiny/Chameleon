﻿using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Infrastructure.Consts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Linq;

namespace Chameleon.Bootstrapper
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [EnableCors]
    public class WebControllerCommonBase : Controller
    {
        public WebControllerCommonBase()
        {

        }

        protected JsonResult JsonResultSuccess(string msg = "操作成功") => Result.Success(msg).ToJsonResult();

        private string GetArgumentFromTokenCanNull(string key)
        {
            var auth = HttpContext.AuthenticateAsync()?.Result?.Principal?.Claims;
            var value = auth?.FirstOrDefault(t => t.Type.Equals(key))?.Value;

            //把token中的kv存入Session
            HttpContext.Session.SetString(key, value);

            return value;
        }

        /// <summary>
        /// 从Token串中获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetArgumentFromToken(string key)
        {
            var value = GetArgumentFromTokenCanNull(key);

            if (string.IsNullOrEmpty(value))
                Response.Redirect(string.Concat(AccountConst.AccountSignInAndRedirectUrl, Request.Host, Request.Path));

            return value;
        }

        protected long CurrentUserId => long.Parse(GetArgumentFromToken(AccountConst.KEY_UserId));

        protected string CurrentUserEmail => GetArgumentFromToken(AccountConst.KEY_UserEmail);

        protected string CurrentUserName => GetArgumentFromToken(AccountConst.KEY_UserName);

        protected int CurrentUserRole => Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_ChameleonRole));
        protected int CurrentUserProfile => Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_Profile));

        protected void GetUserRoleToViewData()
        {
            ViewData["UserRole"] = CurrentUserRole;
        }

        /// <summary>
        /// 当前登陆人的组织
        /// </summary>
        protected Guid CurrentOrganization => Guid.Parse(GetArgumentFromToken(AccountConst.KEY_Organization));

        /// <summary>
        /// 是否开发人员
        /// </summary>
        protected bool IsDeveloper => RoleEnum.Developer == (RoleEnum)CurrentUserRole;

        /// <summary>
        /// 将用户信息存到ViewData里面用于页面展示
        /// </summary>
        protected void SetUserInfoToViewData()
        {
            ViewData["IsDeveloper"] = IsDeveloper;
            ViewData["UserRole"] = ((RoleEnum)CurrentUserRole).GetDescription();
            ViewData["UserName"] = CurrentUserName;
            ViewData["UserEmail"] = CurrentUserEmail;
            ViewData["UserId"] = CurrentUserId;
            ViewData["AvatarPicId"] = GetArgumentFromTokenCanNull(AccountConst.KEY_AvatarPicId);
        }

        /// <summary>
        /// 请求的token
        /// </summary>
        protected string _AccessToken
        {
            get
            {
                return HttpContext.Request.Cookies[AccountConst.KEY_AccessToken];
            }
        }
    }
}