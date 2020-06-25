namespace Chameleon.ValueObject
{
    /// <summary>
    /// 通用数据交换对象
    /// </summary>
    public class CloudData
    {
        /// <summary>
        /// 字段编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 字段显示名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 字段显示值
        /// </summary>
        public string Text { get; set; }
    }
}
