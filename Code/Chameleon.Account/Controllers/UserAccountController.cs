using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chameleon.Domain;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Extensions.AspNetCore;
using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using Microsoft.AspNetCore.Authorization;
using Chameleon.Infrastructure.Consts;
using System.Text.RegularExpressions;
using Chameleon.Application;
using SevenTiny.Bantina.Extensions;

namespace Chameleon.Account.Controllers
{
    public class UserAccountController : WebControllerBase
    {
        IUserAccountService _userAccountService;
        IUserAccountRepository _userAccountRepository;
        IOrganizationRepository _organizationRepository;
        IOrganizationService _organizationService;
        IUserAccountApp _userAccountApp;
        IFileApp _fileApp;
        IProfileRepository _profileRepository;
        public UserAccountController(IProfileRepository profileRepository, IFileApp fileApp, IUserAccountApp userAccountApp, IOrganizationService organizationService, IOrganizationRepository organizationRepository, IUserAccountRepository userAccountRepository, IUserAccountService userAccountService)
        {
            _profileRepository = profileRepository;
            _fileApp = fileApp;
            _userAccountApp = userAccountApp;
            _organizationService = organizationService;
            _organizationRepository = organizationRepository;
            _userAccountRepository = userAccountRepository;
            _userAccountService = userAccountService;
        }

        public IActionResult List()
        {
            List<UserAccount> result = _userAccountService.GetUserAccountList();

            //如果不是开发者，则移除开发者人员的显示
            if (!IsDeveloper)
                result = result.Where(t => t.GetRole() != RoleEnum.Developer).ToList();

            return View(result);
        }

        public IActionResult Add()
        {
            ViewData["Organization"] = _organizationService.GetTreeNameList();
            ViewData["Profile"] = _profileRepository.GetListUnDeleted();
            UserAccount entity = new UserAccount();
            return View(ResponseModel.Success(data: entity));
        }

        public IActionResult AddLogic(UserAccount entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .Continue(_ =>
                {
                    if (!string.IsNullOrEmpty(entity.Phone))
                        return _.ContinueAssert(__ => entity.Phone.IsMobilePhone(), "手机号码不合法");
                    return _;
                })
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Email, nameof(entity.Email))
                .ContinueAssert(_ => entity.Email.IsEmail(), "邮箱不合法")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Organization, nameof(entity.Organization))
                .Continue(_ =>
                {
                    entity.CreateBy = CurrentUserId;
                    entity.Password = "Chameleon123456";
                    entity.IsNeedToResetPassword = 1;//手动添加的用户，下次登陆需要修改密码
                    return _userAccountApp.AddUserAccount(entity, _AccessToken);
                });

