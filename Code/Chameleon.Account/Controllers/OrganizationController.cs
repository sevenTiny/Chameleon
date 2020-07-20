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
    public class OrganizationController : WebControllerBase
    {
        IOrganizationService _organizationService;
        IOrganizationRepository _organizationRepository;
        public OrganizationController(IOrganizationRepository organizationRepository, IOrganizationService organizationService)
        {
            _organizationRepository = organizationRepository;
            _organizationService = organizationService;
        }

        public IActionResult Setting()
        {
            ViewData["EnableList"] = _organizationService.GetTreeNameList();
            return View();
        }

        public IActionResult GetTree()
        {
            return Result<List<Organization>>.Success(data: _organizationService.GetTree()).ToJsonResult();
        }

        public IActionResult AddNode(int relation, Guid chooseId, Organization organization)
        {
            return Result.Success("添加成功")
                .ContinueEnsureArgumentNotNullOrEmpty(organization, nameof(organization))
                .ContinueEnsureArgumentNotNullOrEmpty(organization.Name, nameof(organization.Name))
                .Continue(_ =>
                {
                    organization.CreateBy = CurrentUserId;
                    return _organizationService.AddNode((RelationEnum)relation, chooseId, organization);
                })
                .ToJsonResult();
        }

        public IActionResult UpdateNode(Guid chooseId, Organization organization)
        {
            return Result.Success("修改成功")
                .ContinueEnsureArgumentNotNullOrEmpty(organization, nameof(organization))
                .ContinueEnsureArgumentNotNullOrEmpty(organization.Name, nameof(organization.Name))
                .Continue(_ =>
                {
                    organization.ModifyBy = CurrentUserId;
                    return _organizationService.UpdateWithId(chooseId, t =>
                    {
                        t.Name = organization.Name;
                    });
                })
                .ToJsonResult();
        }

        public IActionResult AjustRelation(int relation, Guid chooseId, Guid ajustId)
        {
            return _organizationService.AjustRelation((RelationEnum)relation, chooseId, ajustId).ToJsonResult();
        }

        public IActionResult DisableNode(Guid nodeId)
        {
            _organizationService.DisableNode(nodeId);

            return JsonResultSuccess("禁用成功");
        }

        public IActionResult EnableNode(Guid nodeId)
        {
            _organizationService.EnableNode(nodeId);

            return JsonResultSuccess("启用成功");
        }

        public IActionResult DisableList()
        {
            return View(_organizationRepository.GetDisableList());
        }
    }
}