using Chameleon.Entity;
using System.Collections.Generic;

namespace Chameleon.ValueObject
{
    /// <summary>
    /// 应用部署载体
    /// </summary>
    public class CloudApplicationDeployDto
    {
        /// <summary>
        /// 应用
        /// </summary>
        public List<CloudApplication> CloudApplication { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public List<MetaObject> MetaObject { get; set; }
        /// <summary>
        /// 触发器
        /// </summary>
        public List<TriggerScript> TriggerScript { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public List<MetaField> MetaField { get; set; }
        /// <summary>
        /// 接口条件
        /// </summary>
        public List<InterfaceCondition> InterfaceCondition { get; set; }
        /// <summary>
        /// 接口字段
        /// </summary>
        public List<InterfaceFields> InterfaceFields { get; set; }
        /// <summary>
        /// 接口设置
        /// </summary>
        public List<InterfaceSetting> InterfaceSetting { get; set; }
        /// <summary>
        /// 接口排序
        /// </summary>
        public List<InterfaceSort> InterfaceSort { get; set; }
        /// <summary>
        /// 接口校验
        /// </summary>
        public List<InterfaceVerification> InterfaceVerification { get; set; }
    }
}
