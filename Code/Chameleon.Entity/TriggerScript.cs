using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.ComponentModel;

namespace Chameleon.Entity
{
    [Table]
    public class TriggerScript : CommonBase
    {
        /// <summary>
        /// 所属应用Id
        /// </summary>
        [Column]
        public Guid CloudApplicationId { get; set; }
        /// <summary>
        /// 所属对象Id，如果是对象接口触发器，则该字段有值，且为当前对象的id
        /// </summary>
        [Column]
        public Guid MetaObjectId { get; set; }
        /// <summary>
        /// 触发器类型
        /// </summary>
        [Column]
        public int ScriptType { get; set; }
        /// <summary>
        /// 对象接口服务类型,如果是对象接口触发器，则该字段有值，且为相应的服务类型
        /// </summary>
        [Column]
        public int MetaObjectInterfaceServiceType { get; set; }
        /// <summary>
        /// 脚本语言
        /// </summary>
        [Column]
        public int Language { get; set; }
        /// <summary>
        /// 类全名
        /// </summary>
        [Column]
        public string ClassFullName { get; set; }
        /// <summary>
        /// 方法全名
        /// </summary>
        [Column]
        public string FunctionName { get; set; }
        /// <summary>
        /// 脚本内容
        /// </summary>
        [Column]
        public string Script { get; set; }

        /// <summary>
        /// 获取脚本类型枚举
        /// </summary>
        /// <returns></returns>
        public ScriptTypeEnum GetScriptType() => (ScriptTypeEnum)this.ScriptType;

        /// <summary>
        /// 获取对象接口服务类型枚举
        /// </summary>
        /// <returns></returns>
        public MetaObjectInterfaceServiceTypeEnum GetMetaObjectInterfaceServiceType() => (MetaObjectInterfaceServiceTypeEnum)this.MetaObjectInterfaceServiceType;

        /// <summary>
        /// 获取语言枚举
        /// </summary>
        /// <returns></returns>
        public LanguageEnum GetLanguage() => (LanguageEnum)this.Language;
    }

    /// <summary>
    /// 脚本类型
    /// </summary>
    public enum ScriptTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        /// <summary>
        /// 对象接口触发器
        /// </summary>
        [Description("对象接口触发器")]
        MetaObjectInterfaceTrigger = 1,
        /// <summary>
        /// 动态脚本接口触发器
        /// </summary>
        [Description("动态脚本接口触发器")]
        DynamicScriptDataSourceTrigger = 2,
        /// <summary>
        /// Json数据源
        /// </summary>
        [Description("JSON 数据源")]
        JsonDataSource = 3
    }

    /// <summary>
    /// 服务类型
    /// </summary>
    public enum MetaObjectInterfaceServiceTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("新增-前")]
        Add_Before = 1,
        [Description("新增-后")]
        Add_After = 2,
        [Description("批量新增-前")]
        BatchAdd_Before = 3,
        [Description("批量新增-后")]
        BatchAdd_After = 4,
        [Description("编辑-前")]
        Update_Before = 5,
        [Description("编辑-后")]
        Update_After = 6,
        [Description("删除-前")]
        Delete_Before = 7,
        [Description("删除-后")]
        Delete_After = 8,
        [Description("查询数量-前")]
        QueryCount_Before = 9,
        [Description("查询数量-后")]
        QueryCount_After = 10,
        [Description("查询单条记录-前")]
        QuerySingle_Before = 11,
        [Description("查询单条记录-后")]
        QuerySingle_After = 12,
        [Description("查询集合-前")]
        QueryList_Before = 13,
        [Description("查询集合-后")]
        QueryList_After = 14
    }

    public enum LanguageEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        /// <summary>
        /// C#
        /// </summary>
        [Description("CSharp")]
        CSharp = 1,
    }
}
