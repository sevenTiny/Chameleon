using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IFunctionRepository : ICommonRepositoryBase<Function>
    {
    }

    public class FunctionRepository : CommonRepositoryBase<Function>, IFunctionRepository
    {
        public FunctionRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
