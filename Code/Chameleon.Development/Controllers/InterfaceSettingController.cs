using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure.Configs;
using Chameleon.Infrastructure.Consts;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Chameleon.Development.Controllers
{
    public class InterfaceSettingController : WebControllerBase
    {
        IInterfaceSettingService _interfaceSettingService;
        IMetaFieldService _metaFieldService;
        IInterfaceSettingRepository _InterfaceSettingRepository;
        IInterfaceSettingApp _interfaceSettingApp;
        IInterfaceConditionRepository _interfaceConditionRepository;
        IInterfaceVerificationRepository _interfaceVerificationRepository;
        IInterfaceFieldsRepository _interfaceFieldsRepository;
        IInterfaceFieldsService _interfaceFieldsService;
        IInterfaceVerificationService _interfaceVerificationService;
        IInterfaceConditionService _interfaceConditionService;
        IInterfaceSortRepository _interfaceSortRepository;
        ITriggerScriptRepository _triggerScriptRepository;
        ITriggerScriptService _triggerScriptService;
        public InterfaceSettingController(ITriggerScriptService triggerScriptService, ITriggerScriptRepository triggerScriptRepository, IInterfaceSortRepository interfaceSortRepository, IInterfaceConditionService interfaceConditionService, IInterfaceVerificationService interfaceVerificationService, IInterfaceFieldsService interfaceFieldsService, IInterfaceFieldsRepository interfaceFieldsRepository, IInterfaceVerificationRepository interfaceVerificationRepository, IInterfaceConditionRepository interfaceConditionRepository, IInterfaceSettingApp interfaceSettingApp, IInterfaceSettingRepository InterfaceSettingRepository, IInterfaceSettingService InterfaceSettingService, IMetaFieldService metaFieldService)
        {
            _triggerScriptService = triggerScriptService;
            _triggerScriptRepository = triggerScriptRepository;
            _interfaceSortRepository = interfaceSortRepository;
            _interfaceConditionService = interfaceConditionService;
            _interfaceVerificationService = interfaceVerificationService;
            _interfaceFieldsService = interfaceFieldsService;
            _interfaceFieldsRepository = interfaceFieldsRepository;
            _interfaceVerificationRepository = interfaceVerificationRepository;
            _interfaceConditionRepository = interfaceConditionRepository;
            _interfaceSettingApp = interfaceSettingApp;
            _InterfaceSettingRepository = InterfaceSettingRepository;
            _metaFieldService = metaFieldService;
            _interfaceSettingService = InterfaceSettingService;
        }

        #region MetaObjectInterface
        public IActionResult MetaObjectInterfaceList(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_interfaceSettingService.GetInterfaceSettingsTranslated(metaObjectId));
        }
        public IActionResult MetaObjectInterfaceAdd()
        {
            ViewData["InterfaceCondition"] = _interfaceConditionRepository.GetTopInterfaceCondition(CurrentMetaObjectId);
            ViewData["InterfaceVerification"] = _interfaceVerificationRepository.GetTopInterfaceVerification(CurrentMetaObjectId);
            ViewData["InterfaceFields"] = _interfaceFieldsRepository.GetTopInterfaceFields(CurrentMetaObjectId);
            ViewData["InterfaceSort"] = _interfaceSortRepository.GetTopInterfaceSort(CurrentMetaObjectId);

            return View(ResponseModel.Success(data: new InterfaceSetting { PageSize = ChameleonSettingConfig.Instance.DefaultInterfacePageSize }));
        }
        public IActionResult MetaObjectInterfaceAddLogic(InterfaceSetting entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    if (entity.GetInterfaceType() == InterfaceTypeEnum.QueryList)
                        return _.ContinueAssert(_t => entity.PageSize > 0, "分页页大小不能<=0");
                    return _;
                })
                .Continue(_ =>
                {
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.CloudApplicationCode = CurrentApplicationCode;
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.MetaObjectCode = CurrentMetaObjectCode;
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.Code = string.Concat(CurrentMetaObjectCode, ".", entity.Code);

                    return _interfaceSettingService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("MetaObjectInterfaceAdd", result.ToResponseModel(data: entity));

            return Redirect($"/InterfaceSetting/MetaObjectInterfaceList?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }
        public IActionResult MetaObjectInterfaceUpdate(Guid id)
        {
            ViewData["InterfaceCondition"] = _interfaceConditionRepository.GetTopInterfaceCondition(CurrentMetaObjectId);
            ViewData["InterfaceVerification"] = _interfaceVerificationRepository.GetTopInterfaceVerification(CurrentMetaObjectId);
            ViewData["InterfaceFields"] = _interfaceFieldsRepository.GetTopInterfaceFields(CurrentMetaObjectId);
            ViewData["InterfaceSort"] = _interfaceSortRepository.GetTopInterfaceSort(CurrentMetaObjectId);

            return View(ResponseModel.Success(data: _interfaceSettingService.GetById(id)));
        }
        public IActionResult MetaObjectInterfaceUpdateLogic(InterfaceSetting entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   if (entity.GetInterfaceType() == InterfaceTypeEnum.QueryList)
                       return _.ContinueAssert(_t => entity.PageSize > 0, "分页页大小不能<=0");
                   return _;
               })
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _interfaceSettingService.UpdateWithOutCode(entity, item =>
                   {
                       item.InterfaceType = entity.InterfaceType;
                       item.InterfaceConditionId = entity.InterfaceConditionId;
                       item.InterfaceVerificationId = entity.InterfaceVerificationId;
                       item.InterfaceFieldsId = entity.InterfaceFieldsId;
                       item.InterfaceSortId = entity.InterfaceSortId;
                       item.PageSize = entity.PageSize;
                   });
               });

            if (!result.IsSuccess)
                return View("MetaObjectInterfaceUpdate", result.ToResponseModel(entity));

            return Redirect($"/InterfaceSetting/MetaObjectInterfaceList?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }
        #endregion

        #region FileManagement
        public IActionResult FileManagementList()
        {
            return View(_InterfaceSettingRepository.GetFileManagementListByCloudApplicationId(CurrentApplicationId));
        }
        public IActionResult FileManagementAdd()
        {
            return View(ResponseModel.Success());
        }
        public IActionResult FileManagementAddLogic(InterfaceSetting entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    if (entity.FileSizeLimit > CommonConst.MaxFileUploadSizeLimit)
                        return Result.Error($"cannot set file upload size limit rather than {CommonConst.MaxFileUploadSizeLimit} byte");

                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.CloudApplicationCode = CurrentApplicationCode;
                    entity.MetaObjectCode = "-";
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.Code = string.Concat(CurrentApplicationCode, ".FDS.", entity.Code);

                    return _interfaceSettingService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("FileManagementAdd", result.ToResponseModel(data: entity));

            return Redirect($"/InterfaceSetting/FileManagementList");
        }
        public IActionResult FileManagementUpdate(Guid id)
        {
            return View(ResponseModel.Success(data: _interfaceSettingService.GetById(id)));
        }
        public IActionResult FileManagementUpdateLogic(InterfaceSetting entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   if (entity.FileSizeLimit > CommonConst.MaxFileUploadSizeLimit)
                       return Result.Error($"cannot set file upload size limit rather than {CommonConst.MaxFileUploadSizeLimit} byte");

                   entity.ModifyBy = CurrentUserId;
                   return _interfaceSettingService.UpdateWithOutCode(entity, item =>
                   {
                       item.FileExtensionLimit = entity.FileExtensionLimit;
                       item.FileSizeLimit = entity.FileSizeLimit;
                   });
               });

            if (!result.IsSuccess)
                return View("FileManagementUpdate", result.ToResponseModel(entity));

            return Redirect($"/InterfaceSetting/FileManagementList");
        }
        #endregion

        #region DynamicScript
        public IActionResult DynamicScriptTriggerList()
        {
            return View(_triggerScriptRepository.GetDataSourceListByApplicationId(CurrentApplicationId, ScriptTypeEnum.DynamicScriptDataSourceTrigger));
        }
        public IActionResult DynamicScriptTriggerAdd()
        {
            //获取默认脚本
            var defaultScript = _triggerScriptService.GetDefaultMetaObjectTriggerScript(InterfaceServiceTypeEnum.Application_DataSource);
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
                    entity.ModifyBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.Code = string.Concat(CurrentApplicationCode, ".TDS.", entity.Code);
                    entity.ScriptType = (int)ScriptTypeEnum.DynamicScriptDataSourceTrigger;
                    return _triggerScriptService.AddCheckCode(entity);
                })
                //同步添加接口
                .Continue(_ =>
                {
                    _interfaceSettingService.AddCheckCode(new InterfaceSetting
                    {
                        CreateBy = CurrentUserId,
                        ModifyBy = CurrentUserId,
                        DataSousrceId = entity.Id,
                        InterfaceType = (int)InterfaceTypeEnum.DynamicScriptDataSource,
                        Code = entity.Code,//这里【应用名.编码】的形式一定要保证应用下的接口编码唯一，因为这里添加脚本没法校验接口是否重复
                        Name = entity.Name,
                        CloudApplicationId = entity.CloudApplicationId,
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
        #endregion

        #region Json
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
                    entity.ModifyBy = CurrentUserId;
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.Code = string.Concat(CurrentApplicationCode, ".JDS.", entity.Code);
                    entity.ScriptType = (int)ScriptTypeEnum.JsonDataSource;
                    entity.ClassFullName = "-";
                    entity.FunctionName = "-";
                    return _triggerScriptService.AddCheckCode(entity);
                })
                //同步添加接口
                .Continue(_ =>
                {
                    _interfaceSettingService.AddCheckCode(new InterfaceSetting
                    {
                        CreateBy = CurrentUserId,
                        ModifyBy = CurrentUserId,
                        DataSousrceId = entity.Id,
                        InterfaceType = (int)InterfaceTypeEnum.JsonDataSource,
                        Code = entity.Code,
                        Name = entity.Name,
                        CloudApplicationId = entity.CloudApplicationId,
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
        #endregion


        /// <summary>
        /// 接口详情页
        /// </summary>
        /// <param name="interfaceCode"></param>
        /// <returns></returns>
        public IActionResult InterfaceDetail(string interfaceCode)
        {
            if (string.IsNullOrEmpty(interfaceCode))
                return View();

            var interfaceSetting = _InterfaceSettingRepository.GetByCode(interfaceCode);

            if (interfaceSetting.InterfaceConditionId != Guid.Empty)
            {
                ViewData["InterfaceCondition"] = _interfaceConditionRepository.GetInterfaceConditionArgumentNodeByBelongToId(interfaceSetting.InterfaceConditionId);
            }

            return View(interfaceSetting);
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _interfaceSettingService.LogicDelete(id).ToJsonResult();
        }
    }
}