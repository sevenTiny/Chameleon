using Chameleon.Entity;
using System;
using System.Collections.Generic;
using SevenTiny.Bantina.Extensions;
using System.Text;

namespace Chameleon.Repository
{
    public interface IMetaFieldRepository : IMetaObjectRepositoryBase<MetaField>
    {
        /// <summary>
        /// 获取当前对象下的所有字段的字典形式 key=字段id
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<Guid, MetaField> GetMetaFieldIdDicByMetaObjectId(Guid metaObjectId);
    }

    public class MetaFieldRepository : MetaObjectRepositoryBase<MetaField>, IMetaFieldRepository
    {
        public MetaFieldRepository(ChameleonDbContext dbContext) : base(dbContext) { }

        public Dictionary<Guid, MetaField> GetMetaFieldIdDicByMetaObjectId(Guid metaObjectId)
        {
            var list = _dbContext.Queryable<MetaField>().Where(t => t.IsDeleted == 0 && t.MetaObjectId == metaObjectId).ToList();

            return list.SafeToDictionary(k => k.Id, v => v);
        }
    }
}
