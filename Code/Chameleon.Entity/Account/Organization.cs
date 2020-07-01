﻿using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Chameleon.Entity
{
    /// <summary>
    /// 组织
    /// </summary>
    [Table]
    public class Organization : CommonBase
    {
        /// <summary>
        /// 父组织id
        /// </summary>
        [Column]
        public Guid Parent { get; set; }
    }
}