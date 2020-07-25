using Chameleon.Bootstrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;

namespace Chameleon.Development.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Authorize]
    public class WebControllerBase : WebControllerCommonBase
    {
        private void StorageKeyValue(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

        private string GetStorage(string key)
        {
            return HttpContext.Session.GetString(key);
        }

        /// <summary>
        /// 将当前操作的应用信息存储到session中
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationCode"></param>
        protected void SetCookiesApplictionInfo(Guid applicationId, string applicationCode)
        {
            StorageKeyValue("ApplicationId", applicationId.ToString());
            StorageKeyValue("ApplicationCode", applicationCode);
        }

        /// <summary>
        /// Cookie中的应用Id
        /// </summary>
        protected Guid CurrentApplicationId
        {
            get
            {
                var applicationId = GetStorage("ApplicationId");

                if (string.IsNullOrEmpty(applicationId))
                {
                    Response.Redirect("/Home/Index");
                    return Guid.Empty;
                }

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
                var applicationCode = GetStorage("ApplicationCode");

                if (string.IsNullOrEmpty(applicationCode))
                    throw new ArgumentNullException("ApplicationCode is null,please check first!");

                return applicationCode;
            }
        }

        protected void SetCookiesMetaObjectInfo(Guid metaObjectId, string metaObjectCode)
        {
            StorageKeyValue("MetaObjectId", metaObjectId.ToString());
            StorageKeyValue("MetaObjectCode", metaObjectCode);
        }

        /// <summary>
        /// Cookie中的对象Id
        /// </summary>
        protected Guid CurrentMetaObjectId
        {
            get
            {
                var metaObjectId = GetStorage("MetaObjectId");

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
                var metaObjectCode = GetStorage("MetaObjectCode");

                if (string.IsNullOrEmpty(metaObjectCode))
                    throw new ArgumentNullException("MetaObjectCode is null,please check first!");

                return metaObjectCode;
            }
        }
    }
}