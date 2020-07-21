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


public class MetaObjectInterfaceTrigger
{

    //1. 记录日志
    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();
    /* 
        logger.LogDebug(string message, params object[] args);
        logger.LogError(string message, params object[] args);
        logger.LogInformation(string message, params object[] args);
    */
    //2. 查询数据
    //MongoDb数据库查询上下文，需要时放开下行注释使用
    //ChameleonDataDbContext dbContext = new ChameleonDataDbContext();
    /* 
     *  查询数据的模板
        var bf = Builders<BsonDocument>.Filter;
        var filter = bf.And(bf.Eq("key","value"),bf.Eq("key2","value2"));
        dbContext.GetCollectionBson("对象编码").Find(filter);
    */

    public List<Dictionary<string, CloudData>> QueryList_After(Dictionary<string, string> triggerContext, List<Dictionary<string, CloudData>> result)
    {
        if (triggerContext["Interface"] == "xxx")
        {

        }
        return result;
    }
}
