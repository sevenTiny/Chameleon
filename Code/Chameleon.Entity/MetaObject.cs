﻿using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Entity
{
    [Table]
    public class MetaObject : CommonBase
    {
        [Column]
        public Guid CloudApplicationId { get; set; }
    }
}
