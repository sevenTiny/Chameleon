﻿using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace Chameleon.Entity
{
    /// <summary>
    /// 应用权限
    /// </summary>
    [Table]
    public class CloudApplicationPermission : CommonBase
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Column]
        public Guid CloudApplicationId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column]
        public long UserId { get; set; }
    }
}
