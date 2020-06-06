using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface ICloudApplicationRepository : ICommonRepositoryBase<CloudApplication>
    {
    }

    public class CloudApplicationRepository : CommonRepositoryBase<CloudApplication>, ICloudApplicationRepository
    {
        public CloudApplicationRepository(ChameleonDbContext dbContext) : base(dbContext) { }
    }
}
