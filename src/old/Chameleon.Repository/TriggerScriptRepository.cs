using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface ITriggerScriptRepository : ICommonRepositoryBase<TriggerScript>
    {
        /// <summary>
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<TriggerScript> GetListByCloudApplicationId(Guid cloudApplicationId);
        /// <summary>
        /// 通过应用id查未删除数量
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        long GetCountByCloudApplicationId(Guid cloudApplicationId);
        /// <summary>
        /// 获取对象下的对象触发器列表
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        List<TriggerScript> GetMetaObjectTriggerListByMetaObjectId(Guid metaObjectId);
        /// <summary>
        /// 获取应用触发器列表
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<TriggerScript> GetCloudApplicationTriggerListByCloudApplicationId(Guid cloudApplicationId);
        /// <summary>
        /// 获取动态脚本数据源列表
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="scriptTypeEnum"></param>
        /// <returns></returns>
        List<TriggerScript> GetDataSourceListByApplicationId(Guid applicationId, ScriptTypeEnum scriptTypeEnum);
        /// <summary>
        /// 校验当前对象下是否已经存在一个同服务类型的脚本
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="metaObjectInterfaceServiceTypeEnum"></param>
        /// <returns></returns>
        bool CheckMetaObjectInterfaceServiceTypeExistIfMetaObjectTrigger(Guid metaObjectId, InterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum);
        /// <summary>
        /// 判断当前应用下是否已经有一个同类型的脚本
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <param name="scriptTypeEnum"></param>
        /// <param name="interfaceServiceTypeEnum"></param>
        /// <returns></returns>
        bool CheckInterfaceServiceTypeExistInCloudApplicationTrigger(Guid cloudApplicationId, ScriptTypeEnum scriptTypeEnum, InterfaceServiceTypeEnum interfaceServiceTypeEnum);
    }

    public class TriggerScriptRepository : CommonRepositoryBase<TriggerScript>, ITriggerScriptRepository
    {
        public TriggerScriptRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public List<TriggerScript> GetMetaObjectTriggerListByMetaObjectId(Guid metaObjectId)
        {
            var scriptType = (int)ScriptTypeEnum.MetaObjectInterfaceTrigger;
            return _dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId.Equals(metaObjectId) && t.ScriptType.Equals(scriptType) && t.IsDeleted == 0).ToList();
        }

        public List<TriggerScript> GetDataSourceListByApplicationId(Guid applicationId, ScriptTypeEnum scriptTypeEnum)
        {
            var scriptType = (int)scriptTypeEnum;
            return _dbContext.Queryable<TriggerScript>().Where(t => t.CloudApplicationId.Equals(applicationId) && t.ScriptType.Equals(scriptType) && t.IsDeleted == 0).ToList() ?? new List<TriggerScript>(0);
        }

        public bool CheckMetaObjectInterfaceServiceTypeExistIfMetaObjectTrigger(Guid metaObjectId, InterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum)
        {
            var scriptType = (int)ScriptTypeEnum.MetaObjectInterfaceTrigger;
            var metaObjectTriggerType = (int)metaObjectInterfaceServiceTypeEnum;
            return _dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId.Equals(metaObjectId) && t.ScriptType.Equals(scriptType) && t.IsDeleted == 0 && t.InterfaceServiceType == metaObjectTriggerType).Any();
        }

        public bool CheckInterfaceServiceTypeExistInCloudApplicationTrigger(Guid cloudApplicationId, ScriptTypeEnum scriptTypeEnum, InterfaceServiceTypeEnum interfaceServiceTypeEnum)
        {
            var scriptType = (int)scriptTypeEnum;
            var interfaceServiceType = (int)interfaceServiceTypeEnum;
            return _dbContext.Queryable<TriggerScript>().Where(t => t.CloudApplicationId.Equals(cloudApplicationId) && t.ScriptType.Equals(scriptType) && t.IsDeleted == 0 && t.InterfaceServiceType == interfaceServiceType).Any();
        }

        public List<TriggerScript> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<TriggerScript>().Where(t => t.CloudApplicationId == cloudApplicationId && t.IsDeleted == 0).ToList();
        }

        public List<TriggerScript> GetCloudApplicationTriggerListByCloudApplicationId(Guid cloudApplicationId)
        {
            //以后扩展其他类型应用触发器在这里添加条件
            int scriptType = (int)ScriptTypeEnum.FileManagement;
            return _dbContext.Queryable<TriggerScript>().Where(t => t.CloudApplicationId.Equals(cloudApplicationId) && t.ScriptType == scriptType && t.IsDeleted == 0).ToList();
        }

        public long GetCountByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<TriggerScript>().Where(t => t.CloudApplicationId == cloudApplicationId && t.IsDeleted == 0).Count();
        }
    }
}
