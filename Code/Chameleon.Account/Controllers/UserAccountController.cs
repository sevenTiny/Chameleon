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

namespace Chameleon.Account.Controllers
{
    public class UserAccountController : WebControllerBase
    {
        IUserAccountService _userAccountService;
        IUserAccountRepository _userAccountRepository;
        public UserAccountController(IUserAccountRepository userAccountRepository, IUserAccountService userAccountService)
        {
            _userAccountRepository = userAccountRepository;
            _userAccountService = userAccountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            var userList = _userAccountRepository.GetUserAccountList();
            return View(userList);
        }

        public IActionResult Add()
        {
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
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
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
                   return _userAccountService.UpdateWithId(entity.Id, t =>
                   {
                       t.Email = entity.Email;
                       t.Phone = entity.Phone;
                       t.Name = entity.Name;
                   });
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _userAccountRepository.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }
    }
}