            if (!result.IsSuccess)
            {
                ViewData["Organization"] = _organizationService.GetTreeNameList();
                ViewData["Profile"] = _profileRepository.GetListUnDeleted();
                return View("Add", result.ToResponseModel(entity));
            }

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            GetUserRoleToViewData();
            ViewData["Organization"] = _organizationService.GetTreeNameList();
            ViewData["Profile"] = _profileRepository.GetListUnDeleted();
            var entity = _userAccountRepository.GetById(id);
            return View(ResponseModel.Success(data: entity));
        }

        public IActionResult UpdateLogic(UserAccount entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .Continue(_ =>
                {
                    if (!string.IsNullOrEmpty(entity.Phone))
                        return _.ContinueAssert(__ => entity.Phone.IsMobilePhone(), "手机号码不合法");
                    return _;
                })
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Email, nameof(entity.Email))
                .ContinueAssert(_ => entity.Email.IsEmail(), "邮箱不合法")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Organization, nameof(entity.Organization))
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _userAccountService.UpdateWithOutCode(entity, t =>
                   {
                       t.Email = entity.Email;
                       t.Phone = entity.Phone;
                       t.Name = entity.Name;
                       t.Organization = entity.Organization;
                       t.Role = entity.Role;
                       t.Identity = entity.Identity;
                   });
               });

            if (!result.IsSuccess)
            {
                GetUserRoleToViewData();
                ViewData["Organization"] = _organizationService.GetTreeNameList();
                ViewData["Profile"] = _profileRepository.GetListUnDeleted();
                return View("Update", result.ToResponseModel(entity));
            }

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _userAccountRepository.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }

        [AllowAnonymous]
        public IActionResult SignIn(string redirect)
        {
            ViewData["Redirect"] = redirect;
            return View();
        }

        private string GetReidrectFullPath(UserAccount userAccount, string redirect)
        {
            //get token
            var token = _userAccountService.GetToken(userAccount).Data;
            //set token to cookie
            Response.Cookies.Append(AccountConst.KEY_AccessToken, token);

            if (string.IsNullOrEmpty(redirect))
                redirect = "/Home/Index";

            //concat url
            if (redirect.Contains('?'))
            {
                redirect = $"{redirect}&{AccountConst.KEY_AccessToken}={token}";
            }
            else
            {
                redirect = redirect.Replace(@"\", "/");

                if (redirect.LastOrDefault().Equals('/'))
                    redirect = redirect.Substring(0, redirect.LastIndexOf('/'));

                redirect = $"{redirect}?{AccountConst.KEY_AccessToken}={token}";
            }

            return redirect;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult SignInLogic(UserAccount userAccount, string redirect)
        {
            ViewData["Redirect"] = redirect;

            if (string.IsNullOrEmpty(userAccount.Email) || string.IsNullOrEmpty(userAccount.Password))
                return View("SignIn", Result<UserAccount>.Error("参数错误", userAccount).ToResponseModel());

            //如果校验密码成功，则会返回账号信息
            var checkResult = _userAccountService.VerifyPassword(null, userAccount.Email, userAccount.Password);

            if (!checkResult.IsSuccess)
            {
                checkResult.Data = userAccount;
                return View("SignIn", checkResult.ToResponseModel());
            }

            //如果需要修改密码，则跳转到密码修改页面（这个操作是强制的，因为校验密码时候重置密码的也算校验成功，如果没有这步判断会有安全风险）
            if (checkResult.Data.IsNeedToResetPassword == 1)
                return Redirect($"/UserAccount/ResetPassword?redirect={redirect}&id={userAccount.Id}");

            //获取token并处理跳转地址
            redirect = GetReidrectFullPath(checkResult.Data, redirect);

            return Redirect(redirect);
        }

        /// <summary>
        /// 登陆（用于前端界面ajax处理）
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SignInLogicJson(UserAccount userAccount, string redirect)
        {
            ViewData["Redirect"] = redirect;

            if (string.IsNullOrEmpty(userAccount.Email) || string.IsNullOrEmpty(userAccount.Password))
                return Result<UserAccount>.Error("参数错误", userAccount).ToJsonResult();

            //如果校验密码成功，则会返回账号信息
            var checkResult = _userAccountService.VerifyPassword(null, userAccount.Email, userAccount.Password);

            if (!checkResult.IsSuccess)
                return checkResult.ToJsonResult();

            //如果需要修改密码，则跳转到密码修改页面（这个操作是强制的，因为校验密码时候重置密码的也算校验成功，如果没有这步判断会有安全风险）
            if (checkResult.Data.IsNeedToResetPassword == 1)
                return Result<string>.Success("该账号需要重置密码", $"/UserAccount/ResetPassword?redirect={redirect}&id={checkResult.Data.Id}").ToJsonResult();

            //获取token并处理跳转地址
            redirect = GetReidrectFullPath(checkResult.Data, redirect);

            return Result<string>.Success("登陆成功", redirect).ToJsonResult();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string id, string redirect)
        {
            ViewData["Id"] = id;
            ViewData["Redirect"] = redirect;
            return View();
        }

        [AllowAnonymous]
        public JsonResult ResetPasswordLogic(UserAccount entity, string redirect)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Id, nameof(entity.Id))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Password, nameof(entity.Password))
                .ContinueAssert(_ => Regex.IsMatch(entity.Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$"), "必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-20之间")
                .Continue(_ =>
                {
                    return _userAccountService.ResetPassword(entity.Id, entity.Password);
                });

            if (!result.IsSuccess)
                return result.ToJsonResult();

            return Result<string>.Success("重置成功", "/UserAccount/SignIn?redirect=" + redirect).ToJsonResult();
        }

        public IActionResult SignOut(string redirect)
        {
            Response.Cookies.Delete(AccountConst.KEY_AccessToken);

            return Redirect("/UserAccount/SignIn?redirect=" + redirect ?? "/Home/Index");
        }

        [AllowAnonymous]
        public IActionResult SignUp(string redirect)
        {
            ViewData["Redirect"] = redirect;
            return View();
        }

        [AllowAnonymous]
        public JsonResult SignUpLogic(UserAccount entity, string redirect)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Email, nameof(entity.Email))
                .ContinueAssert(_ => entity.Email.IsEmail(), "邮箱不合法")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Password, nameof(entity.Password))
                .ContinueAssert(_ => Regex.IsMatch(entity.Password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$"), "必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-20之间")
                .Continue(_ =>
                {
                    entity.Organization = Guid.Empty;
                    return _userAccountApp.AddUserAccount(entity, _AccessToken);
                });

            if (!result.IsSuccess)
                return result.ToJsonResult();

            return Result<string>.Success("注册成功", "/UserAccount/SignIn?redirect=" + redirect).ToJsonResult();
        }

        public IActionResult SetNextTimeResetPassword(Guid id)
        {
            return Result.Success("操作成功")
                .ContinueEnsureArgumentNotNullOrEmpty(id, nameof(id))
                .Continue(_ => _userAccountService.SetNextTimeResetPassword(id))
                .Continue(_ =>
                {
                    var userAccount = _userAccountService.GetById(id);
                    //发送注册成功消息
                    var result = new Chameleon.Common.InterfaceRequest(new Dictionary<string, string> { { "_AccessToken", _AccessToken } }).CloudDataGet($"ChameleonSystem.TDS.PasswordReset&userId={userAccount.UserId}");

                    return _;
                })
                .ToJsonResult();
        }

        public IActionResult UserAccountInfo(long userId)
        {
            SetUserInfoToViewData();

            var user = _userAccountRepository.GetUserAccountByUserId(userId);

            return View(ResponseModel.Success(data: user));
        }

        public IActionResult SaveUserAccountInfo(UserAccount entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _userAccountService.UpdateWithOutCode(entity, t =>
                   {
                       t.Name = entity.Name;

                       //如果头像不一致，则把旧的图片删掉
                       if (!string.IsNullOrEmpty(t.AvatarPicId) && !string.IsNullOrEmpty(entity.AvatarPicId) && !string.Equals(t.AvatarPicId, entity.AvatarPicId))
                           _fileApp.Delete(CurrentUserId, CurrentUserRole, CurrentOrganization, t.AvatarPicId);

                       t.AvatarPicId = entity.AvatarPicId;
                   });
               });

            if (result.IsSuccess)
                result.Message = "保存成功";

            GetUserRoleToViewData();
            return View("UserAccountInfo", result.ToResponseModel(entity));
        }
    }
}