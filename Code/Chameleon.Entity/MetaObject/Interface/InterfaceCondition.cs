using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Chameleon.Infrastructure;


namespace Chameleon.Entity
{
    /// <summary>
    /// 接口条件（用户查询/修改/删除等操作）
    /// </summary>
    [Table]
    public class InterfaceCondition : MetaObjectBase
    {
        /// <summary>
        /// 所属条件，一个接口条件下可能有很多子条件，用这个确认是属于当前条件下的数据
        /// </summary>
        [Column]
        public Guid BelongToCondition { get; set; }
        /// <summary>
        /// 父节点id，首个节点的父节点id为接口条件id
        /// </summary>
        [Column]
        public Guid ParentId { get; set; }
        /// <summary>
        /// 节点类型，表明是属于连接类型还是条件类型
        /// </summary>
        [Column]
        public int ConditionNodeType { get; set; }
        /// <summary>
        /// 所属字段id，连接节点没有所属字段id
        /// </summary>
        [Column]
        public Guid MetaFieldId { get; set; }
        /// <summary>
        /// 所属字段编码
        /// </summary>
        [Column]
        public string MetaFieldShortCode { get; set; }
        /// <summary>
        /// 条件类型（大于，小于，大于等于，包含等）
        /// </summary>
        [Column]
        public int ConditionType { get; set; }
        /// <summary>
        /// 条件连接类型（And|Or）
        /// </summary>
        [Column]
        public int ConditionJointType { get; set; }
        /// <summary>
        /// 参数值类型（参数传递|固定值）
        /// </summary>
        [Column]
        public int ConditionValueType { get; set; }
        /// <summary>
        /// 如果是固定值，则采取该字段的值匹配
        /// </summary>
        [Column]
        public string ConditionValue { get; set; }

        public NodeTypeEnum GetConditionNodeType()
        {
            return (NodeTypeEnum)ConditionNodeType;
        }
        public void SetConditionNodeType(NodeTypeEnum value)
        {
            this.ConditionNodeType = (int)value;
        }

        public ConditionJointTypeEnum GetConditionJointType()
        {
            return (ConditionJointTypeEnum)this.ConditionJointType;
        }
        public void SetConditionJointType(ConditionJointTypeEnum value)
        {
            this.ConditionJointType = (int)value;
        }

        public ConditionTypeEnum GetConditionType()
        {
            return (ConditionTypeEnum)this.ConditionType;
        }
        public void SetConditionType(ConditionTypeEnum value)
        {
            this.ConditionType = (int)value;
        }

        public ConditionValueTypeEnum GetConditionValueType()
        {
            return (ConditionValueTypeEnum)this.ConditionValueType;
        }
        public void SetConditionValueType(ConditionValueTypeEnum value)
        {
            this.ConditionValueType = (int)value;
        }

        public List<InterfaceCondition> Children { get; set; }
    }

    public enum NodeTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        /// <summary>
        /// 连接节点
        /// </summary>
        [Description("连接节点")]
        Joint = 1,
        /// <summary>
        /// 条件节点
        /// </summary>
        [Description("条件节点")]
        Condition = 2
    }

    /// <summary>
    /// 条件连接符
    /// </summary>
    public enum ConditionJointTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        /// <summary>
        /// &&
        /// </summary>
        [Description("&&")]
        And = 1,
        /// <summary>
        /// ||
        /// </summary>
        [Description("||")]
        Or = 2
    }

    /// <summary>
    /// 搜索条件类型
    /// </summary>
    public enum ConditionTypeEnum : int
    {
        [Description("UnKnown")]
        UnKnown = 0,
        /// <summary>
        /// ==
        /// </summary>
        [Description("==")]
        Equal = 1,
        /// <summary>
        /// !=
        /// </summary>
        [Description("!=")]
        NotEqual = 2,
        /// <summary>
        /// >
        /// </summary>
        [Description(">")]
        GreaterThan = 3,
        /// <summary>
        /// <
        /// </summary>
        [Description("<")]
        LessThan = 4,
        /// <summary>
        /// >=
        /// </summary>
        [Description(">=")]
        GreaterThanEqual = 5,
        /// <summary>
        /// <=
        /// </summary>
        [Description("<=")]
        LessThanEqual = 6
    }

    /// <summary>
    /// 条件值类型
    /// </summary>
    public enum ConditionValueTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        /// <summary>
        /// 参数传递
        /// </summary>
        [Description("参数传递值")]
        Parameter = 1,
        /// <summary>
        /// 常量（固定的数值）
        /// </summary>
        [Description("固定值")]
        FixedValue = 2
    }
}