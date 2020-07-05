﻿using Chameleon.Entity;
using Chameleon.Infrastructure.Consts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Bootstrapper
{
    public class ApiControllerCommonBase : ControllerBase
    {
        protected JsonResult JsonResultSuccess(string msg = "操作成功")
        {
            return Result.Success(msg).ToJsonResult();
        }

        /// <summary>
        /// 从Token串中获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetArgumentFromToken(string key)
        {
            var auth = HttpContext.AuthenticateAsync()?.Result?.Principal?.Claims;
            var value = auth?.FirstOrDefault(t => t.Type.Equals(key))?.Value;

            if (string.IsNullOrEmpty(value))
                Response.Redirect(string.Concat(AccountConst.AccountSignInUrl, Request.Host, Request.Path));

            return value;
        }

        protected long CurrentUserId
        {
            get
            {
                return long.Parse(GetArgumentFromToken(AccountConst.KEY_UserId));
            }
        }

        protected string CurrentUserEmail
        {
            get
            {
                return GetArgumentFromToken(AccountConst.KEY_UserEmail);
            }
        }

        protected string CurrentUserName
        {
            get
            {
                return GetArgumentFromToken(AccountConst.KEY_UserName);
            }
        }

        protected int CurrentUserRole
        {
            get
            {
                return Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_ChameleonRole));
            }
        }

        /// <summary>
        /// 当前登陆人的组织
        /// </summary>
        protected Guid CurrentOrganization
        {
            get
            {
                return Guid.Parse(GetArgumentFromToken(AccountConst.KEY_Organization));
            }
        }
    }
}