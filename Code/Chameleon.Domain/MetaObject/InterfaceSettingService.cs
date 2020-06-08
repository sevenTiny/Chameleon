using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface IInterfaceSettingService : IMetaObjectCommonServiceBase<InterfaceSetting>
    {
        /// <summary>
        /// 获取未删除的接口列表（带翻译）
        /// </summary>
        /// <param name="metaObjectid"></param>
        /// <returns></returns>
        List<InterfaceSetting> GetInterfaceSettingsTranslated(Guid metaObjectid);
    }

    public class InterfaceSettingService : MetaObjectCommonServiceBase<InterfaceSetting>, IInterfaceSettingService
    {
        IInterfaceSettingRepository _InterfaceSettingRepository;
        IMetaFieldRepository _metaFieldRepository;
        IInterfaceConditionRepository _interfaceConditionRepository;
        IInterfaceVerificationRepository _interfaceVerificationRepository;
        IInterfaceFieldsRepository _interfaceFieldsRepository;
        public InterfaceSettingService(IInterfaceFieldsRepository interfaceFieldsRepository, IInterfaceVerificationRepository interfaceVerificationRepository, IInterfaceConditionRepository interfaceConditionRepository, IInterfaceSettingRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _interfaceFieldsRepository = interfaceFieldsRepository;
            _interfaceVerificationRepository = interfaceVerificationRepository;
            _interfaceConditionRepository = interfaceConditionRepository;
            _metaFieldRepository = metaFieldRepository;
            _InterfaceSettingRepository = repository;
        }

        public List<InterfaceSetting> GetInterfaceSettingsTranslated(Guid metaObjectId)
        {
            var interfaces = _InterfaceSettingRepository.GetListUnDeletedByMetaObjectId(metaObjectId);

            if (interfaces == null || !interfaces.Any())
                return new List<InterfaceSetting>(0);

            var conditions = _interfaceConditionRepository.GetTopInterfaceCondition(metaObjectId);
            var interfaceVerifications = _interfaceVerificationRepository.GetTopInterfaceVerification(metaObjectId);
            var fields = _interfaceFieldsRepository.GetTopInterfaceFields(metaObjectId);

            interfaces.ForEach(item =>
            {
                item.InterfaceConditionName = conditions?.FirstOrDefault(t => t.Id == item.InterfaceConditionId)?.Name;
                item.InterfaceVerificationName = interfaceVerifications?.FirstOrDefault(t => t.Id == item.InterfaceVerificationId)?.Name;
                item.InterfaceFieldsName = fields?.FirstOrDefault(t => t.Id == item.InterfaceFieldsId)?.Name;
            });

            return interfaces;
        }
    }
}
