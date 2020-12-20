using Chameleon.Bootstrapper;
using Chameleon.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Chameleon.DataApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SystemController : ApiControllerCommonBase
    {
        public SystemController()
        {
        }

        /// <summary>
        /// 获取当前的日志
        /// </summary>
        /// <returns></returns>
        [Route("Log")]
        public IActionResult Log()
        {
            var currentLog = LoggerHelper.GetCurrentLog();
            return Content(currentLog);
        }

        /// <summary>
        /// 清理配置文件
        /// </summary>
        /// <returns></returns>
        [Route("ClearConfig")]
        public IActionResult ClearConfig()
        {
            var msg = ConfigHelper.ClearConfigFiles();
            return Content(msg);
        }
    }
}