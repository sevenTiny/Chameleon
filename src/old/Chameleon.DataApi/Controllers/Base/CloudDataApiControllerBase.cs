using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure.Configs;
using Chameleon.Infrastructure.Consts;
using Chameleon.Repository;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Chameleon.DataApi.Controllers
{
    [Authorize]
    public class CloudDataApiControllerBase : InterfaceApiControllerBase
    {
        protected IDataAccessApp _dataAccessApp;
        protected IInterfaceVerificationRepository _interfaceVerificationRepository;
        protected IInterfaceVerificationService _interfaceVerificationService;
        protected IInterfaceConditionService _interfaceConditionService;
        protected ITriggerScriptService _triggerScriptService;
        protected IOrganizationService _organizationService;
        public CloudDataApiControllerBase(IOrganizationService organizationService, ITriggerScriptService triggerScriptService, ITriggerScriptRepository triggerScriptRepository, IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository)
        : base(
             triggerScriptRepository,
             interfaceSettingRepository
             )
        {
            _organizationService = organizationService;
            _triggerScriptService = triggerScriptService;
            _interfaceConditionService = interfaceConditionService;
            _interfaceVerificationRepository = interfaceVerificationRepository;
            _interfaceVerificationService = interfaceVerificationService;
            _dataAccessApp = dataAccessApp;
        }

        /// <summary>
        /// 尝试执行服务对应的动态脚本，如果没有，则采用默认值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="metaObjectInterfaceServiceTypeEnum"></param>
        /// <param name="parameters"></param>
        /// <param name="ifNoScriptReturnDefaultValue">默认值</param>
        /// <returns></returns>
        protected TResult TryExecuteTriggerByServiceType<TResult>(InterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum, object[] parameters, TResult ifNoScriptReturnDefaultValue)
        {
            //拿到当前服务类型对应的脚本
            var triggerScript = _queryContext.TriggerScripts?.FirstOrDefault(t => t.InterfaceServiceType == (int)metaObjectInterfaceServiceTypeEnum);

            if (triggerScript == null)
                return ifNoScriptReturnDefaultValue;

            //执行脚本
            var executeResult = _triggerScriptService.ExecuteTriggerScript<TResult>(triggerScript, parameters);

            if (!executeResult.IsSuccess)
                throw new InvalidOperationException(executeResult.Message);

            return executeResult.Data;
        }

        protected FilterDefinition<BsonDocument> GetFilterDefinitionFromInterface()
        {
            //获取全部接口校验
            var verificationDic = _interfaceVerificationRepository.GetMetaFieldUpperKeyDicByInterfaceVerificationId(_queryContext.InterfaceSetting.InterfaceVerificationId);

            //校验条件是否满足校验
            foreach (var item in verificationDic)
            {
                if (_queryContext.ConditionArgumentsUpperKeyDic.ContainsKey(item.Key))
                {
                    var arg = _queryContext.ConditionArgumentsUpperKeyDic[item.Key];

                    if (!_interfaceVerificationService.IsMatch(item.Value, arg))
                        throw new ArgumentException(!string.IsNullOrEmpty(item.Value.VerificationTips) ? item.Value.VerificationTips : $"Key[{item.Key}]对应的值[{arg}]格式不正确");
                }
            }

            //构造条件
            var filter = _interfaceConditionService.GetFilterDefinitionByCondition(_queryContext.InterfaceSetting.InterfaceConditionId, _queryContext.ConditionArgumentsUpperKeyDic);

            //如果开启Mou功能，则拼接MOU权限，有权限的组织下的数据才能被查到
            if (ChameleonSettingConfig.Instance.MouEnable == 1)
                filter = Builders<BsonDocument>.Filter.And(filter, Builders<BsonDocument>.Filter.In(AccountConst.KEY_Organization, _organizationService.GetSubordinatOrganizations(CurrentOrganization)));

            return filter;
        }
    }
}
