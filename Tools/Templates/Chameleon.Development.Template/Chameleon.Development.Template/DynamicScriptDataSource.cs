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


public class DynamicScriptDataSource
{
    //1. 记录日志
    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();
    //logger.LogError(string message, params object[] args);

    public object Get(Dictionary<string, string> argumentsUpperKeyDic)
    {
        ChameleonDataDbContext dbContext = new ChameleonDataDbContext();

        var bf = Builders<BsonDocument>.Filter;

        var filter = bf.Eq("IsDeleted", false);

        var re = dbContext.GetCollectionBson("ChameleonDemo.UserInformation").Find(filter).ToList() ;

        return new { Name = "蒋鹏", Age = 200 };
    }
}
