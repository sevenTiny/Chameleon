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
        /// <summary>
        /// 获取当前对象下所有字段以编码为Key的字典
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldCodeDicByMetaObjectId(Guid metaObjectId);
        /// <summary>
        /// 获取当前对象下所有字段以编码为Key大写的字典
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldShortCodeUpperDicByMetaObjectId(Guid metaObjectId);
    }

    public class MetaFieldRepository : MetaObjectRepositoryBase<MetaField>, IMetaFieldRepository
    {
        public MetaFieldRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public Dictionary<Guid, MetaField> GetMetaFieldIdDicByMetaObjectId(Guid metaObjectId)
        {
            var list = _dbContext.Queryable<MetaField>().Where(t => t.IsDeleted == 0 && t.MetaObjectId == metaObjectId).ToList();

            return list.SafeToDictionary(k => k.Id, v => v);
        }

        public Dictionary<string, MetaField> GetMetaFieldCodeDicByMetaObjectId(Guid metaObjectId)
        {
            var list = _dbContext.Queryable<MetaField>().Where(t => t.IsDeleted == 0 && t.MetaObjectId == metaObjectId).ToList();
            return list.SafeToDictionary(k => k.Code, v => v);
        }

        public Dictionary<string, MetaField> GetMetaFieldShortCodeUpperDicByMetaObjectId(Guid metaObjectId)
        {
            var list = _dbContext.Queryable<MetaField>().Where(t => t.IsDeleted == 0 && t.MetaObjectId == metaObjectId).ToList();
            return list.SafeToDictionary(k => k.ShortCode.ToUpperInvariant(), v => v);
        }
    }
}
