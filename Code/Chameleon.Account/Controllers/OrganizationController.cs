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

        public IActionResult Index()
        {
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

        public IActionResult DelNode(Guid nodeId)
        {
            _organizationService.DeleteNode(nodeId);

            return JsonResultSuccess("删除成功");
        }
    }
}