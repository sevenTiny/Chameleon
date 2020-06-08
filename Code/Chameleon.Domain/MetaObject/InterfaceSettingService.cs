using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface IInterfaceSettingService : IMetaObjectCommonServiceBase<InterfaceSetting>
    {
        
    }

    public class InterfaceSettingService : MetaObjectCommonServiceBase<InterfaceSetting>, IInterfaceSettingService
    {
        IInterfaceSettingRepository _InterfaceSettingRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceSettingService(IInterfaceSettingRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _metaFieldRepository = metaFieldRepository;
            _InterfaceSettingRepository = repository;
        }

    }
}
