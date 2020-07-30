using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 身份
    /// </summary>
    [Table]
    public class Profile : CommonBase
    {
        /// <summary>
        /// 有权限的功能
        /// </summary>
        [Column]
        public string PermissionFunction { get; set; }
        /// <summary>
        /// 有权限的菜单
        /// </summary>
        [Column]
        public string PermissionMenu { get; set; }
    }
}
