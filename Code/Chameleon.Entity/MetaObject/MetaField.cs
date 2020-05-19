using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace Chameleon.Entity
{
    [Table]
    [TableCaching]
    public class MetaField : MetaObjectBase
    {
        /// <summary>
        /// 不带对象编码的字段短编码
        /// </summary>
        [Column]
        public string ShortCode { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [Column]
        public int FieldType { get; set; }
        /// <summary>
        /// 数据源Id，如果类型是数据源的话
        /// </summary>
        [Column]
        public Guid DataSourceId { get; set; }
        /// <summary>
        /// 是否系统字段
        /// </summary>
        [Column]
        public int IsSystem { get; set; }
    }
}
