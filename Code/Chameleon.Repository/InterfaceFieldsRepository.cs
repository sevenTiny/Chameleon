using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceFieldsRepository : IMetaObjectRepositoryBase<InterfaceFields>
    {
        /// <summary>
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<InterfaceFields> GetListByCloudApplicationId(Guid cloudApplicationId);
        /// <summary>
        /// 获取顶级接口字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        List<InterfaceFields> GetTopInterfaceFields(Guid metaObjectId);
        /// <summary>
        /// 根据父数据id获取配置下的全部字段配置
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<InterfaceFields> GetInterfaceFieldsByParentId(Guid parentId);
        /// <summary>
        /// 获取以字段编码大写为key的所有接口字段列表
        /// </summary>
        /// <param name="interfaceFieldId"></param>
        /// <returns></returns>
        Dictionary<string, InterfaceFields> GetInterfaceFieldMetaFieldUpperKeyDicByInterfaceFieldsId(Guid interfaceFieldId);
    }

    public class InterfaceFieldsRepository : MetaObjectRepositoryBase<InterfaceFields>, IInterfaceFieldsRepository
    {
        public InterfaceFieldsRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }
        public List<InterfaceFields> GetTopInterfaceFields(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceFields>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.ParentId == guidEmpty).ToList();
        }

        public List<InterfaceFields> GetInterfaceFieldsByParentId(Guid parentId)
        {
            if (parentId == Guid.Empty)
                return new List<InterfaceFields>(0);

            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceFields>().Where(t => t.ParentId == parentId && t.IsDeleted == 0 && t.ParentId != guidEmpty).ToList();
        }

        public Dictionary<string, InterfaceFields> GetInterfaceFieldMetaFieldUpperKeyDicByInterfaceFieldsId(Guid interfaceFieldId)
        {
            var list = GetInterfaceFieldsByParentId(interfaceFieldId);

            if (list == null || !list.Any())
                return new Dictionary<string, InterfaceFields>(0);

            return list.SafeToDictionary(k => k.MetaFieldShortCode.ToUpperInvariant(), v => v);
        }

        public List<InterfaceFields> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<InterfaceFields>().Where(t => t.CloudApplicationtId == cloudApplicationId && t.IsDeleted == 0).ToList();
        }
    }
}
