using Chameleon.Application;
using Chameleon.DataApi.Models;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chameleon.DataApi.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected IDataAccessApp _dataAccessApp;
        protected IInterfaceSettingRepository _interfaceSettingRepository;
        protected IInterfaceVerificationRepository _interfaceVerificationRepository;
        protected IInterfaceVerificationService _interfaceVerificationService;
        protected IInterfaceConditionService _interfaceConditionService;
        protected ITriggerScriptRepository _triggerScriptRepository;
        protected ITriggerScriptService _triggerScriptService;
        public ApiControllerBase(ITriggerScriptService triggerScriptService, ITriggerScriptRepository triggerScriptRepository, IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository)
        {
            _triggerScriptService = triggerScriptService;
            _triggerScriptRepository = triggerScriptRepository;
            _interfaceConditionService = interfaceConditionService;
            _interfaceVerificationRepository = interfaceVerificationRepository;
            _interfaceVerificationService = interfaceVerificationService;
            _interfaceSettingRepository = interfaceSettingRepository;
            _dataAccessApp = dataAccessApp;
        }

        protected QueryContext _queryContext;

        /// <summary>
        /// 返回安全执行结果
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected IActionResult SafeExecute(Func<IActionResult> func)
        {
            try
            {
                return func();
            }
            catch (ArgumentNullException argNullEx)
            {
                return Result.Error(argNullEx.Message).ToJsonResult();
            }
            catch (ArgumentException argEx)
            {
                return Result.Error(argEx.Message).ToJsonResult();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message).ToJsonResult();
            }
        }

        /// <summary>
        /// 初始化查询上下文
        /// </summary>
        /// <param name="queryArgs"></param>
        protected void InitQueryContext(QueryArgs queryArgs)
        {
            _queryContext = new QueryContext();

            //查询接口
            _queryContext.InterfaceSetting = _interfaceSettingRepository.GetInterfaceSettingByCodeWithVerify(queryArgs._interface);

            //argumentsDic generate
            foreach (var item in Request.Query)
                _queryContext.ConditionArgumentsUpperKeyDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

            //初始化触发器
            switch (_queryContext.InterfaceSetting.GetInterfaceType())
            {
                case InterfaceTypeEnum.Add:
                case InterfaceTypeEnum.BatchAdd:
                case InterfaceTypeEnum.Update:
                case InterfaceTypeEnum.Delete:
                case InterfaceTypeEnum.QueryCount:
                case InterfaceTypeEnum.QuerySingle:
                case InterfaceTypeEnum.QueryList:
                //查询对象接口
                    _queryContext.TriggerScripts = _triggerScriptRepository.GetMetaObjectTriggerListByMetaObjectId(_queryContext.InterfaceSetting.MetaObjectId);
                    break;
                //查询动态脚本接口
                case InterfaceTypeEnum.DynamicScriptDataSource:
                    _queryContext.TriggerScripts = new List<TriggerScript> { _triggerScriptRepository.GetById(_queryContext.InterfaceSetting.CloudApplicationtId) };
                    break;
                case InterfaceTypeEnum.UnKnown:
                default:
                    break;
            }
        }

        /// <summary>
        /// 尝试执行服务对应的动态脚本，如果没有，则采用默认值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="metaObjectInterfaceServiceTypeEnum"></param>
        /// <param name="parameters"></param>
        /// <param name="ifNoScriptReturnDefaultValue">默认值</param>
        /// <returns></returns>
        protected TResult TryExecuteTriggerByServiceType<TResult>(MetaObjectInterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum, object[] parameters, TResult ifNoScriptReturnDefaultValue)
        {
            //拿到当前服务类型对应的脚本
            var triggerScript = _queryContext.TriggerScripts?.FirstOrDefault(t => t.MetaObjectInterfaceServiceType == (int)metaObjectInterfaceServiceTypeEnum);

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
            foreach (var item in _queryContext.ConditionArgumentsUpperKeyDic)
            {
                if (verificationDic.ContainsKey(item.Key))
                {
                    var verification = verificationDic[item.Key];

                    if (!_interfaceVerificationService.IsMatch(verification, item.Value))
                        throw new ArgumentException(!string.IsNullOrEmpty(verification.VerificationTips) ? verification.VerificationTips : $"Key[{item.Key}]对应的值[{item.Value}]格式不正确");
                }
            }

            //构造条件
            return _interfaceConditionService.GetFilterDefinitionByCondition(_queryContext.InterfaceSetting.InterfaceConditionId, _queryContext.ConditionArgumentsUpperKeyDic);
        }
    }
}
