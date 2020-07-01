using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 公共实体基类
    /// </summary>
    [Table]
    public class UserAccount
    {
        [Key]
        [Column]
        public Guid Id { get; set; }
        [Column("`Name`")]
        public string Name { get; set; }
        [Column("Phone")]
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
        [Column]
        public int IsDeleted { get; set; } = 0;
        [Column]
        public int CreateBy { get; set; } = 0;
        [Column("`CreateTime`")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [Column]
        public int ModifyBy { get; set; } = 0;
        [Column("`ModifyTime`")]
        public DateTime ModifyTime { get; set; } = DateTime.Now;
        [Column]
        public int TenantId { get; set; } = 0;
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

        public IsDeleted GetIsDeleted()
        {
            return (IsDeleted)this.IsDeleted;
        }
        public void SetIsDeleted(IsDeleted isDeleted)
        {
            this.IsDeleted = (int)isDeleted;
        }

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
}
