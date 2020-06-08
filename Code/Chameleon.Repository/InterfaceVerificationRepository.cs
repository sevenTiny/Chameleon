using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceVerificationRepository : IMetaObjectRepositoryBase<InterfaceVerification>
    {
        /// <summary>
        /// 获取顶级接口校验
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        List<InterfaceVerification> GetTopInterfaceVerification(Guid metaObjectId);
        /// <summary>
        /// 根据父数据id获取配置下的全部接口校验配置
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<InterfaceVerification> GetInterfaceVerificationByParentId(Guid parentId);
    }

    public class InterfaceVerificationRepository : MetaObjectRepositoryBase<InterfaceVerification>, IInterfaceVerificationRepository
    {
        public InterfaceVerificationRepository(ChameleonDbContext dbContext) : base(dbContext) { }
        public List<InterfaceVerification> GetTopInterfaceVerification(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceVerification>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.ParentId == guidEmpty).ToList();
        }

        public List<InterfaceVerification> GetInterfaceVerificationByParentId(Guid parentId)
        {
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceVerification>().Where(t => t.ParentId == parentId && t.IsDeleted == 0 && t.ParentId != guidEmpty).ToList();
        }

    }
}
