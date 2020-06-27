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
        /// 所属接口校验，顶级节点没有接口校验
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
        /// 字段校验的正则表达式
        /// </summary>
        [Column]
        public string RegularExpression { get; set; }
        /// <summary>
        /// 校验失败时的提示消息
        /// </summary>
        [Column]
        public string VerificationTips { get; set; }
    }
}
