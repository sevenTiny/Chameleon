using Chameleon.Application;
using Chameleon.DataApi.Models;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System.Linq;
using System.Text.Json;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudDataController : ApiControllerBase
    {
        public CloudDataController(ITriggerScriptService triggerScriptService, ITriggerScriptRepository triggerScriptRepository, IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository) : base(triggerScriptService, triggerScriptRepository, interfaceConditionService, interfaceSettingRepository, dataAccessApp, interfaceVerificationService, interfaceVerificationRepository)
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
                        //before
                        filter = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.QueryCount_Before, new object[] { _queryContext.TriggerContext, _queryContext.ConditionArgumentsUpperKeyDic, filter }, filter);
                        //execute
                        var countResult = _dataAccessApp.GetCount(_queryContext.InterfaceSetting, filter);
                        //after
                        countResult = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.QueryCount_After, new object[] { _queryContext.TriggerContext, countResult }, countResult);
                        return countResult.ToJsonResult();
                    //查询单条记录
                    case InterfaceTypeEnum.QuerySingle:
                        filter = GetFilterDefinitionFromInterface();
                        //before
                        filter = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.QuerySingle_Before, new object[] { _queryContext.TriggerContext, _queryContext.ConditionArgumentsUpperKeyDic, filter }, filter);
                        //execute
                        var singleResult = _dataAccessApp.Get(_queryContext.InterfaceSetting, filter);
                        //after
                        singleResult = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.QuerySingle_After, new object[] { _queryContext.TriggerContext, singleResult }, singleResult);
                        return singleResult.ToJsonResult();
                    //查询集合
                    case InterfaceTypeEnum.QueryList:
                        filter = GetFilterDefinitionFromInterface();
                        //before
                        filter = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.QueryList_Before, new object[] { _queryContext.TriggerContext, _queryContext.ConditionArgumentsUpperKeyDic, filter }, filter);
                        //execute
                        var listResult = _dataAccessApp.GetList(_queryContext.InterfaceSetting, filter);
                        //after
                        listResult = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.QueryList_After, new object[] { _queryContext.TriggerContext, listResult }, listResult);
                        return listResult.ToJsonResult();
                    //查询动态数据源
                    case InterfaceTypeEnum.DynamicScriptDataSource:
                        return _dataAccessApp.GetDynamicScriptDataSourceResult(_queryContext.InterfaceSetting, _queryContext.ConditionArgumentsUpperKeyDic).ToJsonResult();
                    //查询Json数据源
                    case InterfaceTypeEnum.JsonDataSource:
                        return new JsonResult(_dataAccessApp.GetJsonDataSourceResult(_queryContext.InterfaceSetting));
                    case InterfaceTypeEnum.UnKnown:
                    case InterfaceTypeEnum.Add:
                    case InterfaceTypeEnum.BatchAdd:
                    case InterfaceTypeEnum.Update:
                    case InterfaceTypeEnum.Delete:
                    default:
                        return Result.Error("该接口不适用于该接口编码对应的接口类型").ToJsonResult();
                }
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

                //before
                var args = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.Add_Before, new object[] { _queryContext.TriggerContext, new[] { bson } }, new[] { bson });
                //execute
                var result = _dataAccessApp.BatchAdd(_queryContext.InterfaceSetting, args);
                //after
                result = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.Add_After, new object[] { _queryContext.TriggerContext, result }, result);

                return result.ToJsonResult();
            });
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <param name="arg"></param>
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

                //before
                filter = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.Update_Before, new object[] { _queryContext.TriggerContext, _queryContext.ConditionArgumentsUpperKeyDic, filter }, filter);
                //execute
                var result = _dataAccessApp.BatchUpdate(_queryContext.InterfaceSetting, filter, bson);
                //after
                result = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.Update_After, new object[] { _queryContext.TriggerContext, result }, result);

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

                //before
                filter = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.Delete_Before, new object[] { _queryContext.TriggerContext, _queryContext.ConditionArgumentsUpperKeyDic, filter }, filter);
                //execute
                var result = _dataAccessApp.Delete(_queryContext.InterfaceSetting, filter);
                //after
                result = TryExecuteTriggerByServiceType(MetaObjectInterfaceServiceTypeEnum.Delete_After, new object[] { _queryContext.TriggerContext, result }, result);

                return result.ToJsonResult();
            });
        }
    }
}