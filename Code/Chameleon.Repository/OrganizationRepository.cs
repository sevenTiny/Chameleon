using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IOrganizationRepository : ICommonRepositoryBase<Organization>
    {
       
    }

    public class OrganizationRepository : CommonRepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }

       
    }
}
