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
    public class OrgnizationController : ApiControllerCommonBase
    {
        IOrganizationService _organizationService;
        IOrganizationRepository _organizationRepository;
        public OrgnizationController(IOrganizationRepository organizationRepository, IOrganizationService organizationService)
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
        public IActionResult EnableOrgnizations()
        {
            return SafeExecute(() =>
            {
                var enableOrgs = _organizationRepository.GetEnableList() ?? new List<Organization>(0);

                return ResponseModel.Success(data: enableOrgs).ToJsonResult();
            });
        }

        /// <summary>
        /// 获取所有下级组织
        /// </summary>
        /// <param name="Organization"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SubordinateOrgnizations")]
        public IActionResult SubordinateOrgnizations(string Organization, int level)
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

                var subordinates = _organizationService.GetSubordinatOrganizations(orgnizations, level);

                return ResponseModel.Success(data: subordinates).ToJsonResult();
            });
        }
    }
}