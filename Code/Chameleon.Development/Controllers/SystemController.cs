using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chameleon.Development.Models;
using Chameleon.Infrastructure;

namespace Chameleon.Development.Controllers
{
    public class SystemController : WebControllerBase
    {
        private readonly ILogger<SystemController> _logger;

        public SystemController(ILogger<SystemController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取当前的日志
        /// </summary>
        /// <returns></returns>
        public IActionResult Current()
        {
            var currentLog = LoggerHelper.GetCurrentLog();
            return Content(currentLog);
        }

        /// <summary>
        /// 清理配置文件
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearConfig()
        {
            var msg = ConfigHelper.ClearConfigFiles();
            return Content(msg);
        }
    }
}
