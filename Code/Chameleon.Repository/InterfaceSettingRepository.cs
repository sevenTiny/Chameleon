using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceSettingRepository : IMetaObjectRepositoryBase<InterfaceSetting>
    {
    }

    public class InterfaceSettingRepository : MetaObjectRepositoryBase<InterfaceSetting>, IInterfaceSettingRepository
    {
        public InterfaceSettingRepository(ChameleonDbContext dbContext) : base(dbContext) { }
    }
}
