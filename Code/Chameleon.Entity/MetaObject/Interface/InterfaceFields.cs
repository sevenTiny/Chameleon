using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 接口字段
    /// </summary>
    [Table]
    public class InterfaceFields : MetaObjectBase
    {
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
        /// 字段自定义显示名称（用于列表显示个性化的名称）
        /// 该字段选填
        /// </summary>
        [Column]
        public string MetaFieldCustomViewName { get; set; }
    }
}
