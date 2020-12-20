using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface ICloudApplicationRepository : ICommonRepositoryBase<CloudApplication>
    {
        /// <summary>
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<CloudApplication> GetListByCloudApplicationId(Guid cloudApplicationId);
    }

    public class CloudApplicationRepository : CommonRepositoryBase<CloudApplication>, ICloudApplicationRepository
    {
        public CloudApplicationRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public List<CloudApplication> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<CloudApplication>().Where(t => t.Id == cloudApplicationId && t.IsDeleted == 0).ToList();
        }
    }
}
