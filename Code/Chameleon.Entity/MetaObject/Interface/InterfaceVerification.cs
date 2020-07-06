using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

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
        /// 规则类型
        /// </summary>
        [Column]
        public int RegularType { get; set; }
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

        public RegularTypeEnum GetRegularType() => (RegularTypeEnum)this.RegularType;
    }

    public enum RegularTypeEnum
    {
        [Description("自定义")]
        Custom = 0,
        [Description("不为空")]
        NotNullOrEmpty = 1,
        [Description("邮箱")]
        Email = 2,
        [Description("手机")]
        TelPhone = 3
    }

    public class RegularTypeEnumHelper
    {
        public static IEnumerable<RegularTypeEnum> GetRegularTypeEnums()
        {
            foreach (var item in Enum.GetValues(typeof(RegularTypeEnum)))
            {
                yield return (RegularTypeEnum)item;
            }
        }

        /// <summary>
        /// 这里包含了支持的表达式类型
        /// </summary>
        private static Dictionary<RegularTypeEnum, string> RegularExpressionMapping = new Dictionary<RegularTypeEnum, string>
        {
            { RegularTypeEnum.NotNullOrEmpty,"" },
            { RegularTypeEnum.Email,"" },
            { RegularTypeEnum.TelPhone,"" },
        };

        public static string GetRegularExpressionByRegularType(RegularTypeEnum regularType)
        {
            return RegularExpressionMapping[regularType];
        }
    }
}
