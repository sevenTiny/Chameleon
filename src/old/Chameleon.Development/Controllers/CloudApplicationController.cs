﻿using Chameleon.Application;
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
    public class CloudApplicationController : WebControllerBase
    {
        ICloudApplicationService _applicationService;
        IMetaObjectService _metaObjectService;
        IMetaObjectRepository _metaObjectRepository;
        ICloudApplicationApp _cloudApplicationApp;
        public CloudApplicationController(ICloudApplicationApp cloudApplicationApp, IMetaObjectRepository metaObjectRepository, ICloudApplicationService applicationService, IMetaObjectService metaObjectService)
        {
            _cloudApplicationApp = cloudApplicationApp;
            _metaObjectRepository = metaObjectRepository;
            _applicationService = applicationService;
            _metaObjectService = metaObjectService;
        }

        public IActionResult Select()
        {
            var list = _cloudApplicationApp.GetUserPermissionApplications(CurrentUserId)?.OrderBy(t => t.SortNumber).ThenByDescending(t => t.CreateTime).ToList();
            return View(list);
        }

        public IActionResult List()
        {
            var list = _cloudApplicationApp.GetUserPermissionApplications(CurrentUserId)?.OrderBy(t => t.SortNumber).ThenByDescending(t => t.CreateTime).ToList();
            return View(list);
        }

        public IActionResult Add()
        {
            var application = new CloudApplication
            {
                Icon = "cloud"
            };
            application.Icon = "cloud";
            return View(ResponseModel.Success(data: application));
        }

        public IActionResult AddLogic(CloudApplication entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    return _cloudApplicationApp.AddCloudApplication(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var application = _applicationService.GetById(id);
            return View(ResponseModel.Success(data: application));
        }

        public IActionResult UpdateLogic(CloudApplication entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _applicationService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _applicationService.LogicDelete(id);
            return JsonResultSuccess("删除成功");
        }

        public IActionResult Detail(Guid applicationId)
        {
            //获取用户信息到页面
            SetUserInfoToViewData();

            if (applicationId == Guid.Empty)
                return Redirect("/Home/Index");

            var application = _applicationService.GetById(applicationId);

            if (application == null)
                return Content("应用不存在！");

            //设置cookie
            SetCookiesApplictionInfo(applicationId, application.Code);

            ViewData["ApplicationCode"] = application.Code;
            ViewData["ApplicationId"] = applicationId;
            ViewData["MetaObjects"] = (_metaObjectRepository.GetMetaObjectListUnDeletedByApplicationId(applicationId) ?? new List<MetaObject>(0)).OrderBy(t => t.SortNumber).ThenByDescending(t => t.CreateTime).ToList();

            return View();
        }
    }
}