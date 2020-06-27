using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;

namespace Chameleon.Domain
{
    public interface IMetaObjectCommonServiceBase<TEntity> : ICommonServiceBase<TEntity> where TEntity : MetaObjectBase
    {
        Result CheckHasSameCodeOrNameWithSameMetaObjectId(Guid metaObjectId, TEntity entity);
    }

    public abstract class MetaObjectCommonServiceBase<TEntity> : CommonServiceBase<TEntity>, IMetaObjectCommonServiceBase<TEntity> where TEntity : MetaObjectBase
    {
        public MetaObjectCommonServiceBase(IMetaObjectRepositoryBase<TEntity> metaObjectCommonRepositoryBase) : base(metaObjectCommonRepositoryBase)
        {
            _metaObjectCommonRepositoryBase = metaObjectCommonRepositoryBase;
        }

        protected IMetaObjectRepositoryBase<TEntity> _metaObjectCommonRepositoryBase;

        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Result CheckHasSameCodeOrNameWithSameMetaObjectId(Guid metaObjectId, TEntity entity)
        {
            var obj = _metaObjectCommonRepositoryBase.GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(metaObjectId, entity.Id, entity.Code, entity.Name);
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return Result.Error($"编码[{obj.Code}]已存在");
                else if (obj.Name.Equals(entity.Name))
                    return Result.Error($"名称[{obj.Name}]已存在");
            }
            return Result.Success();
        }
    }
}
