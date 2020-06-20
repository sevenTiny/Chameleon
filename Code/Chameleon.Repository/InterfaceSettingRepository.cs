using Chameleon.Entity;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IInterfaceSettingRepository : IMetaObjectRepositoryBase<InterfaceSetting>
    {
        /// <summary>
        /// 根据id获取接口设置，并且校验了是否查到
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <returns></returns>
        InterfaceSetting GetInterfaceSettingByIdWithVerify(Guid interfaceId);
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
    }
}
