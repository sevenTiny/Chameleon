using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IMenuRepository : ICommonRepositoryBase<Menu>
    {
    }

    public class MenuRepository : CommonRepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }
    }
}
