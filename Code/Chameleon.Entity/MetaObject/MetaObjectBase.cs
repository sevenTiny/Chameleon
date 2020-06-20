using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 对象操作基类
    /// </summary>
    public abstract class MetaObjectBase : CommonBase
    {
        /// <summary>
        /// 所属对象Id
        /// </summary>
        [Column]
        public Guid MetaObjectId { get; set; }
        /// <summary>
        /// 所属应用Id
        /// </summary>
        [Column]
        public Guid CloudApplicationtId { get; set; 
    }
}
