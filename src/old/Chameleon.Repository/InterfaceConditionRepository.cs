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
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<InterfaceCondition> GetListByCloudApplicationId(Guid cloudApplicationId);
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
        /// <summary>
        /// 获取接口条件为参数传递的集合
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<InterfaceCondition> GetInterfaceConditionArgumentNodeByBelongToId(Guid parentId);
    }

    public class InterfaceConditionRepository : MetaObjectRepositoryBase<InterfaceCondition>, IInterfaceConditionRepository
    {
        public InterfaceConditionRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }
        public List<InterfaceCondition> GetTopInterfaceCondition(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceCondition>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.BelongToCondition == guidEmpty).ToList();
        }

        public List<InterfaceCondition> GetInterfaceConditionByBelongToId(Guid parentId)
        {
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceCondition>().Where(t => t.BelongToCondition == parentId && t.IsDeleted == 0).ToList();
        }

        public List<InterfaceCondition> GetInterfaceConditionArgumentNodeByBelongToId(Guid parentId)
        {
            var conditionNodeType = (int)NodeTypeEnum.Condition;
            var conditionValueType = (int)ConditionValueTypeEnum.Parameter;
            return _dbContext.Queryable<InterfaceCondition>().Where(t => t.BelongToCondition == parentId && t.IsDeleted == 0 && t.ConditionNodeType == conditionNodeType && t.ConditionValueType == conditionValueType).ToList();
        }

        public List<InterfaceCondition> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<InterfaceCondition>().Where(t => t.CloudApplicationId == cloudApplicationId && t.IsDeleted == 0).ToList();
        }
    }
}
