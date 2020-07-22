using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceSortRepository : IMetaObjectRepositoryBase<InterfaceSort>
    {
        /// <summary>
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<InterfaceSort> GetListByCloudApplicationId(Guid cloudApplicationId);
        /// <summary>
        /// 获取顶级接口排序
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        List<InterfaceSort> GetTopInterfaceSort(Guid metaObjectId);
        /// <summary>
        /// 根据父数据id获取配置下的全部接口排序配置
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<InterfaceSort> GetInterfaceSortByParentId(Guid parentId);
        /// <summary>
        /// 校验当前接口排序下是否已经存在某字段的排序项
        /// </summary>
        /// <param name="interfaceSortId"></param>
        /// <param name="metaFieldShortCode"></param>
        /// <returns></returns>
        bool CheckMetaFieldShortCodeHasExistInCurrentSort(Guid interfaceSortId, string metaFieldShortCode);
    }

    public class InterfaceSortRepository : MetaObjectRepositoryBase<InterfaceSort>, IInterfaceSortRepository
    {
        public InterfaceSortRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public List<InterfaceSort> GetTopInterfaceSort(Guid metaObjectId)
        {
            //Bankinate组件的一个bug，表达式解析错误，最终解析成了Guid.Empty文本
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceSort>().Where(t => t.MetaObjectId == metaObjectId && t.IsDeleted == 0 && t.ParentId == guidEmpty).ToList();
        }

        public List<InterfaceSort> GetInterfaceSortByParentId(Guid parentId)
        {
            if (parentId == Guid.Empty)
                return new List<InterfaceSort>(0);

            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceSort>().Where(t => t.ParentId == parentId && t.IsDeleted == 0 && t.ParentId != guidEmpty).ToList();
        }

        public bool CheckMetaFieldShortCodeHasExistInCurrentSort(Guid interfaceSortId, string metaFieldShortCode)
        {
            var guidEmpty = Guid.Empty;
            return _dbContext.Queryable<InterfaceSort>().Where(t => t.ParentId == interfaceSortId && t.IsDeleted == 0 && t.ParentId != guidEmpty && t.MetaFieldShortCode.Equals(metaFieldShortCode)).Any();
        }

        public List<InterfaceSort> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<InterfaceSort>().Where(t => t.CloudApplicationtId == cloudApplicationId && t.IsDeleted == 0).ToList();
        }
    }
}
