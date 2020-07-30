using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.Domain
{
    public interface IProfileService : ICommonServiceBase<Profile>
    {
        
    }

    public class ProfileService : CommonServiceBase<Profile>, IProfileService
    {
        IProfileRepository _ProfileRepository;
        public ProfileService(IProfileRepository ProfileRepository) : base(ProfileRepository)
        {
            _ProfileRepository = ProfileRepository;
        }
    }
}
