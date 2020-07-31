using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Chameleon.ValueObject;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Application
{
    public interface IStatisticsApp
    {
        /// <summary>
        /// 开发态统计
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        ChameleonStatistics GetDevelopmentStatistics(Guid application);
        /// <summary>
        /// Account系统统计
        /// </summary>
        /// <returns></returns>
        ChameleonStatistics GetUserAccountStatistics();
    }

    public class StatisticsApp : IStatisticsApp
    {
        ICloudApplicationRepository _cloudApplicationRepository;
        IMetaObjectRepository _metaObjectRepository;
        IMetaFieldRepository _metaFieldRepository;
        ITriggerScriptRepository _triggerScriptRepository;
        IInterfaceSettingRepository _interfaceSettingRepository;
        IInterfaceFieldsRepository _interfaceFieldsRepository;
        IInterfaceConditionRepository _interfaceConditionRepository;
        IInterfaceSortRepository _interfaceSortRepository;
        IInterfaceVerificationRepository _interfaceVerificationRepository;
        IProfileRepository _profileRepository;
        IMenuRepository _menuRepository;
        IFunctionRepository _functionRepository;
        IUserAccountRepository _userAccountRepository;
        public StatisticsApp(
            IUserAccountRepository userAccountRepository,
            IProfileRepository profileRepository,
            IMenuRepository menuRepository,
            IFunctionRepository functionRepository,
            ICloudApplicationRepository cloudApplicationRepository,
            IMetaObjectRepository metaObjectRepository,
            IMetaFieldRepository metaFieldRepository,
            ITriggerScriptRepository triggerScriptRepository,
            IInterfaceSettingRepository interfaceSettingRepository,
            IInterfaceFieldsRepository interfaceFieldsRepository,
            IInterfaceConditionRepository interfaceConditionRepository,
            IInterfaceSortRepository interfaceSortRepository,
            IInterfaceVerificationRepository interfaceVerificationRepository
            )
        {
            _userAccountRepository = userAccountRepository;
            _functionRepository = functionRepository;
            _menuRepository = menuRepository;
            _profileRepository = profileRepository;
            _cloudApplicationRepository = cloudApplicationRepository;
            _metaObjectRepository = metaObjectRepository;
            _metaFieldRepository = metaFieldRepository;
            _triggerScriptRepository = triggerScriptRepository;
            _interfaceSettingRepository = interfaceSettingRepository;
            _interfaceFieldsRepository = interfaceFieldsRepository;
            _interfaceConditionRepository = interfaceConditionRepository;
            _interfaceSortRepository = interfaceSortRepository;
            _interfaceVerificationRepository = interfaceVerificationRepository;
        }

        public ChameleonStatistics GetDevelopmentStatistics(Guid application)
        {
            return new ChameleonStatistics
            {
                MetaFieldCount = _metaFieldRepository.GetCountByCloudApplicationId(application),
                MetaObjectCount = _metaObjectRepository.GetCountByCloudApplicationId(application),
                InterfaceFieldsCount = _interfaceFieldsRepository.GetCountByCloudApplicationId(application),
                InterfaceConditionCount = _interfaceConditionRepository.GetCountByCloudApplicationId(application),
                InterfaceVerificationCount = _interfaceVerificationRepository.GetCountByCloudApplicationId(application),
                InterfaceSortCount = _interfaceSortRepository.GetCountByCloudApplicationId(application),
                InterfaceCount = _interfaceSettingRepository.GetCountByCloudApplicationId(application),
                TriggerScriptCount = _triggerScriptRepository.GetCountByCloudApplicationId(application)
            };
        }

        public ChameleonStatistics GetUserAccountStatistics()
        {
            return new ChameleonStatistics
            {
                UserAccountAllCount = _userAccountRepository.GetAllUserCount(),
                UserAccountAdminisratorCount = _userAccountRepository.GetAdministratorCount(),
                ProfileCount = _profileRepository.GetUnDeletedCount(),
                MenuCount = _menuRepository.GetUnDeletedCount(),
                FunctionCount = _functionRepository.GetUnDeletedCount()
            };
        }
    }
}
