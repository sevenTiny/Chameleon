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

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class InterfaceConditionController : WebControllerBase
    {
        readonly IInterfaceConditionService _InterfaceConditionService;
        IMetaFieldService _metaFieldService;
        IInterfaceConditionRepository _InterfaceConditionRepository;
        IInterfaceSettingApp _interfaceSettingApp;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceConditionController(IMetaFieldRepository metaFieldRepository, IInterfaceSettingApp interfaceSettingApp, IInterfaceConditionRepository InterfaceConditionRepository, IInterfaceConditionService InterfaceConditionService, IMetaFieldService metaFieldService)
        {
            _metaFieldRepository = metaFieldRepository;
            _interfaceSettingApp = interfaceSettingApp;
            _InterfaceConditionRepository = InterfaceConditionRepository;
            _metaFieldService = metaFieldService;
            _InterfaceConditionService = InterfaceConditionService;
        }

        public IActionResult List(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_InterfaceConditionRepository.GetTopInterfaceCondition(metaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(InterfaceCondition entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.CloudApplicationtId = CurrentApplicationId;
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.CreateBy = CurrentUserId;
                    entity.Code = string.Concat(CurrentMetaObjectCode, ".", entity.Code);

                    return _InterfaceConditionService.AddTopInterfaceCondition(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(data: entity));

            return Redirect($"/InterfaceCondition/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult Update(Guid id)
        {
            return View(ResponseModel.Success(data: _InterfaceConditionService.GetById(id)));
        }

        public IActionResult UpdateLogic(InterfaceCondition entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _InterfaceConditionService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return Redirect($"/InterfaceCondition/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult Setting(Guid belongToId)
        {
            var selectedFields = _InterfaceConditionRepository.GetInterfaceConditionByBelongToId(belongToId);

            ViewData["MetaFields"] = _metaFieldRepository.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId);

            ViewData["Id"] = belongToId;

            return View(selectedFields);
        }

        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <param name="id">条件的id</param>
        /// <returns></returns>
        public IActionResult GetTree(Guid id)
        {
            return Result<List<InterfaceCondition>>.Success(data: _InterfaceConditionService.GetTree(id)).ToJsonResult();
        }

        public IActionResult AddNode(Guid id, Guid brotherNodeId, int conditionJointTypeId, Guid fieldId, int conditionTypeId, int conditionValueTypeId, string conditionValue)
        {
            ViewData["Id"] = id;
            return Result.Success("添加成功")
                .ContinueEnsureArgumentNotNullOrEmpty(id, nameof(id))
                .ContinueEnsureArgumentNotNullOrEmpty(fieldId, nameof(fieldId))
                .ContinueEnsureArgumentNotNullOrEmpty(conditionJointTypeId, nameof(conditionJointTypeId))
                .ContinueEnsureArgumentNotNullOrEmpty(conditionTypeId, nameof(conditionTypeId))
                .ContinueEnsureArgumentNotNullOrEmpty(conditionValueTypeId, nameof(conditionValueTypeId))
                .Continue(_ => _interfaceSettingApp.InterfaceConditionAddNode(brotherNodeId, new InterfaceCondition
                {
                    Id = Guid.NewGuid(),
                    BelongToCondition = id,
                    ConditionJointType = conditionJointTypeId,
                    MetaFieldId = fieldId,
                    ConditionType = conditionTypeId,
                    ConditionValueType = conditionValueTypeId,
                    ConditionValue = conditionValue,
                    CloudApplicationtId = CurrentApplicationId,
                    MetaObjectId = CurrentMetaObjectId
                }))
                .ToResult(id)
                .ToJsonResult();
        }

        public IActionResult DelNode(Guid conditionId, Guid nodeId)
        {
            return Result.Success("删除成功")
                .ContinueEnsureArgumentNotNullOrEmpty(conditionId, nameof(conditionId))
                .ContinueEnsureArgumentNotNullOrEmpty(nodeId, nameof(nodeId))
                .Continue(_ => _InterfaceConditionService.DeleteNode(conditionId, nodeId))
                .ToResult(conditionId)
                .ToJsonResult();
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _InterfaceConditionService.LogicDelete(id).ToJsonResult();
        }
    }
}