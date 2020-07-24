using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure.Configs;
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
    public class InterfaceSettingController : WebControllerBase
    {
        IInterfaceSettingService _InterfaceSettingService;
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
        public InterfaceSettingController(IInterfaceSortRepository interfaceSortRepository, IInterfaceConditionService interfaceConditionService, IInterfaceVerificationService interfaceVerificationService, IInterfaceFieldsService interfaceFieldsService, IInterfaceFieldsRepository interfaceFieldsRepository, IInterfaceVerificationRepository interfaceVerificationRepository, IInterfaceConditionRepository interfaceConditionRepository, IInterfaceSettingApp interfaceSettingApp, IInterfaceSettingRepository InterfaceSettingRepository, IInterfaceSettingService InterfaceSettingService, IMetaFieldService metaFieldService)
        {
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
            _InterfaceSettingService = InterfaceSettingService;
        }

        public IActionResult MetaObjectInterfaceList(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_InterfaceSettingService.GetInterfaceSettingsTranslated(metaObjectId));
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

                    return _InterfaceSettingService.AddCheckCode(entity);
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

            return View(ResponseModel.Success(data: _InterfaceSettingService.GetById(id)));
        }

        public IActionResult MetaObjectInterfaceUpdateLogic(InterfaceSetting entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _InterfaceSettingService.UpdateWithOutCode(entity, item =>
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
            return _InterfaceSettingService.LogicDelete(id).ToJsonResult();
        }
    }
}