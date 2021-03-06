﻿using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class CloudApplicationController : WebControllerBase
    {
        ICloudApplicationService _applicationService;
        IMetaObjectService _metaObjectService;
        IMetaObjectRepository _metaObjectRepository;

        public CloudApplicationController(IMetaObjectRepository metaObjectRepository, ICloudApplicationService applicationService, IMetaObjectService metaObjectService)
        {
            _metaObjectRepository = metaObjectRepository;
            _applicationService = applicationService;
            _metaObjectService = metaObjectService;
        }

        public IActionResult Select()
        {
            var list = _applicationService.GetListUnDeleted();
            return View(list);
        }

        public IActionResult List()
        {
            var list = _applicationService.GetListUnDeleted();
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
                    return _applicationService.Add(entity);
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

        public IActionResult Detail(Guid applicationId, string applicationCode)
        {
            if (string.IsNullOrEmpty(applicationCode))
                return Redirect("/CloudApplication/Select");

            //设置cookie
            SetCookiesApplictionInfo(applicationId, applicationCode);

            ViewData["ApplicationCode"] = applicationCode;
            ViewData["ApplicationId"] = applicationId;
            ViewData["MetaObjects"] = _metaObjectRepository.GetMetaObjectListUnDeletedByApplicationId(applicationId);

            return View();
        }
    }
}