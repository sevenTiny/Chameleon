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
using System.Linq;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudDataController : ApiControllerBase
    {
        public CloudDataController(IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository) : base(interfaceConditionService, interfaceSettingRepository, dataAccessApp, interfaceVerificationService, interfaceVerificationRepository)
        {
        }

        [HttpGet]
        public IActionResult Get([FromQuery]QueryArgs queryArgs)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                //查询条件
                FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;

                switch (_queryContext.InterfaceSetting.GetInterfaceType())
                {
                    case InterfaceTypeEnum.QueryCount:
                        filter = GetFilterDefinitionFromInterface();
                        var countResult = _dataAccessApp.GetCount(_queryContext.InterfaceSetting, filter);
                        return countResult.ToJsonResult();
                    case InterfaceTypeEnum.QuerySingle:
                        filter = GetFilterDefinitionFromInterface();
                        var singleResult = _dataAccessApp.Get(_queryContext.InterfaceSetting, filter);
                        return singleResult.ToJsonResult();
                    case InterfaceTypeEnum.QueryList:
                        filter = GetFilterDefinitionFromInterface();
                        var listResult = _dataAccessApp.GetList(_queryContext.InterfaceSetting, filter);
                        return listResult.ToJsonResult();
                    case InterfaceTypeEnum.DataSource:
                        break;
                    case InterfaceTypeEnum.UnKnown:
                    case InterfaceTypeEnum.Add:
                    case InterfaceTypeEnum.BatchAdd:
                    case InterfaceTypeEnum.Update:
                    case InterfaceTypeEnum.Delete:
                    default:
                        return Result.Error("该接口不适用于该接口编码对应的接口类型").ToJsonResult();
                }

                return Result.Success("success, but no data found").ToJsonResult();
            });
        }

        /**
         Content-Type: application/json
         * */
        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JObject jObj)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                var bson = BsonDocument.Parse(jObj.ToString());

                if (bson == null || !bson.Any())
                    return Result.Error("Parameter invalid:jObj = null 业务数据为空，无法执行新增操作").ToJsonResult();

                var result = _dataAccessApp.BatchAdd(_queryContext.InterfaceSetting, new[] { bson });

                return result.ToJsonResult();
            });
        }

        /**
         Content-Type: application/json
         * */
        [HttpPut]
        public IActionResult Update([FromQuery]QueryArgs queryArgs, [FromBody]JObject jObj)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                var bson = BsonDocument.Parse(jObj.ToString());

                if (bson == null || !bson.Any())
                    return Result.Error("Parameter invalid:jObj = null 业务数据为空，无法执行更新操作").ToJsonResult();

                var filter = GetFilterDefinitionFromInterface();

                var result = _dataAccessApp.BatchUpdate(_queryContext.InterfaceSetting, filter, bson);

                return result.ToJsonResult();
            });
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery]QueryArgs queryArgs)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                var filter = GetFilterDefinitionFromInterface();

                var result = _dataAccessApp.Delete(_queryContext.InterfaceSetting, filter);

                return result.ToJsonResult();
            });
        }
    }
}