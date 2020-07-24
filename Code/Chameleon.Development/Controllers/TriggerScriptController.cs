using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;

namespace Chameleon.Development.Controllers
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

        #region MetaObjectTrigger
        public IActionResult MetaObjectTriggerList(Guid metaObjectId, string metaObjectCode)
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
                .ContinueEnsureArgumentNotNullOrEmpty(entity.InterfaceServiceType, nameof(entity.InterfaceServiceType))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.ClassFullName, nameof(entity.ClassFullName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.FunctionName, nameof(entity.FunctionName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Language, nameof(entity.Language))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                //预编译脚本
                .Continue(_ => _triggerScriptService.CheckScript(entity))
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.Name = entity.GetInterfaceServiceType().GetDescription();
                    entity.Code = string.Concat(CurrentMetaObjectCode, ".", entity.GetInterfaceServiceType().ToString());
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.ScriptType = (int)ScriptTypeEnumHelper.GetScriptTypeByInterfaceServiceTypeMapping(entity.GetInterfaceServiceType());
                    return _triggerScriptService.MetaObjectTriggerAdd(entity);
                });

            if (!result.IsSuccess)
                return View("MetaObjectTriggerAdd", result.ToResponseModel(entity));

            return Redirect($"/TriggerScript/MetaObjectTriggerList?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
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

            return Redirect($"/TriggerScript/MetaObjectTriggerList?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }
        #endregion

        #region CloudApplicationTrigger
        public IActionResult CloudApplicationTriggerList()
        {
            return View(_triggerScriptRepository.GetCloudApplicationTriggerListByCloudApplicationId(CurrentApplicationId));
        }
        public IActionResult CloudApplicationTriggerAdd()
        {
            TriggerScript metaObject = new TriggerScript();
            return View(ResponseModel.Success(data: metaObject));
        }
        public IActionResult CloudApplicationTriggerAddLogic(TriggerScript entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.InterfaceServiceType, nameof(entity.InterfaceServiceType))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.ClassFullName, nameof(entity.ClassFullName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.FunctionName, nameof(entity.FunctionName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Language, nameof(entity.Language))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                //预编译脚本
                .Continue(_ => _triggerScriptService.CheckScript(entity))
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.Name = entity.GetInterfaceServiceType().GetDescription();
                    entity.Code = string.Concat(CurrentApplicationCode, ".", entity.GetInterfaceServiceType().ToString());
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.ScriptType = (int)ScriptTypeEnumHelper.GetScriptTypeByInterfaceServiceTypeMapping(entity.GetInterfaceServiceType());
                    return _triggerScriptService.MetaObjectTriggerAdd(entity);
                });

            if (!result.IsSuccess)
                return View("CloudApplicationTriggerAdd", result.ToResponseModel(entity));

            return Redirect($"/TriggerScript/CloudApplicationTriggerList");
        }
        public IActionResult CloudApplicationTriggerUpdate(Guid id)
        {
            var metaObject = _triggerScriptService.GetById(id);
            return View(ResponseModel.Success(data: metaObject));
        }
        public IActionResult CloudApplicationTriggerUpdateLogic(TriggerScript entity)
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
                return View("CloudApplicationTriggerUpdate", result.ToResponseModel(entity));

            return Redirect($"/TriggerScript/CloudApplicationTriggerList");
        }
        #endregion

        public IActionResult GetDefaultTriggerScriptTemplate(InterfaceServiceTypeEnum interfaceServiceType)
        {
            return Result<DefaultScriptBase>.Success(data: _triggerScriptService.GetDefaultMetaObjectTriggerScript(interfaceServiceType)).ToJsonResult();
        }
        public IActionResult LogicDelete(Guid id)
        {
            _triggerScriptService.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }
        public IActionResult ScriptTemplate()
        {
            return View();
        }
    }
}