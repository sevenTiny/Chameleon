using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IProfileRepository : ICommonRepositoryBase<Profile>
    {
    }

    public class ProfileRepository : CommonRepositoryBase<Profile>, IProfileRepository
    {
        public ProfileRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
