using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.DataApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SystemController : ApiControllerCommonBase
    {
        IUserAccountService _userAccountService;
        IProfileRepository _profileRepository;
        IMenuRepository _menuRepository;
        IUserAccountRepository _userAccountRepository;
        IFunctionService _functionService;
        IMenuService _menuService;
        public SystemController(IMenuService menuService, IFunctionService functionService, IUserAccountRepository userAccountRepository, IMenuRepository menuRepository, IProfileRepository profileRepository, IUserAccountService userAccountService)
        {
            _menuService = menuService;
            _functionService = functionService;
            _userAccountRepository = userAccountRepository;
            _menuRepository = menuRepository;
            _profileRepository = profileRepository;
            _userAccountService = userAccountService;
        }

        [Route("ChameleonSystemInfo")]
        public IActionResult ChameleonSystemInfo()
        {
            return SafeExecute(() =>
            {
                var userAccount = _userAccountRepository.GetUserAccountByUserId(CurrentUserId);

                if (userAccount == null)
                    return Result.Error("用户不存在").ToJsonResult();

                var profile = _profileRepository.GetById(userAccount.Profile);

                if (profile == null)
                    return Result.Error("身份不正确").ToJsonResult();

                //有权限的功能
                var permissionFunctions = new HashSet<string>(_functionService.GetFunctionArrayFromString(profile.PermissionFunction));

                var allFunctions = _functionService.GetListUnDeleted();

                //有权限的菜单
                var permissionMenus = new HashSet<string>(_menuService.GetMenuArrayFromString(profile.PermissionMenu));

                var chameleonSystemInfo = new ChameleonSystemInfoDto
                {
                    UserId = userAccount.UserId,
                    UserEmail = userAccount.Email,
                    UserProfileId = userAccount.Profile,
                    UserRoleId = userAccount.Role,
                    AvatarPicId = userAccount.AvatarPicId,
                    ViewFunction = allFunctions.Where(t => permissionFunctions.Contains(t.Id.ToString())).Select(t => t.Code).ToList(),
                    ViewMenu = GetTree(permissionMenus)
                };

                return ResponseModel.Success(data: chameleonSystemInfo).ToJsonResult();
            });
        }

        private List<MenuView> GetTree(HashSet<string> permissionSet)
        {
            //获取所有子节点
            var nodes = _menuRepository.GetListUnDeleted();

            //获取顶级节点
            Menu condition = nodes?.FirstOrDefault(t => t.ParentId == Guid.Empty);

            if (condition == null)
                return new List<MenuView>(0);

            return GetTree(nodes, condition.Id);

            //Tree Search
            List<MenuView> GetTree(List<Menu> source, Guid parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId && permissionSet.Contains(t.Id.ToString()))
                    .OrderBy(t => t.SortNumber)
                    .Select(t => new MenuView { Id = t.Id, Name = t.Name, Route = t.Route })
                    .ToList();

                if (childs == null)
                    return new List<MenuView>(0);

                childs.ForEach(t => t.Children = GetTree(source, t.Id));

                return childs;
            }
        }

        /// <summary>
        /// 获取当前的日志
        /// </summary>
        /// <returns></returns>
        [Route("Log")]
        public IActionResult Log()
        {
            var currentLog = LoggerHelper.GetCurrentLog();
            return Content(currentLog);
        }

        /// <summary>
        /// 清理配置文件
        /// </summary>
        /// <returns></returns>
        [Route("ClearConfig")]
        public IActionResult ClearConfig()
        {
            var msg = ConfigHelper.ClearConfigFiles();
            return Content(msg);
        }
    }
}