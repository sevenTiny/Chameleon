using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chameleon.DataApi.Models
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryArgs
    {
        public string _interface { get; set; }
        public int _pageIndex { get; set; }
        public int _pageSize { get; set; }
    }
}
