using System;
using System.Collections.Generic;
using System.IO;

namespace Chameleon.Infrastructure
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 清理配置文件
        /// </summary>
        /// <returns></returns>
        public static string ClearConfigFiles()
        {
            //配置文件目录
            var configPath = Path.Combine(AppContext.BaseDirectory, "SevenTinyConfig");

            if (!Directory.Exists(configPath))
                return "没有配置文件需要清理";

            var deletedFiles = new List<string>();

            foreach (var item in Directory.GetFileSystemEntries(configPath))
            {
                if (File.Exists(item))
                {
                    deletedFiles.Add(item);

                    File.Delete(item);
                }
            }

            return $"清理配置文件成功，共清理配置文件[{deletedFiles.Count}]个，清理清单：{string.Join(",", deletedFiles)}";
        }
    }
}
