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
        Result Add(Guid applicationId, string applicationCode, MetaObject metaObject);
    }

    public class MetaObjectService : CommonServiceBase<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(IMetaObjectRepository repository) : base(repository)
        {
        }

        public Result Add(Guid applicationId, string applicationCode, MetaObject metaObject)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(applicationCode, nameof(applicationCode))
                .ContinueEnsureArgumentNotNullOrEmpty(metaObject, nameof(metaObject))
                .ContinueEnsureArgumentNotNullOrEmpty(metaObject.Name, nameof(metaObject.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(metaObject.Code, nameof(metaObject.Code))
                //设置默认字段值
                .Continue(_ =>
                {
                    metaObject.CloudApplicationId = applicationId;
                    metaObject.Code = string.Concat(applicationCode, ".", metaObject.Code);
                    return _;
                })
                .Continue(_ => base.Add(metaObject));
        }
    }
}
