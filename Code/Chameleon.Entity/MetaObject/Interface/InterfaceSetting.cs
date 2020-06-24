using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 接口设置
    /// </summary>
    [Table]
    public class InterfaceSetting : MetaObjectBase
    {
        /// <summary>
        /// 所属对象编码
        /// </summary>
        [Column]
        public string MetaObjectCode { get; set; }
        /// <summary>
        /// 所属应用编码
        /// </summary>
        [Column]
        public string CloudApplicationCode { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        [Column]
        public int InterfaceType { get; set; }
        /// <summary>
        /// 接口条件id
        /// </summary>
        [Column]
        public Guid InterfaceConditionId { get; set; }
        /// <summary>
        /// 接口校验id
        /// </summary>
        [Column]
        public Guid InterfaceVerificationId { get; set; }
        /// <summary>
        /// 接口字段id
        /// </summary>
        [Column]
        public Guid InterfaceFieldsId { get; set; }
        /// <summary>
        /// 数据源Id
        /// </summary>
        [Column]
#warning 这里改了数据库!!!!!!!!!!!!!!!!!!!!
        public Guid DynamicScriptInterfaceId { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        [Column]
        public int PageSize { get; set; }

        #region 不参与数据存储的字段
        public string InterfaceConditionName { get; set; }
        public string InterfaceVerificationName { get; set; }
        public string InterfaceFieldsName { get; set; }
        public string DynamicScriptInterfaceName { get; set; }
        #endregion

        public InterfaceTypeEnum GetInterfaceType()
        {
            return (InterfaceTypeEnum)this.InterfaceType;
        }

        public void SetInterfaceType(InterfaceTypeEnum interfaceTypeEnum)
        {
            this.InterfaceType = (int)interfaceTypeEnum;
        }
    }

    /// <summary>
    /// 接口类型
    /// </summary>
    public enum InterfaceTypeEnum
    {
        [Description("UnKnown")]
        UnKnown = 0,
        [Description("添加")]
        Add = 1,
        [Description("批量添加")]
        BatchAdd = 2,
        [Description("更新")]
        Update = 3,
        [Description("删除")]
        Delete = 4,
        [Description("查询数量")]
        QueryCount = 5,
        [Description("查询单条记录")]
        QuerySingle = 6,
        [Description("查询记录集合")]
        QueryList = 7,
        [Description("动态脚本接口")]
        DynamicScriptInterface = 8
    }
}
