using Chameleon.Bootstrapper;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Chameleon.Development.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Authorize]
    public class WebControllerBase : WebControllerCommonBase
    {
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
    }
}