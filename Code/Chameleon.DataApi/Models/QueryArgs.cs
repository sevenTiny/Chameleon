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
        /// <summary>
        /// 接口编码
        /// </summary>
        public string _interface { get; set; }
        public int _pageIndex { get; set; }
        public int _pageSize { get; set; }
        /// <summary>
        /// 文件id
        /// </summary>
        public string _fileId { get; set; }
    }
}
