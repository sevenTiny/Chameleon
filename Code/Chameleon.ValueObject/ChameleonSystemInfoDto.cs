using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chameleon.ValueObject
{
    /// <summary>
    /// Chameleon系统信息
    /// </summary>
    public class ChameleonSystemInfoDto
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// 用户头像Id
        /// </summary>
        public string AvatarPicId { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public int UserRoleId { get; set; }
        /// <summary>
        /// 用户身份
        /// </summary>
        public Guid UserProfileId { get; set; }
        /// <summary>
        /// 有权限的菜单
        /// </summary>
        public List<MenuView> ViewMenu { get; set; }
        /// <summary>
        /// 有权限的功能
        /// </summary>
        public List<ViewFunction> ViewFunction { get; set; }
    }

    public class ViewFunction
    {
        public string Code { get; set; }
    }

    public class MenuView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }

        public List<MenuView> Children { get; set; }
    }
}
