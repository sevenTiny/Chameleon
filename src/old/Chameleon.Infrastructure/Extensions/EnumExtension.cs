using System;
using System.ComponentModel;
using System.Reflection;

namespace Chameleon.Infrastructure
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            return (value.GetType().GetField(value.ToString())?.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute descAttr)
                ? descAttr.Description : value.ToString();
        }
    }
}
