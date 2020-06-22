using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chameleon.DataApi.Models
{
    public class QueryContext
    {
        /// <summary>
        /// 请求参数
        /// </summary>
        public Dictionary<string, string> ConditionArgumentsUpperKeyDic { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 接口设置
        /// </summary>
        public InterfaceSetting InterfaceSetting { get; set; }
    }
}
