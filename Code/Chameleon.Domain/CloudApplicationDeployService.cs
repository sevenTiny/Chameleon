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
        CloudApplicationDeployDto CloudApplicationExport(Guid cloudApplicationId);
        /// <summary>
        /// 全应用导入
        /// </summary>
        /// <param name="cloudApplicationDeployDto"></param>
        /// <returns></returns>
        Result CloudApplicationImport(CloudApplicationDeployDto cloudApplicationDeployDto);
        /// <summary>
        /// 对象导出
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        CloudApplicationDeployDto MetaObjectExport(Guid metaObjectId);
        /// <summary>
        /// 应用下接口导出
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        CloudApplicationDeployDto DataSourceExport(Guid cloudApplicationId);
        /// <summary>
        /// 身份菜单功能
        /// </summary>
        /// <returns></returns>
        CloudApplicationDeployDto ProfileMenuFunc();
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
        IProfileRepository _profileRepository;
        IMenuRepository _menuRepository;
        IFunctionRepository _functionRepository;
        public CloudApplicationDeployService(
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

        public CloudApplicationDeployDto CloudApplicationExport(Guid cloudApplicationId)
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

        public CloudApplicationDeployDto ProfileMenuFunc()
        {
            var deployDto = new CloudApplicationDeployDto();
            deployDto.Profile = _profileRepository.GetListUnDeleted();
            deployDto.Menu = _menuRepository.GetListUnDeleted();
            deployDto.Function = _functionRepository.GetListUnDeleted();

            return deployDto;
        }

        public CloudApplicationDeployDto MetaObjectExport(Guid metaObjectId)
        {
            var deployDto = new CloudApplicationDeployDto();
            deployDto.MetaObject = new List<Entity.MetaObject> { _metaObjectRepository.GetById(metaObjectId) };
            deployDto.MetaField = _metaFieldRepository.GetListByMetaObjectId(metaObjectId);
            deployDto.InterfaceSetting = _interfaceSettingRepository.GetListByMetaObjectId(metaObjectId);
            deployDto.InterfaceFields = _interfaceFieldsRepository.GetListByMetaObjectId(metaObjectId);
            deployDto.InterfaceCondition = _interfaceConditionRepository.GetListByMetaObjectId(metaObjectId);
            deployDto.InterfaceSort = _interfaceSortRepository.GetListByMetaObjectId(metaObjectId);
            deployDto.InterfaceVerification = _interfaceVerificationRepository.GetListByMetaObjectId(metaObjectId);
            deployDto.TriggerScript = _triggerScriptRepository.GetMetaObjectTriggerListByMetaObjectId(metaObjectId);

            return deployDto;
        }

        public CloudApplicationDeployDto DataSourceExport(Guid cloudApplicationId)
        {
            var deployDto = new CloudApplicationDeployDto();
            deployDto.TriggerScript = _triggerScriptRepository.GetDataSourceListByApplicationId(cloudApplicationId, Entity.ScriptTypeEnum.DynamicScriptDataSourceTrigger);
            deployDto.TriggerScript.AddRange(_triggerScriptRepository.GetDataSourceListByApplicationId(cloudApplicationId, Entity.ScriptTypeEnum.JsonDataSource));
            deployDto.TriggerScript.AddRange(_triggerScriptRepository.GetDataSourceListByApplicationId(cloudApplicationId, Entity.ScriptTypeEnum.FileManagement));
            deployDto.InterfaceSetting = _interfaceSettingRepository.GetFileManagementListByCloudApplicationId(cloudApplicationId);
            return deployDto;
        }

        public Result CloudApplicationImport(CloudApplicationDeployDto deployDto)
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
            result.MoreMessage.Add($"--> 成功导入[{deployDto.CloudApplication?.Count ?? 0}]个应用");
            deployDto.CloudApplication?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _metaObjectRepository.BatchAdd(deployDto.MetaObject);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.MetaObject?.Count ?? 0}]个对象");
            deployDto.MetaObject?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _metaFieldRepository.BatchAdd(deployDto.MetaField);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.MetaField?.Count ?? 0}]个字段");
            deployDto.MetaField?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _triggerScriptRepository.BatchAdd(deployDto.TriggerScript);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.TriggerScript?.Count ?? 0}]个触发器");
            deployDto.TriggerScript?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _interfaceSettingRepository.BatchAdd(deployDto.InterfaceSetting);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.InterfaceSetting?.Count ?? 0}]个接口设置");
            deployDto.InterfaceSetting?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _interfaceFieldsRepository.BatchAdd(deployDto.InterfaceFields);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.InterfaceFields?.Count ?? 0}]个接口字段");
            deployDto.InterfaceFields?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _interfaceConditionRepository.BatchAdd(deployDto.InterfaceCondition);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.InterfaceCondition?.Count ?? 0}]个接口条件");
            deployDto.InterfaceCondition?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _interfaceSortRepository.BatchAdd(deployDto.InterfaceSort);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.InterfaceSort?.Count ?? 0}]个接口排序");
            deployDto.InterfaceSort?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _interfaceVerificationRepository.BatchAdd(deployDto.InterfaceVerification);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.InterfaceVerification?.Count ?? 0}]个接口校验");
            deployDto.InterfaceVerification?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _profileRepository.BatchAdd(deployDto.Profile);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.Profile?.Count ?? 0}]个身份");
            deployDto.Profile?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _menuRepository.BatchAdd(deployDto.Menu);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.Menu?.Count ?? 0}]个菜单");
            deployDto.Menu?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            _functionRepository.BatchAdd(deployDto.Function);
            result.MoreMessage.Add($"--> 成功导入[{deployDto.Function?.Count ?? 0}]个功能");
            deployDto.Function?.ForEach(item => result.MoreMessage.Add($"{item.Code}({item.Name})"));

            return result;
        }
    }
}
