using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 接口校验（用于各场景的参数校验）
    /// </summary>
    [Table]
    public class InterfaceVerification : MetaObjectBase
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
        /// 字段校验的正则表达式
        /// </summary>
        [Column]
        public string RegularExpression { get; set; }
    }
}
