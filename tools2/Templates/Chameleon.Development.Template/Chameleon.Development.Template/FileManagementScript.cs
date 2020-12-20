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


public class FileManagementScript
{
    //记录日志请使用：logger.LogError("error log.");  logger.LogDebug("debug log.")
    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();

    public FileUploadPayload UploadFile_Before(Dictionary<string, string> triggerContext, FileUploadPayload fileUploadPayload)
    {
        return fileUploadPayload;
    }
}
