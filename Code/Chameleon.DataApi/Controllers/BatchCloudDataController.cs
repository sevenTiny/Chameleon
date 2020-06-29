using Chameleon.Application;
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
    public class BatchCloudDataController : ApiControllerBase
    {
        public BatchCloudDataController(ITriggerScriptService triggerScriptService, ITriggerScriptRepository triggerScriptRepository, IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository) : base(triggerScriptService, triggerScriptRepository, interfaceConditionService, interfaceSettingRepository, dataAccessApp, interfaceVerificationService, interfaceVerificationRepository)
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

                //before
                var arg = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.BatchAdd_Before, new object[] { _queryContext.TriggerContext, documents }, documents);
                //execute
                var result = _dataAccessApp.BatchAdd(_queryContext.InterfaceSetting, arg);
                //after
                TryExecuteTriggerByServiceType<object>(MetaObjectInterfaceServiceTypeEnum.BatchAdd_After, new object[] { _queryContext.TriggerContext }, null);

                return result.ToJsonResult();
            });
        }
    }
}