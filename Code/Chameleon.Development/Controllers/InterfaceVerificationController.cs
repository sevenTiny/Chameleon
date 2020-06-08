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
    public class InterfaceVerificationController : WebControllerBase
    {
        IInterfaceVerificationService _InterfaceVerificationService;
        IMetaFieldService _metaFieldService;
        IInterfaceVerificationRepository _InterfaceVerificationRepository;
        IInterfaceSettingApp _interfaceSettingApp;
        public InterfaceVerificationController(IInterfaceSettingApp interfaceSettingApp, IInterfaceVerificationRepository InterfaceVerificationRepository, IInterfaceVerificationService InterfaceVerificationService, IMetaFieldService metaFieldService)
        {
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

                    return _InterfaceVerificationService.Add(entity);
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

        public IActionResult SeletedMetaFieldEdit(Guid id)
        {
            return View(ResponseModel.Success(data: _InterfaceVerificationService.GetById(id)));
        }

        public IActionResult SeletedMetaFieldEditLogic(InterfaceVerification entity)
        {
            var result = _InterfaceVerificationService.UpdateSeletedMetaField(entity);

            if (!result.IsSuccess)
                return View("SeletedMetaFieldEdit", result.ToResponseModel(entity));

            return Content("修改成功");
        }

        public IActionResult SaveSetting(Guid id, string metaFieldIds)
        {
            if (string.IsNullOrEmpty(metaFieldIds))
                return Result.Success().ToJsonResult();

            List<Guid> metaFieldsToAdd = new List<Guid>();

            foreach (var item in metaFieldIds.Split(','))
            {
                if (Guid.TryParse(item, out Guid result))
                    metaFieldsToAdd.Add(result);
            }

            var re = _InterfaceVerificationService.SaveSetting(CurrentMetaObjectId, id, metaFieldsToAdd);

            if (re.IsSuccess)
                re.Message = "操作成功";

            return re.ToJsonResult();
        }

        public IActionResult Setting(Guid parentId)
        {
            var selectedFields = _InterfaceVerificationRepository.GetInterfaceVerificationByParentId(parentId);

            var selectedMetaFieldIds = selectedFields?.Select(t => t.MetaFieldId).ToArray() ?? new Guid[0];

            ViewData["MetaFields"] = _metaFieldService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId)?.Where(t => !selectedMetaFieldIds.Contains(t.Id)).ToList();

            ViewData["Id"] = parentId;

            return View(selectedFields);
        }
    }
}