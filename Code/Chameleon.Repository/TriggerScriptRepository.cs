using Chameleon.Entity;
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
        List<TriggerScript> GetMetaObjectInterfaceListByMetaObjectId(Guid metaObjectId);
    }

    public class TriggerScriptRepository : CommonRepositoryBase<TriggerScript>, ITriggerScriptRepository
    {
        public TriggerScriptRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public List<TriggerScript> GetMetaObjectInterfaceListByMetaObjectId(Guid metaObjectId)
        {
            return _dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId.Equals(metaObjectId) && t.ScriptType.Equals((int)ScriptTypeEnum.MetaObjectInterfaceTrigger) && t.IsDeleted == 0).ToList();
        }
    }
}
