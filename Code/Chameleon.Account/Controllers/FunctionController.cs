using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chameleon.Domain;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Extensions.AspNetCore;
using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;

namespace Chameleon.Account.Controllers
{
    public class FunctionController : WebControllerBase
    {
        IFunctionService _FunctionService;
        IFunctionRepository _FunctionRepository;
        public FunctionController(IFunctionRepository FunctionRepository, IFunctionService FunctionService)
        {
            _FunctionRepository = FunctionRepository;
            _FunctionService = FunctionService;
        }

        public IActionResult List()
        {
            return View(_FunctionRepository.GetListUnDeleted());
        }

        public IActionResult Add()
        {
            Function Function = new Function();
            return View(ResponseModel.Success(data: Function));
        }

        public IActionResult AddLogic(Function entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreateBy = CurrentUserId;
                    entity.ModifyBy = CurrentUserId;
                    entity.Code = entity.Code;
                    return _FunctionService.AddCheckCode(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var Function = _FunctionService.GetById(id);
            return View(ResponseModel.Success(data: Function));
        }

        public IActionResult UpdateLogic(Function entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _FunctionService.UpdateWithOutCode(entity, item =>
                   {
                       item.Code = entity.Code;
                   });
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _FunctionService.LogicDelete(id);

            return JsonResultSuccess("删除成功");
        }
    }
}