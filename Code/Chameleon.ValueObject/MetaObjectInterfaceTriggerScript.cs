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
        internal abstract string ClassFullName { get; }
        internal abstract string FunctionName { get; }
        internal abstract string Script { get; }
    }
    public class MetaObjectInterface_Add_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_Add_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_BatchAdd_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_BatchAdd_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_Update_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_Update_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_Delete_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_Delete_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_QueryCount_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_QueryCount_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_QuerySingle_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_QuerySingle_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_QueryList_Before : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class MetaObjectInterface_QueryList_After : DefaultScriptBase
    {
        internal override string ClassFullName => "";

        internal override string FunctionName => "";

        internal override string Script => "";
    }
    public class DynamicScriptInterfaceScript : DefaultScriptBase
    {
        internal override string ClassFullName => throw new NotImplementedException();

        internal override string FunctionName => throw new NotImplementedException();

        internal override string Script => throw new NotImplementedException();
    }
}
