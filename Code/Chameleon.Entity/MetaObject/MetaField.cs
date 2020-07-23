using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.ComponentModel;

namespace Chameleon.Entity
{
    [Table]
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

        public DataType GetFieldType()
        {
            return (DataType)FieldType;
        }
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType : int
    {
        [Description("UnKnown")]
        Unknown = 0,
        [Description("文本")]
        Text = 2,
        [Description("日期时间")]
        DateTime = 3,
        [Description("日期")]
        Date = 4,
        [Description("布尔（true/false）")]
        Boolean = 5,
        [Description("32位整数")]
        Int32 = 6,
        [Description("64位整数")]
        Int64 = 7,
        [Description("小数（双精度）")]
        Double = 9,
        [Description("货币")]
        Decimal = 10,
        //[Description("数据源")]
        //CloudApplicationInterface = 11,
    }
}
