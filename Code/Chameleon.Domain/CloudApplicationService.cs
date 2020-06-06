using Chameleon.Entity;
using Chameleon.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Domain
{
    public interface ICloudApplicationService : ICommonServiceBase<CloudApplication>
    {

    }

    public class CloudApplicationService : CommonServiceBase<CloudApplication>, ICloudApplicationService
    {
        public CloudApplicationService(ICloudApplicationRepository repository) : base(repository)
        {
        }
    }
}
