using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class DataSourceController : WebControllerBase
    {
        ITriggerScriptService _triggerScriptService;
        ITriggerScriptRepository _triggerScriptRepository;
        IInterfaceSettingService _interfaceSettingService;
        IInterfaceSettingRepository _interfaceSettingRepository;
        public DataSourceController(IInterfaceSettingRepository interfaceSettingRepository, IInterfaceSettingService interfaceSettingService, ITriggerScriptService metaObjectService, ITriggerScriptRepository metaObjectRepository)
        {
            _interfaceSettingRepository = interfaceSettingRepository;
            _interfaceSettingService = interfaceSettingService;
            _triggerScriptService = metaObjectService;
            _triggerScriptRepository = metaObjectRepository;
        }

        public IActionResult DynamicScriptTriggerList()
        {
            return View(_triggerScriptRepository.GetDataSourceListByApplicationId(CurrentApplicationId, ScriptTypeEnum.DynamicScriptDataSourceTrigger));
        }

        public IActionResult DynamicScriptTriggerAdd()
        {
            //获取默认脚本
            var defaultScript = _triggerScriptService.GetDeefaultDynamicScriptDataSourceTrigger();
            TriggerScript metaObject = new TriggerScript()
            {
                Script = defaultScript.Script,
                ClassFullName = defaultScript.ClassFullName,
                FunctionName = defaultScript.FunctionName
            };
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult DynamicScriptTriggerAddLogic(TriggerScript entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.ClassFullName, nameof(entity.ClassFullName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.FunctionName, nameof(entity.FunctionName))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Language, nameof(entity.Language))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                //预编译脚本
                .Continue(_ => _triggerScriptService.CheckScript(entity))
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreateBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.Code = string.Concat(CurrentApplicationCode, ".", entity.Code);
                    entity.ScriptType = (int)ScriptTypeEnum.DynamicScriptDataSourceTrigger;
                    return _triggerScriptService.Add(entity);
                })
                //同步添加接口
                .Continue(_ =>
                {
                    _interfaceSettingService.Add(new InterfaceSetting
                    {
                        DataSousrceId = entity.Id,
                        InterfaceType = (int)InterfaceTypeEnum.DynamicScriptDataSource,
                        Code = entity.Code,
                        Name = entity.Name,
                        CloudApplicationtId = entity.CloudApplicationId,
                        CloudApplicationCode = CurrentApplicationCode,
                        MetaObjectId = Guid.Empty,
                        MetaObjectCode = "-"
                    });
                    return _;
                });

            if (!result.IsSuccess)
                return View("DynamicScriptTriggerAdd", result.ToResponseModel(entity));

            return RedirectToAction("DynamicScriptTriggerList");
        }

        public IActionResult DynamicScriptTriggerUpdate(Guid id)
        {
            var metaObject = _triggerScriptService.GetById(id);
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult DynamicScriptTriggerUpdateLogic(TriggerScript entity)
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
                return View("DynamicScriptTriggerUpdate", result.ToResponseModel(entity));

            return RedirectToAction("DynamicScriptTriggerList");
        }

        public IActionResult JsonDataSourceList()
        {
            return View(_triggerScriptRepository.GetDataSourceListByApplicationId(CurrentApplicationId, ScriptTypeEnum.JsonDataSource));
        }

        public IActionResult JsonDataSourceAdd()
        {
            //获取默认脚本
            TriggerScript metaObject = new TriggerScript()
            {
                Script =
@"{
    ""Key"":""item""
}"
            };
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult JsonDataSourceAddLogic(TriggerScript entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
               //校验json格式
               .Continue(_ =>
               {
                   try
                   {
                       var a = JObject.Parse(entity.Script);
                   }
                   catch (Exception ex)
                   {
                       return Result.Error($"JSON格式解析错误，ex:{ex.Message}");
                   }
                   return _;
               })
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreateBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.Code = string.Concat(CurrentApplicationCode, ".", entity.Code);
                    entity.ScriptType = (int)ScriptTypeEnum.JsonDataSource;
                    entity.ClassFullName = "-";
                    entity.FunctionName = "-";
                    return _triggerScriptService.Add(entity);
                })
                //同步添加接口
                .Continue(_ =>
                {
                    _interfaceSettingService.Add(new InterfaceSetting
                    {
                        DataSousrceId = entity.Id,
                        InterfaceType = (int)InterfaceTypeEnum.JsonDataSource,
                        Code = entity.Code,
                        Name = entity.Name,
                        CloudApplicationtId = entity.CloudApplicationId,
                        CloudApplicationCode = CurrentApplicationCode,
                        MetaObjectId = Guid.Empty,
                        MetaObjectCode = "-"
                    });
                    return _;
                });

            if (!result.IsSuccess)
                return View("JsonDataSourceAdd", result.ToResponseModel(entity));

            return RedirectToAction("JsonDataSourceList");
        }

        public IActionResult JsonDataSourceUpdate(Guid id)
        {
            var metaObject = _triggerScriptService.GetById(id);
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult JsonDataSourceUpdateLogic(TriggerScript entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
               //校验json格式
               .Continue(_ =>
               {
                   try
                   {
                       JObject.Parse(entity.Script);
                   }
                   catch
                   {
                       try
                       {
                           JArray.Parse(entity.Script);
                       }
                       catch (Exception ex)
                       {
                           return Result.Error($"JSON格式解析错误，ex:{ex.Message}");
                       }
                   }
                   return _;
               })
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _triggerScriptService.UpdateWithOutCode(entity, t =>
                   {
                       t.Script = entity.Script;
                   });
               });

            if (!result.IsSuccess)
                return View("JsonDataSourceUpdateLogic", result.ToResponseModel(entity));

            return RedirectToAction("JsonDataSourceList");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _triggerScriptService.LogicDelete(id);

            //同时删除接口
            _interfaceSettingRepository.LogicDeleteByDataSourceId(id);

            return JsonResultSuccess("删除成功");
        }
    }
}