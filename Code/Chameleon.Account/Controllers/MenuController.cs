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
    public class MenuController : WebControllerBase
    {
        IMenuService _MenuService;
        IMenuRepository _MenuRepository;
        public MenuController(IMenuRepository MenuRepository, IMenuService MenuService)
        {
            _MenuRepository = MenuRepository;
            _MenuService = MenuService;
        }

        public IActionResult Setting()
        {
            ViewData["EnableList"] = _MenuService.GetTreeNameList();
            return View();
        }

        public IActionResult GetTree()
        {
            return Result<List<Menu>>.Success(data: _MenuService.GetTree()).ToJsonResult();
        }

        public IActionResult AddNode(int relation, Guid chooseId, Menu entity)
        {
            return Result.Success("添加成功")
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .Continue(_ =>
                {
                    entity.CreateBy = CurrentUserId;
                    return _MenuService.AddNode((RelationEnum)relation, chooseId, entity);
                })
                .ToJsonResult();
        }

        public IActionResult UpdateNode(Guid chooseId, Menu entity)
        {
            return Result.Success("修改成功")
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .Continue(_ =>
                {
                    entity.ModifyBy = CurrentUserId;
                    return _MenuService.UpdateWithId(chooseId, t =>
                    {
                        t.Name = entity.Name;
                        //t.Icon = entity.Icon;
                        t.Route = entity.Route;
                    });
                })
                .ToJsonResult();
        }

        public IActionResult AjustRelation(int relation, Guid chooseId, Guid ajustId)
        {
            return _MenuService.AjustRelation((RelationEnum)relation, chooseId, ajustId).ToJsonResult();
        }

        public IActionResult GetById(Guid id)
        {
            var menu = _MenuRepository.GetById(id);

            return ResponseModel.Success(data: menu).ToJsonResult();
        }

        public IActionResult LogicDelete(Guid id)
        {
            _MenuRepository.LogicDelete(id);

            return Result.Success("删除成功").ToJsonResult();
        }
    }
}