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
    public class InterfaceSortController : WebControllerBase
    {
        readonly IInterfaceSortService _interfaceSortService;
        IInterfaceSortRepository _interfaceSortRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceSortController(IMetaFieldRepository metaFieldRepository, IInterfaceSortRepository interfaceSortRepository, IInterfaceSortService InterfaceSortService)
        {
            _metaFieldRepository = metaFieldRepository;
            _interfaceSortRepository = interfaceSortRepository;
            _interfaceSortService = InterfaceSortService;
        }

        public IActionResult List(Guid metaObjectId, string metaObjectCode)
        {
            SetCookiesMetaObjectInfo(metaObjectId, metaObjectCode);

            return View(_interfaceSortRepository.GetTopInterfaceSort(metaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(InterfaceSort entity)
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
                    entity.MetaFieldShortCode = Guid.NewGuid().ToString().Replace("-", string.Empty);

                    return _interfaceSortService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(data: entity));

            return Redirect($"/InterfaceSort/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult Update(Guid id)
        {
            return View(ResponseModel.Success(data: _interfaceSortService.GetById(id)));
        }

        public IActionResult UpdateLogic(InterfaceSort entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _interfaceSortService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return Redirect($"/InterfaceSort/List?metaObjectId={CurrentMetaObjectId}&metaObjectCode={CurrentMetaObjectCode}");
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _interfaceSortService.LogicDelete(id).ToJsonResult();
        }

        public IActionResult Delete(Guid id)
        {
            return _interfaceSortService.Delete(id).ToJsonResult();
        }

        public IActionResult SortItemAdd(Guid parentId)
        {
            ViewData["MetaFields"] = _metaFieldRepository.GetListByMetaObjectId(CurrentMetaObjectId);
            return View(Result.Success().ToResponseModel(new InterfaceSort { ParentId = parentId }));
        }

        public IActionResult SortItemAddLogic(Guid parentId, InterfaceSort entity)
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
                    entity.Code = Guid.NewGuid().ToString().Replace("-", string.Empty);

                    return _interfaceSortService.AddSortItem(entity);
                });

            if (!result.IsSuccess)
            {
                ViewData["MetaFields"] = _metaFieldRepository.GetListByMetaObjectId(CurrentMetaObjectId);
                return View($"SortItemAdd", result.ToResponseModel(data: entity));
            }

            return Redirect($"/InterfaceSort/SortItemList?parentId={entity.ParentId}");
        }

        public IActionResult SortItemUpdate(Guid id)
        {
            return View(ResponseModel.Success(data: _interfaceSortService.GetById(id)));
        }

        public IActionResult SortItemUpdateLogic(Guid parentId, InterfaceSort entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _interfaceSortService.UpdateSortItem(entity);
               });

            if (!result.IsSuccess)
                return View("SortItemUpdate", result.ToResponseModel(entity));

            return Redirect($"/InterfaceSort/SortItemList?parentId={parentId}");
        }

        public IActionResult SortItemList(Guid parentId)
        {
            var selectedFields = _interfaceSortRepository.GetInterfaceSortByParentId(parentId);

            ViewData["Id"] = parentId;

            return View(selectedFields);
        }
    }
}