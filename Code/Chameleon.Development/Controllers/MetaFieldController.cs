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
    public class MetaFieldController : WebControllerBase
    {
        readonly IMetaFieldService _metaFieldService;
        IMetaFieldRepository _metaFieldRepository;

        public MetaFieldController(IMetaFieldRepository metaFieldRepository, IMetaFieldService metaFieldService)
        {
            _metaFieldRepository = metaFieldRepository;
            _metaFieldService = metaFieldService;
        }

        public IActionResult List(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);
            return View(_metaFieldRepository.GetListUnDeletedByMetaObjectId(metaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(MetaField entity)
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
                    entity.ShortCode = entity.Code;
                    entity.Code = string.Concat(CurrentMetaObjectCode, ".", entity.ShortCode);

                    return _metaFieldService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(data: entity));

            return Redirect($"/MetaField/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult Update(Guid id)
        {
            var metaObject = _metaFieldService.GetById(id);
            return View(ResponseModel.Success(data: metaObject));
        }

        public IActionResult UpdateLogic(MetaField entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _metaFieldService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return Redirect($"/MetaField/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _metaFieldService.LogicDelete(id).ToJsonResult();
        }

        public IActionResult Delete(Guid id)
        {
            return _metaFieldService.Delete(id).ToJsonResult();
        }
    }
}