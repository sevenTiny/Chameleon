using Chameleon.Bootstrapper;
using Chameleon.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chameleon.DataApi.Controllers
{
    public class SystemController : WebControllerCommonBase
    {
        /// <summary>
        /// 获取当前的日志
        /// </summary>
        /// <returns></returns>
        public IActionResult Current()
        {
            var currentLog = LoggerHelper.GetCurrentLog();
            return Content(currentLog);
        }
    }
}