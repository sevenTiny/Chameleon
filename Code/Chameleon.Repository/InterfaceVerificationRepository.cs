using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceVerificationRepository : IMetaObjectRepositoryBase<InterfaceVerification>
    {
        /// <summary>
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<InterfaceVerification> GetListByCloudApplicationId(Guid cloudApplicationId);
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
        /// <summary>
        /// 获取按字段短编码大写为key的接口校验
        /// </summary>
        /// <param name="interfaceVerificationId"></param>
        /// <returns></returns>
        Dictionary<string, InterfaceVerification> GetMetaFieldUpperKeyDicByInterfaceVerificationId(Guid interfaceVerificationId);
        /// <summary>
        /// 校验当前接口校验下是否已经存在某字段的校验项
        /// </summary>
        /// <param name="interfaceVerificationId"></param>
        /// <param name="metaFieldShortCode"></param>
        /// <returns></returns>
        bool CheckMetaFieldShortCodeHasExistInCurrentVerification(Guid interfaceVerificationId, string metaFieldShortCode);
    }

    public class InterfaceVerificationRepository : MetaObjectRepositoryBase<InterfaceVerification>, IInterfaceVerificationRepository
    {
        public InterfaceVerificationRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }
        public List<InterfaceVerification> GetTopInterfaceVerification(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceVerification>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.ParentId == guidEmpty).ToList();
        }

        public bool CheckMetaFieldShortCodeHasExistInCurrentVerification(Guid interfaceVerificationId, string metaFieldShortCode)
        {
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceVerification>().Where(t => t.ParentId == interfaceVerificationId && t.IsDeleted == 0 && t.ParentId != guidEmpty && t.MetaFieldShortCode.Equals(metaFieldShortCode)).Any();
        }

        public List<InterfaceVerification> GetInterfaceVerificationByParentId(Guid parentId)
        {
            if (parentId == Guid.Empty)
                return new List<InterfaceVerification>(0);

            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceVerification>().Where(t => t.ParentId == parentId && t.IsDeleted == 0 && t.ParentId != guidEmpty).ToList();
        }

        public Dictionary<string, InterfaceVerification> GetMetaFieldUpperKeyDicByInterfaceVerificationId(Guid interfaceVerificationId)
        {
            var list = GetInterfaceVerificationByParentId(interfaceVerificationId);

            if (list == null || !list.Any())
                return new Dictionary<string, InterfaceVerification>(0);

            return list.SafeToDictionary(k => k.MetaFieldShortCode.ToUpperInvariant(), v => v);
        }

        public List<InterfaceVerification> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<InterfaceVerification>().Where(t => t.CloudApplicationtId == cloudApplicationId && t.IsDeleted == 0).ToList();
        }
    }
}
