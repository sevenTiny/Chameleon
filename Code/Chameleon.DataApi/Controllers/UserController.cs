using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerCommonBase
    {
        IOrganizationService _organizationService;
        IOrganizationRepository _organizationRepository;
        public UserController(IOrganizationRepository organizationRepository, IOrganizationService organizationService)
        {
            _organizationRepository = organizationRepository;
            _organizationService = organizationService;
        }

        /// <summary>
        /// 获取所有启用的组织
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("EnableOrgnizations")]
        public IActionResult Get()
        {
            return SafeExecute(() =>
            {
                var enableOrgs = _organizationRepository.GetEnableList() ?? new List<Organization>(0);

                return ResponseModel.Success(data: enableOrgs).ToJsonResult();
            });
        }

        /// <summary>
        /// 当前组织列表中有权限的所有下级组织
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SubordinateOrgnizations")]
        public IActionResult Get(string Organization)
        {
            return SafeExecute(() =>
            {
                if (string.IsNullOrEmpty(Organization))
                    return Result.Error($"argument [Organization] must be provide").ToJsonResult();

                var orgs = Organization.ToString().Split(',');

                var orgnizations = new List<Guid>(orgs.Length);

                foreach (var item in orgs)
                {
                    if (!Guid.TryParse(item, out Guid uid))
                        return Result.Error($"argument [Organization] format error, input is {item}").ToJsonResult();

                    orgnizations.Add(uid);
                }

                var subordinates = _organizationService.GetSubordinatOrganizations(orgnizations);

                return ResponseModel.Success(data: subordinates).ToJsonResult();
            });
        }
    }
}