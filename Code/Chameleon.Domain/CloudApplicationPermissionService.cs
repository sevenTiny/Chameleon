using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
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
    }

    public class CloudApplicationPermissionService : CommonServiceBase<CloudApplicationPermission>, ICloudApplicationPermissionService
    {
        ICloudApplicationPermissionRepository _cloudApplicationPermissionRepository;
        public CloudApplicationPermissionService(ICloudApplicationPermissionRepository repository) : base(repository)
        {
            _cloudApplicationPermissionRepository = repository;
        }

        public Result AddIfNotExist(CloudApplicationPermission cloudApplicationPermission)
        {
            //获取当前用户有权限的所有应用
            var currentUserAppExist = _cloudApplicationPermissionRepository.GetCloudApplicationPermissionsByUserId(cloudApplicationPermission.UserId);

            if (currentUserAppExist.Any(t => t.CloudApplicationId == cloudApplicationPermission.CloudApplicationId))
                return Result.Error("该用户已经存在该应用的权限");

            return base.AddNoCareCode(cloudApplicationPermission);
        }

        public void DeleteUserApplicationPermission(Guid cloudApplicationId, long userId)
        {
            _cloudApplicationPermissionRepository.DeleteByUserIdAndApplicationId(cloudApplicationId, userId);
        }
    }
}
