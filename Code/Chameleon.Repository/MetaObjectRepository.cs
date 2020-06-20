using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IMetaObjectRepository : ICommonRepositoryBase<MetaObject>
    {
        List<MetaObject> GetMetaObjectListUnDeletedByApplicationId(Guid applicationId);
        List<MetaObject> GetMetaObjectListDeletedByApplicationId(Guid applicationId);
        MetaObject GetMetaObjectByCodeOrNameWithApplicationId(Guid applicationId, string code, string name);
        MetaObject GetMetaObjectByCodeAndApplicationId(Guid applicationId, string code);
    }

    public class MetaObjectRepository : CommonRepositoryBase<MetaObject>, IMetaObjectRepository
    {
        public MetaObjectRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public List<MetaObject> GetMetaObjectListUnDeletedByApplicationId(Guid applicationId)
            => _dbContext.Queryable<MetaObject>().Where(t => t.CloudApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();

        public List<MetaObject> GetMetaObjectListDeletedByApplicationId(Guid applicationId)
            => _dbContext.Queryable<MetaObject>().Where(t => t.CloudApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public MetaObject GetMetaObjectByCodeOrNameWithApplicationId(Guid applicationId, string code, string name)
            => _dbContext.Queryable<MetaObject>().Where(t => t.CloudApplicationId == applicationId && (t.Name.Equals(name) || t.Code.Equals(code))).FirstOrDefault();

        public MetaObject GetMetaObjectByCodeAndApplicationId(Guid applicationId, string code)
            => _dbContext.Queryable<MetaObject>().Where(t => t.CloudApplicationId == applicationId && t.Code.Equals(code)).FirstOrDefault();
    }
}
