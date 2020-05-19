using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Entity
{
    [TableCaching]
    [Table]
    public class MetaObject : CommonBase
    {
        [Column]
        public Guid ApplicationId { get; set; }
    }
}
