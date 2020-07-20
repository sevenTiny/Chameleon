using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Repository
{
    public interface ICloudApplicationPermissionRepository : ICommonRepositoryBase<CloudApplicationPermission>
    {
        List<Guid> GetUserPermissionCloudApplicationIds(long userId);
        List<CloudApplicationPermission> GetCloudApplicationPermissionsByUserId(long userId);
        void DeleteByUserIdAndApplicationId(Guid applicationId, long userId);
        List<CloudApplicationPermission> GetCloudApplicationPermissionsByCloudApplicationId(Guid cloudApplicationId);
    }

    public class CloudApplicationPermissionRepository : CommonRepositoryBase<CloudApplicationPermission>, ICloudApplicationPermissionRepository
    {
        public CloudApplicationPermissionRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public List<Guid> GetUserPermissionCloudApplicationIds(long userId)
        {
            var permissions = GetCloudApplicationPermissionsByUserId(userId);

            if (permissions == null || !permissions.Any())
                return new List<Guid>(0);

            return permissions.Select(t => t.CloudApplicationId).ToList();
        }

        public List<CloudApplicationPermission> GetCloudApplicationPermissionsByUserId(long userId)
        {
            return _dbContext.Queryable<CloudApplicationPermission>().Where(t => t.IsDeleted == 0 && t.UserId == userId).ToList();
        }

        public void DeleteByUserIdAndApplicationId(Guid applicationId, long userId)
        {
            _dbContext.Delete<CloudApplicationPermission>(t => t.CloudApplicationId == applicationId && t.UserId == userId);
        }

        public List<CloudApplicationPermission> GetCloudApplicationPermissionsByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<CloudApplicationPermission>().Where(t => t.IsDeleted == 0 && t.CloudApplicationId == cloudApplicationId).ToList();
        }
    }
}
