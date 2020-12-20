using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Application
{
    public interface IMetaObjectApp
    {
        Result AddMetaObject(Guid metaObjectId, string metaObjectCode, Guid applicationId, MetaObject metaObject);
    }

    public class MetaObjectApp: IMetaObjectApp
    {
        IMetaObjectRepository _metaObjectRepository;
        IMetaObjectService _metaObjectService;
        IMetaFieldService _metaFieldService;
        public MetaObjectApp(IMetaObjectRepository metaObjectRepository, IMetaObjectService metaObjectService, IMetaFieldService metaFieldService)
        {
            _metaFieldService = metaFieldService;
            _metaObjectRepository = metaObjectRepository;
            _metaObjectService = metaObjectService;
        }

        public Result AddMetaObject(Guid metaObjectId, string metaObjectCode, Guid applicationId, MetaObject metaObject)
        {
            Ensure.ArgumentNotNullOrEmpty(metaObjectId, nameof(metaObjectId));
            Ensure.ArgumentNotNullOrEmpty(metaObjectCode, nameof(metaObjectCode));
            Ensure.ArgumentNotNullOrEmpty(applicationId, nameof(applicationId));

            //添加对象
            _metaObjectRepository.Add(metaObject);
            //预置系统字段
            _metaFieldService.PresetSystemFields(metaObjectId, metaObjectCode, applicationId);

            return Result.Success();
        }
    }
}
