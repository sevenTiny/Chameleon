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
        public ApiControllerBase(IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository)
        {
            _interfaceConditionService = interfaceConditionService;
            _interfaceVerificationRepository = interfaceVerificationRepository;
            _interfaceVerificationService = interfaceVerificationService;
            _interfaceSettingRepository = interfaceSettingRepository;
            _dataAccessApp = dataAccessApp;
        }

        protected QueryContext _queryContext;

        /// <summary>
        /// 初始化查询上下文
        /// </summary>
        /// <param name="queryArgs"></param>
        protected void InitQueryContext(QueryArgs queryArgs)
        {
            _queryContext = new QueryContext();

            //查询接口
            _queryContext.InterfaceSetting = _interfaceSettingRepository.GetInterfaceSettingByIdWithVerify(queryArgs._interface);

            //argumentsDic generate
            foreach (var item in Request.Query)
                _queryContext.ConditionArguments.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);
        }

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

        protected FilterDefinition<BsonDocument> GetFilterDefinitionFromInterface()
        {
            //获取全部接口校验
            var verificationDic = _interfaceVerificationRepository.GetMetaFieldUpperKeyDicByInterfaceVerificationId(_queryContext.InterfaceSetting.InterfaceVerificationId);

            //校验条件是否满足校验
            foreach (var item in _queryContext.ConditionArguments)
            {
                if (verificationDic.ContainsKey(item.Key))
                {
                    var verification = verificationDic[item.Key];

                    if (!_interfaceVerificationService.IsMatch(verification, item.Value))
                        throw new ArgumentException(!string.IsNullOrEmpty(verification.VerificationTips) ? verification.VerificationTips : $"Key[{item.Key}]对应的值[{item.Value}]格式不正确");
                }
            }

            //构造条件
            FilterDefinition<BsonDocument> filter = _interfaceConditionService.GetFilterDefinitionByCondition(_queryContext.InterfaceSetting.InterfaceConditionId, _queryContext.ConditionArguments);

            return filter;
        }
    }
}
