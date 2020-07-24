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
        public int InterfaceServiceType { get; set; }
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
        /// 获取接口服务类型枚举
        /// </summary>
        /// <returns></returns>
        public InterfaceServiceTypeEnum GetInterfaceServiceType() => (InterfaceServiceTypeEnum)this.InterfaceServiceType;

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
        [Description("对象接口触发器")]
        MetaObjectInterfaceTrigger = 1,
        [Description("动态脚本接口触发器")]
        DynamicScriptDataSourceTrigger = 2,
        [Description("JSON 数据源")]
        JsonDataSource = 3,
        [Description("文件管理")]
        FileManagement = 4,
    }

    public static class ScriptTypeEnumHelper
    {
        public static ScriptTypeEnum GetScriptTypeByInterfaceServiceTypeMapping(InterfaceServiceTypeEnum interfaceServiceTypeEnum)
        {
            switch (interfaceServiceTypeEnum)
            {
                case InterfaceServiceTypeEnum.MetaObject_Add_Before:
                case InterfaceServiceTypeEnum.MetaObject_Add_After:
                case InterfaceServiceTypeEnum.MetaObject_BatchAdd_Before:
                case InterfaceServiceTypeEnum.MetaObject_BatchAdd_After:
                case InterfaceServiceTypeEnum.MetaObject_Update_Before:
                case InterfaceServiceTypeEnum.MetaObject_Update_After:
                case InterfaceServiceTypeEnum.MetaObject_Delete_Before:
                case InterfaceServiceTypeEnum.MetaObject_Delete_After:
                case InterfaceServiceTypeEnum.MetaObject_QueryCount_Before:
                case InterfaceServiceTypeEnum.MetaObject_QueryCount_After:
                case InterfaceServiceTypeEnum.MetaObject_QuerySingle_Before:
                case InterfaceServiceTypeEnum.MetaObject_QuerySingle_After:
                case InterfaceServiceTypeEnum.MetaObject_QueryList_Before:
                case InterfaceServiceTypeEnum.MetaObject_QueryList_After:
                    return ScriptTypeEnum.MetaObjectInterfaceTrigger;
                case InterfaceServiceTypeEnum.Application_UploadFile_Before:
                case InterfaceServiceTypeEnum.Application_UploadFile_After:
                case InterfaceServiceTypeEnum.Application_DownloadFile_Before:
                case InterfaceServiceTypeEnum.Application_DownloadFile_After:
                    return ScriptTypeEnum.FileManagement;
                case InterfaceServiceTypeEnum.Application_DataSource:
                    return ScriptTypeEnum.DynamicScriptDataSourceTrigger;
                case InterfaceServiceTypeEnum.UnKnown:
                default:
                    return ScriptTypeEnum.UnKnown;
            }
        }
    }

    /// <summary>
    /// 服务类型
    /// </summary>
    public enum InterfaceServiceTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,

        //对象触发器
        [Description("新增-前")]
        MetaObject_Add_Before = 1,
        [Description("新增-后")]
        MetaObject_Add_After = 2,
        [Description("批量新增-前")]
        MetaObject_BatchAdd_Before = 3,
        [Description("批量新增-后")]
        MetaObject_BatchAdd_After = 4,
        [Description("编辑-前")]
        MetaObject_Update_Before = 5,
        [Description("编辑-后")]
        MetaObject_Update_After = 6,
        [Description("删除-前")]
        MetaObject_Delete_Before = 7,
        [Description("删除-后")]
        MetaObject_Delete_After = 8,
        [Description("查询数量-前")]
        MetaObject_QueryCount_Before = 9,
        [Description("查询数量-后")]
        MetaObject_QueryCount_After = 10,
        [Description("查询单条记录-前")]
        MetaObject_QuerySingle_Before = 11,
        [Description("查询单条记录-后")]
        MetaObject_QuerySingle_After = 12,
        [Description("查询集合-前")]
        MetaObject_QueryList_Before = 13,
        [Description("查询集合-后")]
        MetaObject_QueryList_After = 14,

        //应用触发器
        [Description("上传文件-前")]
        Application_UploadFile_Before = 15,
        [Description("上传文件-前")]
        Application_UploadFile_After = 16,
        [Description("下载文件-前")]
        Application_DownloadFile_Before = 17,
        [Description("下载文件-后")]
        Application_DownloadFile_After = 18,

        [Description("脚本数据源")]
        Application_DataSource = 19
    }

    public static class InterfaceServiceTypeEnumHelper
    {
        /// <summary>
        /// 对象类型触发器
        /// </summary>
        /// <returns></returns>
        public static InterfaceServiceTypeEnum[] GetMetaObjectInterfaceServiceTypeEnums()
        {
            return new InterfaceServiceTypeEnum[] {
                InterfaceServiceTypeEnum.MetaObject_Add_Before,
                InterfaceServiceTypeEnum.MetaObject_Add_After,
                InterfaceServiceTypeEnum.MetaObject_BatchAdd_Before,
                InterfaceServiceTypeEnum.MetaObject_BatchAdd_After,
                InterfaceServiceTypeEnum.MetaObject_Update_Before,
                InterfaceServiceTypeEnum.MetaObject_Update_After,
                InterfaceServiceTypeEnum.MetaObject_Delete_Before,
                InterfaceServiceTypeEnum.MetaObject_Delete_After,
                InterfaceServiceTypeEnum.MetaObject_QueryCount_Before,
                InterfaceServiceTypeEnum.MetaObject_QueryCount_After,
                InterfaceServiceTypeEnum.MetaObject_QuerySingle_Before,
                InterfaceServiceTypeEnum.MetaObject_QuerySingle_After,
                InterfaceServiceTypeEnum.MetaObject_QueryList_Before,
                InterfaceServiceTypeEnum.MetaObject_QueryList_After,
            };
        }

        /// <summary>
        /// 应用类型触发器
        /// </summary>
        /// <returns></returns>
        public static InterfaceServiceTypeEnum[] GetCloudApplicationInterfaceServiceTypeEnums()
        {
            return new InterfaceServiceTypeEnum[] {
                InterfaceServiceTypeEnum.Application_UploadFile_Before,
                InterfaceServiceTypeEnum.Application_UploadFile_After,
                InterfaceServiceTypeEnum.Application_DownloadFile_Before,
                InterfaceServiceTypeEnum.Application_DownloadFile_After,
            };
        }
    }

    public static class LanguageEnumHelper
    {
        public static LanguageEnum[] GetLanguageEnums()
        {
            return new LanguageEnum[] {
                LanguageEnum.CSharp
            };
        }
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
