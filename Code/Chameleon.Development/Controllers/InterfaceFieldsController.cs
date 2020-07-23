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
    public class InterfaceFieldsController : WebControllerBase
    {
        readonly IInterfaceFieldsService _interfaceFieldsService;
        IMetaFieldService _metaFieldService;
        IInterfaceFieldsRepository _interfaceFieldsRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceFieldsController(IMetaFieldRepository metaFieldRepository, IInterfaceFieldsRepository interfaceFieldsRepository, IInterfaceFieldsService InterfaceFieldsService, IMetaFieldService metaFieldService)
        {
            _metaFieldRepository = metaFieldRepository;
            _interfaceFieldsRepository = interfaceFieldsRepository;
            _metaFieldService = metaFieldService;
            _interfaceFieldsService = InterfaceFieldsService;
        }

        public IActionResult List(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_interfaceFieldsRepository.GetTopInterfaceFields(metaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(InterfaceFields entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.CloudApplicationId = CurrentApplicationId;
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.Code = string.Concat(CurrentMetaObjectCode, ".", entity.Code);

                    return _interfaceFieldsService.AddTopInterfaceFields(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(data: entity));

            return Redirect($"/InterfaceFields/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult Update(Guid id)
        {
            return View(ResponseModel.Success(data: _interfaceFieldsService.GetById(id)));
        }

        public IActionResult UpdateLogic(InterfaceFields entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _interfaceFieldsService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return Redirect($"/InterfaceFields/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult SeletedMetaFieldEdit(Guid id)
        {
            return View(ResponseModel.Success(data: _interfaceFieldsService.GetById(id)));
        }

        public IActionResult SeletedMetaFieldEditLogic(InterfaceFields entity)
        {
            var result = _interfaceFieldsService.UpdateSeletedMetaField(entity);

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

            var re = _interfaceFieldsService.SaveSetting(CurrentMetaObjectId, id, metaFieldsToAdd);

            if (re.IsSuccess)
                re.Message = "操作成功";

            return re.ToJsonResult();
        }

        public IActionResult Setting(Guid parentMetaFieldsId)
        {
            var selectedFields = _interfaceFieldsRepository.GetInterfaceFieldsByParentId(parentMetaFieldsId);

            var selectedMetaFieldIds = selectedFields?.Select(t => t.MetaFieldId).ToArray() ?? new Guid[0];

            ViewData["MetaFields"] = _metaFieldRepository.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId)?.Where(t => !selectedMetaFieldIds.Contains(t.Id)).ToList();

            ViewData["Id"] = parentMetaFieldsId;

            return View(selectedFields);
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _interfaceFieldsService.LogicDelete(id).ToJsonResult();
        }
    }
}