﻿using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class TriggerScriptController : WebControllerBase
    {
        ITriggerScriptService _triggerScriptService;
        ITriggerScriptRepository _triggerScriptRepository;

        public TriggerScriptController(ITriggerScriptService metaObjectService, ITriggerScriptRepository metaObjectRepository)
        {
            _triggerScriptService = metaObjectService;
            _triggerScriptRepository = metaObjectRepository;
        }

        public IActionResult MetaObjectInterfaceList(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_triggerScriptRepository.GetMetaObjectTriggerListByMetaObjectId(metaObjectId));
        }

        public IActionResult MetaObjectTriggerAdd()
        {
            TriggerScript metaObject = new TriggerScript();
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult MetaObjectTriggerAddLogic(TriggerScript entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.MetaObjectInterfaceServiceType, nameof(entity.MetaObjectInterfaceServiceType))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.ClassFullName, nameof(entity.ClassFullName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.FunctionName, nameof(entity.FunctionName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Language, nameof(entity.Language))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                //预编译脚本
                .Continue(_ => _triggerScriptService.CheckScript(entity))
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.Name = entity.GetMetaObjectInterfaceServiceType().GetDescription();
                    entity.Code = string.Concat(CurrentMetaObjectCode, ".", entity.GetMetaObjectInterfaceServiceType().ToString());
                    entity.CreateBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.ScriptType = (int)ScriptTypeEnum.MetaObjectInterfaceTrigger;
                    return _triggerScriptService.MetaObjectTriggerAdd(entity);
                });

            if (!result.IsSuccess)
                return View("MetaObjectTriggerAdd", result.ToResponseModel(entity));

            return Redirect($"/TriggerScript/MetaObjectInterfaceList?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult MetaObjectTriggerUpdate(Guid id)
        {
            var metaObject = _triggerScriptService.GetById(id);
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult MetaObjectTriggerUpdateLogic(TriggerScript entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
               //预编译脚本
               .Continue(_ => _triggerScriptService.CheckScript(entity))
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _triggerScriptService.UpdateWithOutCode(entity, t =>
                   {
                       t.Script = entity.Script;
                   });
               });

            if (!result.IsSuccess)
                return View("MetaObjectTriggerUpdate", result.ToResponseModel(entity));

            return Redirect($"/TriggerScript/MetaObjectInterfaceList?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult GetDefaultMetaObjectTriggerScript(MetaObjectInterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum)
        {
            return Result<DefaultScriptBase>.Success(data: _triggerScriptService.GetDefaultMetaObjectTriggerScript(metaObjectInterfaceServiceTypeEnum)).ToJsonResult();
        }

        public IActionResult LogicDelete(Guid id)
        {
            _triggerScriptService.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }
    }
}