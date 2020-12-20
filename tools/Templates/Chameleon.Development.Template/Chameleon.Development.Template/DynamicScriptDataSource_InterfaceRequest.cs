﻿//请不要修改脚本模板中的默认类名和方法名
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using SevenTiny.Bantina;
using Chameleon.Common;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

public class DynamicScriptDataSource_InterfaceRequest
{
    //记录日志请使用：logger.LogError("error log.");  logger.LogDebug("debug log.")
    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();

    public object Get(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic)
    {
        var request = new InterfaceRequest(triggerContext);

        var args = new Dictionary<string, string>();
        args.Add("MessageTitle", "您收到了一条消息");
        args.Add("MessageContent", "今天晚上红旗广场不见不散哦");
        args.Add("UserId", triggerContext["CurrentUserId"]);
        args.Add("HasRead", "false");

        var result = request.CloudDataPost("ChameleonMessage.MessageAlert.SendMessage", args);

        return result;
    }
}