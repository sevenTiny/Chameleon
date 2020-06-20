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

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchCloudDataController : ApiControllerBase
    {
        public BatchCloudDataController(IInterfaceConditionService interfaceConditionService, IInterfaceSettingRepository interfaceSettingRepository, IDataAccessApp dataAccessApp, IInterfaceVerificationService interfaceVerificationService, IInterfaceVerificationRepository interfaceVerificationRepository) : base(interfaceConditionService, interfaceSettingRepository, dataAccessApp, interfaceVerificationService, interfaceVerificationRepository)
        {
        }

        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JArray jArray)
        {
            return SafeExecute(() =>
            {
                if (jArray == null || !jArray.Any())
                    return Result.Error("Parameter invalid:jArray = null 业务数据为空，无法执行新增操作").ToJsonResult();

                List<BsonDocument> documents = jArray.Select(item => BsonDocument.Parse(item.ToString())).ToList();

                InitQueryContext(queryArgs);

                var result = _dataAccessApp.BatchAdd(_queryContext.InterfaceSetting, documents);

                return result.ToJsonResult();
            });
        }
    }
}