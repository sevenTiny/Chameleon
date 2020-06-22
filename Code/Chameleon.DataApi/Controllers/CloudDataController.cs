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
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudDataController : ApiControllerBase
    {
        public CloudDataController(IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository) : base(interfaceConditionService, interfaceSettingRepository, dataAccessApp, interfaceVerificationService, interfaceVerificationRepository)
        {
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
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
                    //查询数量
                    case InterfaceTypeEnum.QueryCount:
                        filter = GetFilterDefinitionFromInterface();
                        var countResult = _dataAccessApp.GetCount(_queryContext.InterfaceSetting, filter);
                        return countResult.ToJsonResult();
                    //查询单条记录
                    case InterfaceTypeEnum.QuerySingle:
                        filter = GetFilterDefinitionFromInterface();
                        var singleResult = _dataAccessApp.Get(_queryContext.InterfaceSetting, filter);
                        return singleResult.ToJsonResult();
                    //查询集合
                    case InterfaceTypeEnum.QueryList:
                        filter = GetFilterDefinitionFromInterface();
                        var listResult = _dataAccessApp.GetList(_queryContext.InterfaceSetting, filter);
                        return listResult.ToJsonResult();
                    //查询数据源
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

        /// <summary>
        /// 新增单条数据
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <param name="arg">数据json</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JsonElement arg)
        {
            return SafeExecute(() =>
            {
                var bson = BsonDocument.Parse(arg.ToString());

                if (bson == null || !bson.Any())
                    return Result.Error("Parameter invalid: 业务数据为空，无法执行新增操作").ToJsonResult();

                InitQueryContext(queryArgs);

                var result = _dataAccessApp.BatchAdd(_queryContext.InterfaceSetting, new[] { bson });

                return result.ToJsonResult();
            });
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <param name="jObj"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update([FromQuery]QueryArgs queryArgs, [FromBody]JsonElement arg)
        {
            return SafeExecute(() =>
            {
                var bson = BsonDocument.Parse(arg.ToString());

                if (bson == null || !bson.Any())
                    return Result.Error("Parameter invalid: 业务数据为空，无法执行更新操作").ToJsonResult();

                InitQueryContext(queryArgs);

                var filter = GetFilterDefinitionFromInterface();

                var result = _dataAccessApp.BatchUpdate(_queryContext.InterfaceSetting, filter, bson);

                return result.ToJsonResult();
            });
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
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