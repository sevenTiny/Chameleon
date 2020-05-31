using Chameleon.Infrastructure.Enums;
using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
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
        UnKnown = 0,
        /// <summary>
        /// 添加
        /// </summary>
        Add = 1,
        /// <summary>
        /// 批量添加
        /// </summary>
        BatchAdd = 2,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 3,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 查询数量统计
        /// </summary>
        QueryCount = 5,
        /// <summary>
        /// 查询单条记录
        /// </summary>
        QuerySingle = 6,
        /// <summary>
        /// 查询记录集合
        /// </summary>
        QueryList = 7
    }
}
