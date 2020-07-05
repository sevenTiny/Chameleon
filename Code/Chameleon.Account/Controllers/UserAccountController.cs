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

namespace Chameleon.Account.Controllers
{
    public class UserAccountController : WebControllerBase
    {
        IUserAccountService _userAccountService;
        IUserAccountRepository _userAccountRepository;
        IOrganizationRepository _organizationRepository;
        public UserAccountController(IOrganizationRepository organizationRepository, IUserAccountRepository userAccountRepository, IUserAccountService userAccountService)
        {
            _organizationRepository = organizationRepository;
            _userAccountRepository = userAccountRepository;
            _userAccountService = userAccountService;
        }

        public IActionResult List()
        {
            var userList = _userAccountService.GetUserAccountList();
            return View(userList);
        }

        public IActionResult Add()
        {
            ViewData["Organization"] = _organizationRepository.GetListUnDeleted();
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
                    return _userAccountService.AddUserAccount(entity);
                });

            if (!result.IsSuccess)
            {
                ViewData["Organization"] = _organizationRepository.GetListUnDeleted();
                return View("Add", result.ToResponseModel(entity));
            }

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            ViewData["Organization"] = _organizationRepository.GetListUnDeleted();
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
                   });
               });

            if (!result.IsSuccess)
            {
                ViewData["Organization"] = _organizationRepository.GetListUnDeleted();
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

            if (string.IsNullOrEmpty(userAccount.Email) || string.IsNullOrEmpty(userAccount.Password) || string.IsNullOrEmpty(redirect))
                return View("SignIn", Result<UserAccount>.Error("参数错误", userAccount).ToResponseModel());

            //如果校验密码成功，则会返回账号信息
            var checkResult = _userAccountService.VerifyPassword(null, userAccount.Email, userAccount.Password);

            if (!checkResult.IsSuccess)
            {
                checkResult.Data = userAccount;
                return View("SignIn", checkResult.ToResponseModel());
            }

            //get token
            var token = _userAccountService.GetToken(checkResult.Data).Data;
            //set token to cookie
            Response.Cookies.Append(AccountConst.KEY_AccessToken, token);
            //concat url
            if (redirect.Contains('?'))
                redirect = $"{redirect}&{AccountConst.KEY_AccessToken}={token}";
            else
                redirect = $"{redirect}?{AccountConst.KEY_AccessToken}={token}";

            return Redirect(redirect);
        }

        public IActionResult SignOut()
        {
            Response.Cookies.Delete(AccountConst.KEY_AccessToken);

            return Redirect("/UserAccount/SignIn?redirect=/Home/Index");
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }
    }
}