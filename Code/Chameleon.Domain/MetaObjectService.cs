using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Domain
{
    public interface IMetaObjectService : ICommonServiceBase<MetaObject>
    {
        
    }

    public class MetaObjectService : CommonServiceBase<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(IMetaObjectRepository repository) : base(repository)
        {
        }
    }
}
