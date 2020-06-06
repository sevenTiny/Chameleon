using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IMetaFieldRepository : IMetaObjectRepositoryBase<MetaField>
    {

    }

    public class MetaFieldRepository : MetaObjectRepositoryBase<MetaField>, IMetaFieldRepository
    {
        public MetaFieldRepository(ChameleonDbContext dbContext) : base(dbContext) { }

    }
}
