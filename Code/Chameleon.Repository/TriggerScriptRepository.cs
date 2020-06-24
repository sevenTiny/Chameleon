using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface ITriggerScriptRepository : ICommonRepositoryBase<TriggerScript>
    {
       
    }

    public class TriggerScriptRepository : CommonRepositoryBase<TriggerScript>, ITriggerScriptRepository
    {
        public TriggerScriptRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

    }
}
