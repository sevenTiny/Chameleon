using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    //[DevelopmentAuthFilter]
    //[Authorize]
    public class WebControllerBase : Controller
    {
        protected JsonResult JsonResultSuccess(string msg = "操作成功")
        {
            return Result.Success(msg).ToJsonResult();
        }

        /// <summary>
        /// 将当前操作的应用信息存储到cookies中
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationCode"></param>
        protected void SetCookiesApplictionInfo(Guid applicationId, string applicationCode)
        {
            HttpContext.Response.Cookies.Append("ApplicationId", applicationId.ToString());
            HttpContext.Response.Cookies.Append("ApplicationCode", applicationCode);
        }

        /// <summary>
        /// Cookie中的应用Id
        /// </summary>
        protected Guid CurrentApplicationId
        {
            get
            {
                var applicationId = HttpContext.Request.Cookies["ApplicationId"];

                if (string.IsNullOrEmpty(applicationId))
                    Response.Redirect("/CloudApplication/Select");

                return Guid.Parse(applicationId);
            }
        }

        /// <summary>
        /// Cookie中的应用编码
        /// </summary>
        protected string CurrentApplicationCode
        {
            get
            {
                var applicationCode = HttpContext.Request.Cookies["ApplicationCode"];

                if (string.IsNullOrEmpty(applicationCode))
                    throw new ArgumentNullException("ApplicationCode is null,please check first!");

                return applicationCode;
            }
        }

        protected void SetCookiesMetaObjectInfo(Guid metaObjectId, string metaObjectCode)
        {
            HttpContext.Response.Cookies.Append("MetaObjectId", metaObjectId.ToString());
            HttpContext.Response.Cookies.Append("MetaObjectCode", metaObjectCode);
        }

        /// <summary>
        /// Cookie中的对象Id
        /// </summary>
        protected Guid CurrentMetaObjectId
        {
            get
            {
                var metaObjectId = HttpContext.Request.Cookies["MetaObjectId"];

                if (string.IsNullOrEmpty(metaObjectId))
                    throw new ArgumentNullException("MetaObjectId is null,please check first!");

                return Guid.Parse(metaObjectId);
            }
        }

        /// <summary>
        /// Cookie中的对象编码
        /// </summary>
        protected string CurrentMetaObjectCode
        {
            get
            {
                var metaObjectCode = HttpContext.Request.Cookies["MetaObjectCode"];

                if (string.IsNullOrEmpty(metaObjectCode))
                    throw new ArgumentNullException("MetaObjectCode is null,please check first!");

                return metaObjectCode;
            }
        }

        ///// <summary>
        ///// 请求上下文信息
        ///// </summary>
        //private ApplicationContext _applicationContext;
        //protected ApplicationContext CurrentApplicationContext
        //{
        //    get
        //    {
        //        if (_applicationContext == null)
        //        {
        //            _applicationContext = new ApplicationContext
        //            {
        //                ApplicationCode = CurrentApplicationCode,
        //                TenantId = CurrentTenantId,
        //                UserId = CurrentUserId,
        //                UserEmail = CurrentUserEmail
        //            };
        //        }
        //        return _applicationContext;
        //    }
        //}


        ///// <summary>
        ///// 从Token串中获取参数
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //private string GetArgumentFromToken(string key)
        //{
        //    return HttpContext.GetArgumentFromToken(key);
        //}

        //protected int CurrentTenantId
        //{
        //    get
        //    {
        //        var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_TenantId));

        //        if (result <= 0)
        //            Response.Redirect("/UserAccount/Login");

        //        return result;
        //    }
        //}

        //protected string CurrentTenantName
        //{
        //    get
        //    {
        //        var result = GetArgumentFromToken(AccountConst.KEY_TenantName);
        //        return result ?? CurrentTenantId.ToString();
        //    }
        //}

        protected int CurrentUserId
        {
            get
            {
                return 0;

                //var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_UserId));

                //if (result <= 0)
                //    Response.Redirect($"{UrlsConfig.Instance.Account}/UserAccount/Login?_redirectUrl={UrlsConfig.Instance.DevelopmentWebUrl}/Home/Index");

                //return result;
            }
        }

        //protected string CurrentUserEmail
        //{
        //    get
        //    {
        //        var result = GetArgumentFromToken(AccountConst.KEY_UserEmail);

        //        if (string.IsNullOrEmpty(result))
        //            Response.Redirect("/UserAccount/Login");

        //        return result;
        //    }
        //}

        //protected string CurrentUserName
        //{
        //    get
        //    {
        //        var result = GetArgumentFromToken(AccountConst.KEY_UserName);
        //        return result ?? CurrentUserEmail;
        //    }
        //}

        //protected int CurrentIdentity
        //{
        //    get
        //    {
        //        var result = Convert.ToInt32(GetArgumentFromToken(AccountConst.KEY_SystemIdentity));
        //        return result;
        //    }
        //}

        ///// <summary>
        ///// 将用户信息存到ViewData里面用于页面展示
        ///// </summary>
        //protected void SetUserInfoToViewData()
        //{
        //    ViewData["UserIdentity"] = SystemIdentityTranslator.ToChinese(CurrentIdentity);
        //    ViewData["UserName"] = CurrentUserName;
        //}
    }
}