using Chameleon.Repository;
using Chameleon.ValueObject;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface ICloudApplicationDeployService
    {
        /// <summary>
        /// 全应用导出
        /// </summary>
        /// <returns></returns>
        CloudApplicationDeployDto AllCloudApplicationExport(Guid cloudApplicationId);
        /// <summary>
        /// 全应用导入
        /// </summary>
        /// <param name="cloudApplicationDeployDto"></param>
        /// <returns></returns>
        Result AllCloudApplicationImport(CloudApplicationDeployDto cloudApplicationDeployDto);
    }

    public class CloudApplicationDeployService : ICloudApplicationDeployService
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
        public CloudApplicationDeployService(
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

        public CloudApplicationDeployDto AllCloudApplicationExport(Guid cloudApplicationId)
        {
            var deployDto = new CloudApplicationDeployDto();
            deployDto.CloudApplication = _cloudApplicationRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.MetaObject = _metaObjectRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.MetaField = _metaFieldRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.TriggerScript = _triggerScriptRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.InterfaceSetting = _interfaceSettingRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.InterfaceFields = _interfaceFieldsRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.InterfaceCondition = _interfaceConditionRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.InterfaceSort = _interfaceSortRepository.GetListByCloudApplicationId(cloudApplicationId);
            deployDto.InterfaceVerification = _interfaceVerificationRepository.GetListByCloudApplicationId(cloudApplicationId);

            return deployDto;
        }

        public Result AllCloudApplicationImport(CloudApplicationDeployDto deployDto)
        {
            if (deployDto == null)
                return Result.Success("Success，未找到需要导入的元数据");

            //清理环境已有的数据
            _cloudApplicationRepository.BatchDelete(deployDto.CloudApplication?.Select(t => t.Id));
            _metaObjectRepository.BatchDelete(deployDto.MetaObject?.Select(t => t.Id));
            _metaFieldRepository.BatchDelete(deployDto.MetaField?.Select(t => t.Id));
            _triggerScriptRepository.BatchDelete(deployDto.TriggerScript?.Select(t => t.Id));
            _interfaceSettingRepository.BatchDelete(deployDto.InterfaceSetting?.Select(t => t.Id));
            _interfaceFieldsRepository.BatchDelete(deployDto.InterfaceFields?.Select(t => t.Id));
            _interfaceConditionRepository.BatchDelete(deployDto.InterfaceCondition?.Select(t => t.Id));
            _interfaceSortRepository.BatchDelete(deployDto.InterfaceSort?.Select(t => t.Id));
            _interfaceVerificationRepository.BatchDelete(deployDto.InterfaceVerification?.Select(t => t.Id));

            var result = Result.Success("导入完成");
            result.MoreMessage = new List<string>();

            //新增数据
            _cloudApplicationRepository.BatchAdd(deployDto.CloudApplication);
            result.MoreMessage.Add($"成功导入[{deployDto.CloudApplication?.Count ?? 0}]个应用");

            _metaObjectRepository.BatchAdd(deployDto.MetaObject);
            result.MoreMessage.Add($"成功导入[{deployDto.MetaObject?.Count ?? 0}]个对象");

            _metaFieldRepository.BatchAdd(deployDto.MetaField);
            result.MoreMessage.Add($"成功导入[{deployDto.MetaField?.Count ?? 0}]个字段");

            _triggerScriptRepository.BatchAdd(deployDto.TriggerScript);
            result.MoreMessage.Add($"成功导入[{deployDto.TriggerScript?.Count ?? 0}]个触发器脚本");

            _interfaceSettingRepository.BatchAdd(deployDto.InterfaceSetting);
            result.MoreMessage.Add($"成功导入[{deployDto.InterfaceSetting?.Count ?? 0}]个接口设置");

            _interfaceFieldsRepository.BatchAdd(deployDto.InterfaceFields);
            result.MoreMessage.Add($"成功导入[{deployDto.InterfaceFields?.Count ?? 0}]个接口字段");

            _interfaceConditionRepository.BatchAdd(deployDto.InterfaceCondition);
            result.MoreMessage.Add($"成功导入[{deployDto.InterfaceCondition?.Count ?? 0}]个接口条件");

            _interfaceSortRepository.BatchAdd(deployDto.InterfaceSort);
            result.MoreMessage.Add($"成功导入[{deployDto.InterfaceSort?.Count ?? 0}]个接口排序");

            _interfaceVerificationRepository.BatchAdd(deployDto.InterfaceVerification);
            result.MoreMessage.Add($"成功导入[{deployDto.InterfaceVerification?.Count ?? 0}]个接口校验");

            return result;
        }
    }
}
