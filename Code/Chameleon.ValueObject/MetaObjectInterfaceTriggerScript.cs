using System;
using System.Collections.Generic;
using System.Text;

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
using Chameleon.ValueObject;
";
        /// <summary>
        /// 公共的类内的代码
        /// </summary>
        protected string CommonClassCode =>
@"";
    }
    public class MetaObjectInterface_Add_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Add_Before";

        public override string Script =>
@$"{CommonUsing}
public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public BsonDocument[] Add_Before(Dictionary<string, object> triggerContext, BsonDocument[] bsonDocuments)
    {{
        return bsonDocuments;
    }}
}}
";
    }
    public class MetaObjectInterface_Add_After : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Add_After";

        public override string Script =>
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result Delete_After(Dictionary<string, object> triggerContext, Result result)
    {{
        return result;
    }}
}}
";
    }
    public class MetaObjectInterface_BatchAdd_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "BatchAdd_Before";

        public override string Script =>
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public BsonDocument[] Add_Before(Dictionary<string, object> triggerContext, BsonDocument[] bsonDocuments)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result Delete_After(Dictionary<string, object> triggerContext, Result result)
    {{
        return result;
    }}
}}
";
    }
    public class MetaObjectInterface_Update_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Update_Before";

        public override string Script =>
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QueryCount_Before(Dictionary<string, object> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result Delete_After(Dictionary<string, object> triggerContext, Result result)
    {{
        return result;
    }}
}}
";
    }
    public class MetaObjectInterface_Delete_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "Delete_Before";

        public override string Script =>
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QueryCount_Before(Dictionary<string, object> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result Delete_After(Dictionary<string, object> triggerContext, Result result)
    {{
        return result;
    }}
}}
";
    }
    public class MetaObjectInterface_QueryCount_Before : DefaultScriptBase
    {
        public override string ClassFullName => "MetaObjectInterfaceTrigger";

        public override string FunctionName => "QueryCount_Before";

        public override string Script =>
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QueryCount_Before(Dictionary<string, object> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result<int> QueryCount_After(Dictionary<string, object> triggerContext, Result<int> result)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QuerySingle_Before(Dictionary<string, object> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public Result<Dictionary<string, CloudData>> QuerySingle_After(Dictionary<string, object> triggerContext, Result<Dictionary<string, CloudData>> result)
    {{
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
@$"{CommonUsing}

public class MetaObjectInterfaceTrigger
{{
    {CommonClassCode}
    public FilterDefinition<BsonDocument> QueryList_Before(Dictionary<string, object> triggerContext, Dictionary<string, string> argumentsUpperKeyDic, FilterDefinition<BsonDocument> filter)
    {{
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
@"using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using SevenTiny.Bantina;
using Chameleon.ValueObject;

public class MetaObjectInterfaceTrigger
{
    public Result<List<Dictionary<string, CloudData>>> QueryList_After(Dictionary<string, object> triggerContext, Result<List<Dictionary<string, CloudData>>> result)
    {
        return result;
    }
}
";
    }
    public class DynamicScriptInterfaceScript : DefaultScriptBase
    {
        public override string ClassFullName => throw new NotImplementedException();

        public override string FunctionName => throw new NotImplementedException();

        public override string Script => throw new NotImplementedException();
    }
}
