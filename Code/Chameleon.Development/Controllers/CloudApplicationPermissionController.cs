using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.Development.Controllers
{
    public class CloudApplicationPermissionController : WebControllerBase
    {
        ICloudApplicationPermissionRepository _cloudApplicationPermissionRepository;
        ICloudApplicationPermissionService _cloudApplicationPermissionService;
        IUserAccountRepository _userAccountRepository;
        public CloudApplicationPermissionController(IUserAccountRepository userAccountRepository, ICloudApplicationPermissionService cloudApplicationPermissionService, ICloudApplicationPermissionRepository cloudApplicationPermissionRepository)
        {
            _userAccountRepository = userAccountRepository;
            _cloudApplicationPermissionService = cloudApplicationPermissionService;
            _cloudApplicationPermissionRepository = cloudApplicationPermissionRepository;
        }

        public IActionResult List()
        {
            var list = _cloudApplicationPermissionService.GetCloudApplicationPermissionsByApplicationIdToView(CurrentApplicationId);
            return View(list);
        }

        public IActionResult Add()
        {
            ViewData["UserAccounts"] = _userAccountRepository.GetDeveloperUserAccount();
            return View();
        }

        public IActionResult AddLogic(CloudApplicationPermission entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .Continue(_ =>
                {
                    entity.Name = "-";
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    return _cloudApplicationPermissionService.AddIfNotExist(entity);
                });

            if (!result.IsSuccess)
            {
                ViewData["UserAccounts"] = _userAccountRepository.GetDeveloperUserAccount();
                return View("Add", result.ToResponseModel(entity));
            }

            return RedirectToAction("List");
        }

        public IActionResult Delete(Guid id)
        {
            _cloudApplicationPermissionService.Delete(id);
            return JsonResultSuccess("删除成功");
        }
    }
}