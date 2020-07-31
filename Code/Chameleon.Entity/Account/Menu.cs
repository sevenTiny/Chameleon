using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Table]
    public class Menu : CommonBase
    {
        /// <summary>
        /// 父菜单id
        /// </summary>
        [Column]
        public Guid ParentId { get; set; }
        /// <summary>
        /// 菜单的图标
        /// </summary>
        [Column]
        public string Icon { get; set; }
        /// <summary>
        /// 菜单路由
        /// </summary>
        [Column]
        public string Route { get; set; }

        public List<Menu> Children { get; set; }
    }

    /// <summary>
    /// 和当前节点的关系
    /// </summary>
    public enum MenuRelationEnum
    {
        [Description("成为当前选中的子菜单")]
        Child = 1,
        [Description("成为当前选中的父菜单")]
        Parent = 2,
    }

    public static class MenuRelationEnumHelper
    {
        public static MenuRelationEnum[] GetRelationEnums()
        {
            return new[] { MenuRelationEnum.Child, MenuRelationEnum.Parent };
        }
    }
}
