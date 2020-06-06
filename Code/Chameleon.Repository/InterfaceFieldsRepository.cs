using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceFieldsRepository : IMetaObjectRepositoryBase<InterfaceFields>
    {
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
    }

    public class InterfaceFieldsRepository : MetaObjectRepositoryBase<InterfaceFields>, IInterfaceFieldsRepository
    {
        public InterfaceFieldsRepository(ChameleonDbContext dbContext) : base(dbContext) { }
        public List<InterfaceFields> GetTopInterfaceFields(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceFields>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.ParentId == guidEmpty).ToList();
        }

        public List<InterfaceFields> GetInterfaceFieldsByParentId(Guid parentId)
        {
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceFields>().Where(t => t.ParentId == parentId && t.IsDeleted == 0 && t.ParentId != guidEmpty).ToList();
        }
    }
}
