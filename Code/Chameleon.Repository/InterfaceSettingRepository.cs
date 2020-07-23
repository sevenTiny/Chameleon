using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceSettingRepository : IMetaObjectRepositoryBase<InterfaceSetting>
    {
        /// <summary>
        /// 通过应用id查未删除数据
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        List<InterfaceSetting> GetListByCloudApplicationId(Guid cloudApplicationId);
        /// <summary>
        /// 根据id获取接口设置，并且校验了是否查到
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <returns></returns>
        InterfaceSetting GetInterfaceSettingByIdWithVerify(Guid interfaceId);
        /// <summary>
        /// 根据code获取接口设置，并且校验了是否查到
        /// </summary>
        /// <param name="interfaceCode"></param>
        /// <returns></returns>
        InterfaceSetting GetInterfaceSettingByCodeWithVerify(string interfaceCode);
        /// <summary>
        /// 通过DataSourceId删除
        /// </summary>
        /// <param name="dataSourceId"></param>
        /// <returns></returns>
        Result LogicDeleteByDataSourceId(Guid dataSourceId);
    }

    public class InterfaceSettingRepository : MetaObjectRepositoryBase<InterfaceSetting>, IInterfaceSettingRepository
    {
        public InterfaceSettingRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext) { }

        public InterfaceSetting GetInterfaceSettingByIdWithVerify(Guid interfaceId)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceId, nameof(interfaceId), $"the interface id [{interfaceId}] is incorrect, please check the validity of id");

            var interfaceSetting = base.GetById(interfaceId);

            Ensure.ArgumentNotNullOrEmpty(interfaceSetting, nameof(interfaceSetting), $"the interface data of id [{interfaceId}] not found, please check the validity of id");

            return interfaceSetting;
        }

        public InterfaceSetting GetInterfaceSettingByCodeWithVerify(string interfaceCode)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode), $"the interface code [{interfaceCode}] is incorrect, please check the validity of code");

            var interfaceSetting = base.GetByCode(interfaceCode);

            Ensure.ArgumentNotNullOrEmpty(interfaceSetting, nameof(interfaceSetting), $"the interface data of code [{interfaceCode}] not found, please check the validity of code");

            return interfaceSetting;
        }

        public Result LogicDeleteByDataSourceId(Guid dataSourceId)
        {
            //空的不删除，只删除类型是数据源的
            if (dataSourceId == Guid.Empty)
                return Result.Success();

            var entities = _dbContext.Queryable<InterfaceSetting>().Where(t => t.DataSousrceId == dataSourceId && t.IsDeleted == 0).ToList();

            if (entities == null || !entities.Any())
                return Result.Success();

            entities.ForEach(t =>
            {
                t.IsDeleted = (int)IsDeleted.Deleted;

                _dbContext.Update(t);
            });

            return Result.Success();
        }

        public List<InterfaceSetting> GetListByCloudApplicationId(Guid cloudApplicationId)
        {
            return _dbContext.Queryable<InterfaceSetting>().Where(t => t.CloudApplicationId == cloudApplicationId && t.IsDeleted == 0).ToList();
        }
    }
}
