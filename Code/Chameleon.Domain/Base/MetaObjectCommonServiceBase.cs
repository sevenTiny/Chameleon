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
        void LogicDeleteByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId);
        TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name);

        MetaObject GetMetaObjectById(Guid metaObjectId);
        string GetMetaObjectCodeById(Guid metaObjectId);
        string GetMetaObjectNameById(Guid metaObjectId);
    }

    internal abstract class MetaObjectCommonServiceBase<TEntity> : CommonServiceBase<TEntity>, IMetaObjectCommonServiceBase<TEntity> where TEntity : MetaObjectBase
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

        public void LogicDeleteByMetaObjectId(Guid metaObjectId)
        {
            _metaObjectCommonRepositoryBase.LogicDeleteByMetaObjectId(metaObjectId);
        }

        public List<TEntity> GetListByMetaObjectId(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetListByMetaObjectId(metaObjectId);
        }

        public List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetListDeletedByMetaObjectId(metaObjectId);
        }

        public List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetListUnDeletedByMetaObjectId(metaObjectId);
        }

        public TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name)
        {
            return _metaObjectCommonRepositoryBase.GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(metaObjectId, id, code, name);
        }

        public MetaObject GetMetaObjectById(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetMetaObjectById(metaObjectId);
        }

        public string GetMetaObjectNameById(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetMetaObjectNameById(metaObjectId);
        }

        public string GetMetaObjectCodeById(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetMetaObjectCodeById(metaObjectId);
        }
    }
}
