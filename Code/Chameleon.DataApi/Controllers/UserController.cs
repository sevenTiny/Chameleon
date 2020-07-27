using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerCommonBase
    {
        IOrganizationService _organizationService;
        IUserAccountRepository _userAccountRepository;
        public UserController(IUserAccountRepository userAccountRepository, IOrganizationService organizationService)
        {
            _userAccountRepository = userAccountRepository;
            _organizationService = organizationService;
        }

        /// <summary>
        /// 获取所有人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllUsers")]
        public IActionResult AllUsers()
        {
            return SafeExecute(() =>
            {
                var allUsers = _userAccountRepository.GetUserAccountList() ?? new List<UserAccount>(0);

                return ResponseModel.Success(data: allUsers).ToJsonResult();
            });
        }

        /// <summary>
        /// 当前组织列表中所有下级组织下的人员
        /// </summary>
        /// <param name="Organization"></param>
        /// <param name="level">层级</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AllOrgnizationsUsers")]
        public IActionResult AllOrgnizationsUsers(string Organization, int level)
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

                //获取有权限的所有下级组织
                var subordinates = _organizationService.GetSubordinatOrganizations(orgnizations, level);

                //获取所有组织下的人员
                var allUsers = _userAccountRepository.GetUserAccountList() ?? new List<UserAccount>(0);

                //所有组织内的人员
                var useResult = allUsers.Where(t => subordinates.Contains(t.Organization.ToString())).ToList();

                return ResponseModel.Success(data: useResult).ToJsonResult();
            });
        }

        /// <summary>
        /// 当前组织列表中所有下级组织下的人员UserId
        /// </summary>
        /// <param name="Organization"></param>
        /// <param name="level">层级</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AllOrgnizationsUserId")]
        public IActionResult AllOrgnizationsUserIds(string Organization, int level)
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

                //获取有权限的所有下级组织
                var subordinates = _organizationService.GetSubordinatOrganizations(orgnizations, level);

                //获取所有组织下的人员
                var allUsers = _userAccountRepository.GetUserAccountList() ?? new List<UserAccount>(0);

                //所有组织内的人员
                var useResult = allUsers.Where(t => subordinates.Contains(t.Organization.ToString())).Select(t => t.UserId).ToList();

                return ResponseModel.Success(data: useResult).ToJsonResult();
            });
        }

        /// <summary>
        /// 当前组织列表中所有下级组织下的人员UserId
        /// </summary>
        /// <param name="Organization"></param>
        /// <param name="level">层级</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AllOrgnizationsUserIdEmailNamePhone")]
        public IActionResult AllOrgnizationsUserIdEmailNamePhone(string Organization, int level)
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

                //获取有权限的所有下级组织
                var subordinates = _organizationService.GetSubordinatOrganizations(orgnizations, level);

                //获取所有组织下的人员
                var allUsers = _userAccountRepository.GetUserAccountList() ?? new List<UserAccount>(0);

                //所有组织内的人员
                var useResult = allUsers.Where(t => subordinates.Contains(t.Organization.ToString()))
                .Select(t => new
                {
                    UserId = t.UserId,
                    Email = t.Email,
                    Name = t.Name,
                    Phone = t.Phone
                }).ToList();

                return ResponseModel.Success(data: useResult).ToJsonResult();
            });
        }

        /// <summary>
        /// 查询人员账户信息
        /// </summary>
        /// <param name="UserId">多个英文逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByUserIds")]
        public IActionResult GetUserByUserIds(string UserId)
        {
            return SafeExecute(() =>
            {
                if (string.IsNullOrEmpty(UserId))
                    return Result.Error($"argument [UserId] must be provide").ToJsonResult();

                var userIds = UserId.ToString().Split(',');

                var userIdss = new List<long>(userIds.Length);

                foreach (var item in userIds)
                {
                    if (!long.TryParse(item, out long uid))
                        return Result.Error($"argument [UserId] format error, input is {item}").ToJsonResult();

                    userIdss.Add(uid);
                }

                //获取所有组织下的人员
                var allUsers = _userAccountRepository.GetUserAccountList() ?? new List<UserAccount>(0);

                //所有组织内的人员
                var useResult = allUsers.Where(t => userIdss.Contains(t.UserId)).ToList();

                //清空密码字段，密码不对外显示
                useResult.ForEach(t => t.Password = string.Empty);

                return ResponseModel.Success(data: useResult).ToJsonResult();
            });
        }

        /// <summary>
        /// 查询人员账户信息
        /// </summary>
        /// <param name="UserId">多个英文逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserUserIdEmailNamePhoneByUserId")]
        public IActionResult GetUserUserIdEmailNamePhoneByUserId(string UserId)
        {
            return SafeExecute(() =>
            {
                if (string.IsNullOrEmpty(UserId))
                    return Result.Error($"argument [UserId] must be provide").ToJsonResult();

                var userIds = UserId.ToString().Split(',');

                var userIdss = new List<long>(userIds.Length);

                foreach (var item in userIds)
                {
                    if (!long.TryParse(item, out long uid))
                        return Result.Error($"argument [UserId] format error, input is {item}").ToJsonResult();

                    userIdss.Add(uid);
                }

                //获取所有人员
                var allUsers = _userAccountRepository.GetUserAccountList() ?? new List<UserAccount>(0);

                //所有范围内人员
                var useResult = allUsers.Where(t => userIdss.Contains(t.UserId))
                .Select(t => new
                {
                    UserId = t.UserId,
                    Email = t.Email,
                    Name = t.Name,
                    Phone = t.Phone
                }).ToList();

                return ResponseModel.Success(data: useResult).ToJsonResult();
            });
        }
    }
}