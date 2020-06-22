using SevenTiny.Bantina.Bankinate.Attributes;

namespace Chameleon.Entity
{
    /// <summary>
    /// 应用
    /// </summary>
    [Table]
    public class CloudApplication : CommonBase
    {
        [Column]
        public string Icon { get; set; }
    }
}
