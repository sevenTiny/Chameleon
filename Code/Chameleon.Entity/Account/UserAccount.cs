using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 用户账号
    /// </summary>
    [Table]
    public class UserAccount : CommonBase
    {
        [Column]
        public string Phone { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Password { get; set; }
        /// <summary>
        /// 是否需要重置密码？如果值为1，那么下次登陆会提示重置密码
        /// </summary>
        [Column]
        public int IsNeedToResetPassword { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Column]
        public int Role { get; set; }
        /// <summary>
        /// 身份
        /// </summary>
        [Column]
        public int Identity { get; set; }
        /// <summary>
        /// 所属组织
        /// </summary>
        [Column]
        public Guid Organization { get; set; }

        /// <summary>
        /// 翻译用
        /// </summary>
        public string OrganizationView { get; set; }

        public RoleEnum GetRole() => (RoleEnum)this.Role;
    }

    /// <summary>
    /// 角色
    /// </summary>
    public enum RoleEnum
    {
        [Description("用户")]
        User = 0,
        [Description("管理员")]
        Administrator = 1,
        [Description("开发人员")]
        Deveolper = 2,
    }

    public static class RoleEnumHelper
    {
        public static RoleEnum[] GetRelationEnums()
        {
            return new[] { RoleEnum.User, RoleEnum.Administrator };
        }
    }
}
