using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IOrganizationRepository : ICommonRepositoryBase<Organization>
    {
        /// <summary>
        /// 获取启用的组织列表
        /// </summary>
        /// <returns></returns>
        List<Organization> GetEnableList();
        List<Organization> GetDisableList();
    }

    public class OrganizationRepository : CommonRepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }

        public List<Organization> GetDisableList()
        {
            return _dbContext.Queryable<Organization>().Where(t => t.Disable == 1).ToList();
        }

        public List<Organization> GetEnableList()
        {
            return _dbContext.Queryable<Organization>().Where(t => t.Disable != 1).ToList();
        }
    }
}
