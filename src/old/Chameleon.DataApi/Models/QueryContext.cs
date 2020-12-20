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
        /// <summary>
        /// 将要执行的脚本库，一次查出来
        /// </summary>
        public List<TriggerScript> TriggerScripts { get; set; }
        //贯穿触发器执行方法的上下文，常用于从before往after传递参数
        public Dictionary<string, string> TriggerContext { get; set; } = new Dictionary<string, string>();
    }
}
