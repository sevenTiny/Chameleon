﻿using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;

namespace Chameleon.Development.Controllers
{
    public class MetaObjectController : WebControllerBase
    {
        IMetaObjectService _metaObjectService;
        IMetaObjectRepository _metaObjectRepository;
        IMetaObjectApp _metaObjectApp;

        public MetaObjectController(IMetaObjectApp metaObjectApp, IMetaObjectService metaObjectService, IMetaObjectRepository metaObjectRepository)
        {
            _metaObjectApp = metaObjectApp;
            _metaObjectService = metaObjectService;
            _metaObjectRepository = metaObjectRepository;
        }

        public IActionResult List()
        {
            return View(_metaObjectRepository.GetMetaObjectListUnDeletedByApplicationId(CurrentApplicationId));
        }

        public IActionResult Add()
        {
            MetaObject metaObject = new MetaObject();
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult AddLogic(MetaObject entity)
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
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.Code = string.Concat(CurrentApplicationCode, ".", entity.Code);
                    return _metaObjectApp.AddMetaObject(entity.Id, entity.Code, CurrentApplicationId, entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var metaObject = _metaObjectService.GetById(id);
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult UpdateLogic(MetaObject entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _metaObjectService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _metaObjectService.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }
    }
}