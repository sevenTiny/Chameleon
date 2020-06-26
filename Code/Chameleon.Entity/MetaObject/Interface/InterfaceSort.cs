using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.ComponentModel;

namespace Chameleon.Entity
{
    /// <summary>
    /// 接口排序
    /// </summary>
    [Table]
    public class InterfaceSort : MetaObjectBase
    {
        /// <summary>
        /// 所属接口排序，顶级节点没有为默认值
        /// </summary>
        [Column]
        public Guid ParentId { get; set; }
        /// <summary>
        /// 字段Id
        /// 该字段必填
        /// </summary>
        [Column]
        public Guid MetaFieldId { get; set; }
        /// <summary>
        /// 字段的短编码，和MetaFieldId的短编码一致
        /// 该字段必填
        /// </summary>
        [Column]
        public string MetaFieldShortCode { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        [Column]
        public int SortType { get; set; }

        public SortTypeEnum GetSortType() => (SortTypeEnum)this.SortType;
    }

    /// <summary>
    /// 排序方式
    /// </summary>
    public enum SortTypeEnum
    {
        [Description("倒序")]
        DESC = 0,
        [Description("正序")]
        ASC = 1
    }
}
