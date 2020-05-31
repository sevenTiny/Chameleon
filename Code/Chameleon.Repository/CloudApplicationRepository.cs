using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    internal interface ICloudApplicationRepository : ICommonRepositoryBase<CloudApplication>
    {
    }

    internal class CloudApplicationRepository : CommonRepositoryBase<CloudApplication>, ICloudApplicationRepository
    {
        public CloudApplicationRepository(ChameleonDbContext dbContext) : base(dbContext) { }
    }
}
