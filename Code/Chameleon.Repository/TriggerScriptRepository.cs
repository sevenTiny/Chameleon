﻿using Chameleon.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface ITriggerScriptRepository : ICommonRepositoryBase<TriggerScript>
    {
        /// <summary>
        /// 获取对象下的对象接口列表
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        List<TriggerScript> GetMetaObjectTriggerListByMetaObjectId(Guid metaObjectId);
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
        bool CheckMetaObjectInterfaceServiceTypeExistIfMetaObjectTrigger(Guid metaObjectId, MetaObjectInterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum);
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
            return _dbContext.Queryable<TriggerScript>().Where(t => t.CloudApplicationId.Equals(applicationId) && t.ScriptType.Equals(scriptType) && t.IsDeleted == 0).ToList();
        }

        public bool CheckMetaObjectInterfaceServiceTypeExistIfMetaObjectTrigger(Guid metaObjectId, MetaObjectInterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum)
        {
            var scriptType = (int)ScriptTypeEnum.MetaObjectInterfaceTrigger;
            var metaObjectTriggerType = (int)metaObjectInterfaceServiceTypeEnum;
            return _dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId.Equals(metaObjectId) && t.ScriptType.Equals(scriptType) && t.IsDeleted == 0 && t.MetaObjectInterfaceServiceType == metaObjectTriggerType).Any();
        }
    }
}
