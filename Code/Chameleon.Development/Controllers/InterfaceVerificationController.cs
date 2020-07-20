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
    public class InterfaceVerificationController : WebControllerBase
    {
        IInterfaceVerificationService _InterfaceVerificationService;
        IMetaFieldService _metaFieldService;
        IInterfaceVerificationRepository _InterfaceVerificationRepository;
        IInterfaceSettingApp _interfaceSettingApp;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceVerificationController(IMetaFieldRepository metaFieldRepository, IInterfaceSettingApp interfaceSettingApp, IInterfaceVerificationRepository InterfaceVerificationRepository, IInterfaceVerificationService InterfaceVerificationService, IMetaFieldService metaFieldService)
        {
            _metaFieldRepository = metaFieldRepository;
            _interfaceSettingApp = interfaceSettingApp;
            _InterfaceVerificationRepository = InterfaceVerificationRepository;
            _metaFieldService = metaFieldService;
            _InterfaceVerificationService = InterfaceVerificationService;
        }

        public IActionResult List(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_InterfaceVerificationRepository.GetTopInterfaceVerification(metaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(InterfaceVerification entity)
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
                    entity.MetaFieldShortCode = "-";

                    return _InterfaceVerificationService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(data: entity));

            return Redirect($"/InterfaceVerification/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult Update(Guid id)
        {
            return View(ResponseModel.Success(data: _InterfaceVerificationService.GetById(id)));
        }

        public IActionResult UpdateLogic(InterfaceVerification entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _InterfaceVerificationService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return Redirect($"/InterfaceVerification/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _InterfaceVerificationService.LogicDelete(id).ToJsonResult();
        }

        public IActionResult Delete(Guid id)
        {
            return _InterfaceVerificationService.Delete(id).ToJsonResult();
        }

        public IActionResult VerificationItemAdd(Guid parentId)
        {
            ViewData["MetaFields"] = _metaFieldRepository.GetListByMetaObjectId(CurrentMetaObjectId);
            return View(Result.Success().ToResponseModel(new InterfaceVerification { ParentId = parentId }));
        }

        public IActionResult VerificationItemAddLogic(Guid parentId, InterfaceVerification entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(parentId, nameof(parentId))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.MetaFieldId, nameof(entity.MetaFieldId))
                .Continue(_ =>
                {
                    entity.ParentId = parentId;
                    entity.CloudApplicationtId = CurrentApplicationId;
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.CreateBy = CurrentUserId;
                    entity.Name = "-";
                    entity.Code = Guid.NewGuid().ToString();

                    return _InterfaceVerificationService.AddVerificationItem(entity);
                });

            if (!result.IsSuccess)
            {
                ViewData["MetaFields"] = _metaFieldRepository.GetListByMetaObjectId(CurrentMetaObjectId);
                return View($"VerificationItemAdd", result.ToResponseModel(data: entity));
            }

            return Redirect($"/InterfaceVerification/VerificationItemList?parentId={entity.ParentId}");
        }

        public IActionResult VerificationItemUpdate(Guid id)
        {
            return View(ResponseModel.Success(data: _InterfaceVerificationService.GetById(id)));
        }

        public IActionResult VerificationItemUpdateLogic(Guid parentId, InterfaceVerification entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.RegularExpression, nameof(entity.RegularExpression))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.VerificationTips, nameof(entity.VerificationTips))
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;

                   return _InterfaceVerificationService.UpdateVerificationItem(entity);
               });

            if (!result.IsSuccess)
                return View("VerificationItemUpdate", result.ToResponseModel(entity));

            return Redirect($"/InterfaceVerification/VerificationItemList?parentId={parentId}");
        }

        public IActionResult VerificationItemList(Guid parentId)
        {
            var selectedFields = _InterfaceVerificationRepository.GetInterfaceVerificationByParentId(parentId);

            ViewData["Id"] = parentId;

            return View(selectedFields);
        }

        /// <summary>
        /// 常用正则表达式
        /// </summary>
        /// <returns></returns>
        public IActionResult GeneralRegularExpresion()
        {
            return View();
        }
    }
}