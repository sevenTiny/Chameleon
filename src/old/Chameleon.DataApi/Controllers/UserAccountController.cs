using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure.Consts;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ApiControllerCommonBase
    {
        IOrganizationService _organizationService;
        IUserAccountRepository _userAccountRepository;
        IUserAccountService _userAccountService;
        IUserAccountApp _userAccountApp;
        IProfileRepository _profileRepository;
        IMenuRepository _menuRepository;
        IFunctionService _functionService;
        IMenuService _menuService;
        public UserAccountController(IMenuService menuService, IFunctionService functionService, IUserAccountRepository userAccountRepository, IMenuRepository menuRepository, IProfileRepository profileRepository, IUserAccountService userAccountService, IUserAccountApp userAccountApp, IOrganizationService organizationService)
        {
            _userAccountApp = userAccountApp;
            _organizationService = organizationService;
            _menuService = menuService;
            _functionService = functionService;
            _userAccountRepository = userAccountRepository;
            _menuRepository = menuRepository;
            _profileRepository = profileRepository;
            _userAccountService = userAccountService;
        }

        private string GetTokenAndSaveCookie(UserAccount userAccount)
        {
            //get token
            var token = _userAccountService.GetToken(userAccount).Data;
            //set token to cookie
            Response.Cookies.Append(AccountConst.KEY_AccessToken, token);

            return token;
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
        /// 第三方登陆，返回授权token
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("SignInThirdParty")]
        public JsonResult SignInThirdParty([FromBody]LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                return Result.Error("参数错误").ToJsonResult();

            //如果校验密码成功，则会返回账号信息
            var checkResult = _userAccountService.VerifyPassword(null, loginRequest.Email, loginRequest.Password);

            if (!checkResult.IsSuccess)
                return Result.Error("账号或密码不正确").ToJsonResult();

            LoginResponse loginResult = new LoginResponse
            {
                AccessToken = GetTokenAndSaveCookie(checkResult.Data),
                IsNeedToResetPassword = checkResult.Data.IsNeedToResetPassword,
                TokenExpiredTimeStamp = TimeHelper.GetTimeStamp(DateTime.Now.AddDays(1)),
            };

            return Result<LoginResponse>.Success("登陆成功", loginResult).ToJsonResult();
        }

        /// <summary>
        /// 第三方修改密码
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPasswordThirdParty")]
        public JsonResult ResetPasswordThirdParty([FromBody]LoginRequest loginRequest)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(loginRequest, nameof(loginRequest))
                .ContinueEnsureArgumentNotNullOrEmpty(loginRequest.Email, nameof(loginRequest.Email))
                .ContinueEnsureArgumentNotNullOrEmpty(loginRequest.NewPassword, nameof(loginRequest.NewPassword))
                .ContinueAssert(_ => Regex.IsMatch(loginRequest.NewPassword, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$"), "必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-20之间")
                .Continue(_ =>
                {
                    return _userAccountService.ResetPassword(loginRequest.Email, loginRequest.NewPassword);
                });

            if (!result.IsSuccess)
                return result.ToJsonResult();

            return Result.Success("操作成功").ToJsonResult();
        }
    }
}