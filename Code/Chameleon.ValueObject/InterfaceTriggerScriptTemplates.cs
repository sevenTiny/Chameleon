namespace Chameleon.ValueObject
{
    /// <summary>
    /// 对象接口触发器脚本
    /// </summary>
    public abstract class DefaultScriptBase
    {
        public abstract string ClassFullName { get; }
        public abstract string FunctionName { get; }
        public abstract string Script { get; }

        /// <summary>
        /// 公共引用部分
        /// </summary>
        protected string CommonUsing =>
@"//请不要修改脚本模板中的默认类名和方法名
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using SevenTiny.Bantina;
using Chameleon.Common;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
";
        /// <summary>
        /// 公共的类内的代码
        /// </summary>
        protected string CommonClassCode =>
@"  //记录日志请使用：logger.LogError(""error log."");  logger.LogDebug(""debug log."")
    ILogger logger = new SevenTiny.Bantina.Logging.LogManager();
";
    }

    #region MetaObjectInterface
    public class MetaObjectInterface_Add_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Add_Before";

        public override string Script =>
$@"{CommonUsing}
public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public BsonDocument Add_Before(Dictionary<string, string> triggerContext, BsonDocument bsonDocument)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return bsonDocument;
    }}
}}
";
    }
    public class MetaObjectInterface_Add_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Add_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public void Add_After(Dictionary<string, string> triggerContext)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
    }}
}}
";
    }
    public class MetaObjectInterface_BatchAdd_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "BatchAdd_Before";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public BsonDocument[] BatchAdd_Before(Dictionary<string, string> triggerContext, BsonDocument[] bsonDocuments)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return bsonDocuments;
    }}
}}
";
    }
    public class MetaObjectInterface_BatchAdd_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "BatchAdd_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public void BatchAdd_After(Dictionary<string, string> triggerContext)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
    }}
}}
";
    }
    public class MetaObjectInterface_Update_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Update_Before";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> Update_Before(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter, BsonDocument bsonDocument)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return filter;
    }}
}}
";
    }
    public class MetaObjectInterface_Update_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Update_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result Update_After(Dictionary<string, string> triggerContext)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
    }}
}}
";
    }
    public class MetaObjectInterface_Delete_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Delete_Before";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> Delete_Before(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return filter;
    }}
}}
";
    }
    public class MetaObjectInterface_Delete_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Delete_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result Delete_After(Dictionary<string, string> triggerContext)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
    }}
}}
";
    }
    public class MetaObjectInterface_QueryCount_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QueryCount_Before";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QueryCount_Before(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return filter;
    }}
}}
";
    }
    public class MetaObjectInterface_QueryCount_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QueryCount_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public int QueryCount_After(Dictionary<string, string> triggerContext, int result)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return result;
    }}
}}
";
    }
    public class MetaObjectInterface_QuerySingle_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QuerySingle_Before";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QuerySingle_Before(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return filter;
    }}
}}
";
    }
    public class MetaObjectInterface_QuerySingle_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QuerySingle_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Dictionary<string, CloudData> QuerySingle_After(Dictionary<string, string> triggerContext, Dictionary<string, CloudData> result)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return result;
    }}
}}
";

    }
    public class MetaObjectInterface_QueryList_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QueryList_Before";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QueryList_Before(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return filter;
    }}
}}
";
    }
    public class MetaObjectInterface_QueryList_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QueryList_After";

        public override string Script =>
$@"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public List<Dictionary<string, CloudData>> QueryList_After(Dictionary<string, string> triggerContext, List<Dictionary<string, CloudData>> result)
    {{
        if(triggerContext[""Interface""] == ""xxx"")
        {{

        }}
        return result;
    }}
}}
";
    }
    #endregion

    #region CloudApplicationInterface
    public class DynamicScriptDataSourceScript : DefaultScriptBase
    {
        public override string ClassFullName => "DynamicScriptDataSource";

        public override string FunctionName => "Get";

        public override string Script =>
$@"{CommonUsing}

public class DynamicScriptDataSource
{{
    {CommonClassCode}
    public object Get(Dictionary<string, string> triggerContext, Dictionary<string, string> argumentsUpperKeyDic)
    {{
        return null;
    }}
}}
";
    }

    public class FileManagementScript_UploadFile_Before : DefaultScriptBase
    {
        public override string ClassFullName => "FileManagementScript";

        public override string FunctionName => "UploadFile_Before";

        public override string Script =>
$@"{CommonUsing}

public class FileManagementScript
{{
    {CommonClassCode}
    public FileUploadPayload UploadFile_Before(Dictionary<string, string> triggerContext, FileUploadPayload fileUploadPayload)
    {{
        return fileUploadPayload;
    }}
}}
";
    }

    public class FileManagementScript_UploadFile_After : DefaultScriptBase
    {
        public override string ClassFullName => "FileManagementScript";

        public override string FunctionName => "UploadFile_After";

        public override string Script =>
$@"{CommonUsing}

public class FileManagementScript
{{
    {CommonClassCode}
    public void UploadFile_After(Dictionary<string, string> triggerContext, FileUploadPayload fileUploadPayload)
    {{
        
    }}
}}
";
    }
    public class FileManagementScript_DownloadFile_Before : DefaultScriptBase
    {
        public override string ClassFullName => "FileManagementScript";

        public override string FunctionName => "DownloadFile_Before";

        public override string Script =>
$@"{CommonUsing}

public class FileManagementScript
{{
    {CommonClassCode}
    public void DownloadFile_Before(Dictionary<string, string> triggerContext)
    {{
        
    }}
}}
";
    }
    public class FileManagementScript_DownloadFile_After : DefaultScriptBase
    {
        public override string ClassFullName => "FileManagementScript";

        public override string FunctionName => "DownloadFile_After";

        public override string Script =>
$@"{CommonUsing}

public class FileManagementScript
{{
    {CommonClassCode}
    public FileDownloadPayload DownloadFile_After(Dictionary<string, string> triggerContext, FileDownloadPayload fileDownloadPayload)
    {{
        return fileDownloadPayload;
    }}
}}
";
    }
    #endregion


}
