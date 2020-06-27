using Chameleon.Entity;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;

namespace Chameleon.Repository
{
    public interface IMetaObjectRepositoryBase<TEntity> : ICommonRepositoryBase<TEntity> where TEntity : MetaObjectBase
    {
        void LogicDeleteByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId);
        TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name);

        MetaObject GetMetaObjectById(Guid metaObjectId);
        string GetMetaObjectCodeById(Guid metaObjectId);
        string GetMetaObjectNameById(Guid metaObjectId);
    }

    public abstract class MetaObjectRepositoryBase<TEntity> : CommonRepositoryBase<TEntity>, IMetaObjectRepositoryBase<TEntity> where TEntity : MetaObjectBase
    {
        public MetaObjectRepositoryBase(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public void LogicDeleteByMetaObjectId(Guid metaObjectId)
        {
            var entity = _dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId).FirstOrDefault();
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                _dbContext.Update(entity);
            }
        }

        public List<TEntity> GetListByMetaObjectId(Guid metaObjectId)
            => _dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId).ToList();

        public List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId)
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted && t.MetaObjectId == metaObjectId).ToList();

        public List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId)
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted && t.MetaObjectId == metaObjectId).ToList();

        /// <summary>
        /// 获取同对象下的id不同编码或者名称相同的数据
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name)
            => _dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId && t.Id != id && (t.Code.Equals(code) || t.Name.Equals(name))).FirstOrDefault();

        public MetaObject GetMetaObjectById(Guid metaObjectId)
        {
            var metaObject = _dbContext.Queryable<MetaObject>().Where(t => t.Id == metaObjectId).FirstOrDefault();

            Ensure.ArgumentNotNullOrEmpty(metaObject, nameof(metaObject), $"未找到 metaobjectId[{metaObjectId}] 对应的数据");

            return metaObject;
        }

        public string GetMetaObjectNameById(Guid metaObjectId)
        {
            return GetMetaObjectById(metaObjectId).Name;
        }

        public string GetMetaObjectCodeById(Guid metaObjectId)
        {
            return GetMetaObjectById(metaObjectId).Code;
        }
    }
}
