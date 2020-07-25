//请不要修改脚本模板中的默认类名和方法名
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using SevenTiny.Bantina;
using Chameleon.Common;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

public class DynamicScriptDataSource_DataOperation
{
    //记录日志请使用：logger.LogError("error log.");  logger.LogDebug("debug log.")
    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();

    public object Get(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic)
    {
        ChameleonDataDbContext dbContext = new ChameleonDataDbContext();

        var bf = Builders<BsonDocument>.Filter;

        var filter = bf.Eq("IsDeleted", false);

        var re = dbContext.GetCollectionBson("ChameleonDemo.UserInformation").Find(filter).ToList();

        return new { Name = "xxx", Age = 200 };
    }
}
