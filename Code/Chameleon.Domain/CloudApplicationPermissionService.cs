using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface ICloudApplicationPermissionService : ICommonServiceBase<CloudApplicationPermission>
    {
        Result AddIfNotExist(CloudApplicationPermission cloudApplicationPermission);
        void DeleteUserApplicationPermission(Guid cloudApplicationId, long userId);
        /// <summary>
        /// 获取应用授权并且将用户邮箱进行翻译显示
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        List<CloudApplicationPermission> GetCloudApplicationPermissionsByApplicationIdToView(Guid applicationId);
    }

    public class CloudApplicationPermissionService : CommonServiceBase<CloudApplicationPermission>, ICloudApplicationPermissionService
    {
        ICloudApplicationPermissionRepository _cloudApplicationPermissionRepository;
        IUserAccountRepository _userAccountRepository;
        public CloudApplicationPermissionService(IUserAccountRepository userAccountRepository, ICloudApplicationPermissionRepository repository) : base(repository)
        {
            _userAccountRepository = userAccountRepository;
            _cloudApplicationPermissionRepository = repository;
        }

        public Result AddIfNotExist(CloudApplicationPermission cloudApplicationPermission)
        {
            //获取当前用户有权限的所有应用
            var currentUserAppExist = _cloudApplicationPermissionRepository.GetCloudApplicationPermissionsByUserId(cloudApplicationPermission.UserId);

            if (currentUserAppExist != null && currentUserAppExist.Any(t => t.CloudApplicationId == cloudApplicationPermission.CloudApplicationId))
                return Result.Error("该用户已经存在该应用的权限");

            return base.AddNoCareCode(cloudApplicationPermission);
        }

        public void DeleteUserApplicationPermission(Guid cloudApplicationId, long userId)
        {
            _cloudApplicationPermissionRepository.DeleteByUserIdAndApplicationId(cloudApplicationId, userId);
        }

        public List<CloudApplicationPermission> GetCloudApplicationPermissionsByApplicationIdToView(Guid applicationId)
        {
            var permissions = _cloudApplicationPermissionRepository.GetCloudApplicationPermissionsByCloudApplicationId(applicationId);

            if (permissions == null || !permissions.Any())
                return new List<CloudApplicationPermission>(0);

            var developers = _userAccountRepository.GetDeveloperUserAccount();

            if (developers == null || !developers.Any())
                return new List<CloudApplicationPermission>(0);

            var developersDic = developers.SafeToDictionary(k => k.UserId, v => v);

            foreach (var item in permissions)
            {
                item.UserEmail = developersDic.SafeGet(item.UserId)?.Email;
            }

            return permissions;
        }
    }
}
