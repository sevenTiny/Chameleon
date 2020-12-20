﻿using Chameleon.Application;
using Chameleon.DataApi.Models;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Chameleon.DataApi.Controllers
{
    /// <summary>
    /// 批量操作api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BatchCloudDataController : CloudDataApiControllerBase
    {
        public BatchCloudDataController(
            IOrganizationService organizationService,
            ITriggerScriptService triggerScriptService,
            ITriggerScriptRepository triggerScriptRepository,
            IInterfaceConditionService interfaceConditionService,
            IInterfaceSettingRepository interfaceSettingRepository,
            IDataAccessApp dataAccessApp,
            IInterfaceVerificationService interfaceVerificationService,
            IInterfaceVerificationRepository interfaceVerificationRepository) :
            base(
                organizationService,
                triggerScriptService,
                triggerScriptRepository,
                interfaceConditionService,
                interfaceSettingRepository,
                dataAccessApp,
                interfaceVerificationService,
                interfaceVerificationRepository
                )
        {
        }

        /// <summary>
        /// 批量新增操作
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <param name="args">json数组</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JArray args)
        {
            return SafeExecute(() =>
            {
                var documents = args.Select(item => BsonDocument.Parse(item.ToString())).ToArray();

                if (documents == null || !documents.Any())
                    return Result.Error("Parameter invalid: 业务数据为空，无法执行新增操作").ToJsonResult();

                InitQueryContext(queryArgs);

                //校验接口类型是否匹配
                if (_queryContext.InterfaceSetting.GetInterfaceType() != InterfaceTypeEnum.BatchAdd)
                    return Result.Error("该接口不适用于该接口编码对应的接口类型").ToJsonResult();

                //before
                var arg = TryExecuteTriggerByServiceType(InterfaceServiceTypeEnum.MetaObject_BatchAdd_Before, new object[] { _queryContext.TriggerContext, documents }, documents);

                //系统字段赋值
                if (arg != null && arg.Any())
                {
                    foreach (var item in arg)
                    {
                        item["Organization"] = CurrentOrganization.ToString();
                        item["CreateBy"] = CurrentUserId;
                        item["ModifyBy"] = CurrentUserId;
                    }
                }

                //execute
                var result = _dataAccessApp.BatchAdd(_queryContext.InterfaceSetting, arg);
                //after
                TryExecuteTriggerByServiceType<object>(InterfaceServiceTypeEnum.MetaObject_BatchAdd_After, new object[] { _queryContext.TriggerContext }, null);

                return result.ToJsonResult();
            });
        }
    }
}