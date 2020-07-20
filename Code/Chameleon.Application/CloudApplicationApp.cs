using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Application
{
    public interface ICloudApplicationApp
    {
        List<CloudApplication> GetUserPermissionApplications(long userId);
        Result AddCloudApplication(CloudApplication cloudApplication);
    }

    public class CloudApplicationApp : ICloudApplicationApp
    {
        ICloudApplicationService _cloudApplicationService;
        ICloudApplicationPermissionService _cloudApplicationPermissionService;
        ICloudApplicationPermissionRepository _cloudApplicationPermissionRepository;
        public CloudApplicationApp(ICloudApplicationPermissionRepository cloudApplicationPermissionRepository, ICloudApplicationPermissionService cloudApplicationPermissionService, ICloudApplicationService cloudApplicationService)
        {
            _cloudApplicationPermissionRepository = cloudApplicationPermissionRepository;
            _cloudApplicationPermissionService = cloudApplicationPermissionService;
            _cloudApplicationService = cloudApplicationService;
        }

        public List<CloudApplication> GetUserPermissionApplications(long userId)
        {
            //未删除所有应用
            var allApps = _cloudApplicationService.GetListUnDeleted();

            if (allApps == null || !allApps.Any())
                return new List<CloudApplication>(0);

            //有权限的应用id集合
            var permissionsAppIds = _cloudApplicationPermissionRepository.GetUserPermissionCloudApplicationIds(userId);

            if (permissionsAppIds == null || !permissionsAppIds.Any())
                return new List<CloudApplication>(0);

            return allApps.Where(t => permissionsAppIds.Contains(t.Id)).ToList();
        }

        public Result AddCloudApplication(CloudApplication cloudApplication)
        {
            return Result.Success("操作成功")
                .ContinueEnsureArgumentNotNullOrEmpty(cloudApplication, nameof(cloudApplication))
                .Continue(_ =>
                {
                    if (cloudApplication.Id == Guid.Empty)
                        cloudApplication.Id = Guid.NewGuid();
                    return _;
                })
                .Continue(_ => _cloudApplicationService.AddCheckCode(cloudApplication))
                .Continue(_ => _cloudApplicationPermissionService.AddNoCareCode(new CloudApplicationPermission
                {
                    CloudApplicationId = cloudApplication.Id,
                    UserId = cloudApplication.CreateBy
                }));
        }
    }
}
