using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceConditionRepository : IMetaObjectRepositoryBase<InterfaceCondition>
    {
        /// <summary>
        /// 获取顶级接口条件
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        List<InterfaceCondition> GetTopInterfaceCondition(Guid metaObjectId);
        /// <summary>
        /// 根据父数据id获取配置下的全部条件配置
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<InterfaceCondition> GetInterfaceConditionByBelongToId(Guid parentId);
    }

    public class InterfaceConditionRepository : MetaObjectRepositoryBase<InterfaceCondition>, IInterfaceConditionRepository
    {
        public InterfaceConditionRepository(ChameleonDbContext dbContext) : base(dbContext) { }
        public List<InterfaceCondition> GetTopInterfaceCondition(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceCondition>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.BelongToCondition == guidEmpty).ToList();
        }

        public List<InterfaceCondition> GetInterfaceConditionByBelongToId(Guid parentId)
        {
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceCondition>().Where(t => t.BelongToCondition == parentId && t.IsDeleted == 0 && t.BelongToCondition != guidEmpty).ToList();
        }
    }
}
