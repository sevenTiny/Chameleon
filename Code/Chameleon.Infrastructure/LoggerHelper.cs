using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chameleon.Infrastructure
{
    public class LoggerHelper
    {
        /// <summary>
        /// 获取当前的日志
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLog()
        {
            //日志文件目录
            var logPath = Path.Combine(AppContext.BaseDirectory, "SevenTinyLogs", "log.log");
            if (!File.Exists(logPath))
                return string.Empty;

            return File.ReadAllText(logPath);
        }
    }
}
