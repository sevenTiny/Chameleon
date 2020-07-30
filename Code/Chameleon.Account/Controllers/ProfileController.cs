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
    public class ProfileController : WebControllerBase
    {
        IProfileService _ProfileService;
        IProfileRepository _ProfileRepository;
        IMenuRepository _menuRepository;
        IFunctionRepository _functionRepository;
        IMenuService _menuService;
        public ProfileController(IMenuService menuService, IFunctionRepository functionRepository, IMenuRepository menuRepository, IProfileRepository ProfileRepository, IProfileService ProfileService)
        {
            _menuService = menuService;
            _functionRepository = functionRepository;
            _menuRepository = menuRepository;
            _ProfileRepository = ProfileRepository;
            _ProfileService = ProfileService;
        }

        public IActionResult List()
        {
            return View(_ProfileRepository.GetListUnDeleted());
        }

        public IActionResult Add()
        {
            Profile Profile = new Profile();
            return View(ResponseModel.Success(data: Profile));
        }

        public IActionResult Setting(Guid profileId)
        {
            ViewData["ProfileId"] = profileId;

            var profile = _ProfileRepository.GetById(profileId);

            ViewData["PermissionMenu"] = new HashSet<string>(profile.PermissionMenu?.Split(',').ToArray() ?? new string[0]);
            ViewData["PermissionFunction"] = new HashSet<string>(profile.PermissionFunction?.Split(',').ToArray() ?? new string[0]);
            ViewData["Menus"] = _menuService.GetTreeNameList();
            ViewData["Functions"] = _functionRepository.GetListUnDeleted();
            return View();
        }

        public IActionResult SettingSave()
        {
            return null;
        }

        public IActionResult AddLogic(Profile entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.Code = entity.Code;
                    return _ProfileService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var Profile = _ProfileService.GetById(id);
            return View(ResponseModel.Success(data: Profile));
        }

        public IActionResult UpdateLogic(Profile entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _ProfileService.UpdateWithOutCode(entity, item =>
                   {
                       item.Code = entity.Code;
                   });
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _ProfileService.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }
    }
}