﻿using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 组织
    /// </summary>
    [Table]
    public class Organization : CommonBase
    {
        /// <summary>
        /// 父组织id
        /// </summary>
        [Column]
        public Guid ParentId { get; set; }

        public List<Organization> Children { get; set; }
    }

    /// <summary>
    /// 和当前节点的关系
    /// </summary>
    public enum RelationEnum
    {
        [Description("成为当前选中的兄弟组织")]
        Brother = 0,
        [Description("成为当前选中的父组织")]
        Parent = 1,
        [Description("成为当前选中的子组织")]
        Child = 2
    }

    public static class RelationEnumHelper
    {
        public static RelationEnum[] GetRelationEnums()
        {
            return new[] { RelationEnum.Brother, RelationEnum.Parent, RelationEnum.Child };
        }
    }
}
