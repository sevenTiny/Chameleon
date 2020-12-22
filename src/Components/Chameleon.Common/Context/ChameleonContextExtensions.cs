using System.Collections.Generic;

namespace Chameleon.Common.Context
{
    /// <summary>
    /// 上下文扩展
    /// </summary>
    public static class ChameleonContextExtensions
    {
        /// <summary>
        /// 获取请求url的参数
        /// </summary>
        /// <param name="chameleonContext">上下文实例</param>
        /// <returns>如果没有，则返回空集合</returns>
        public static IDictionary<string, string> GetRequestQuery(this ChameleonContext chameleonContext)
        {
            return (chameleonContext?.Get(ChameleonContext.Const.RequestQuery) as Dictionary<string, string>) ?? new Dictionary<string, string>(0);
        }

        /// <summary>
        /// 获取请求body
        /// </summary>
        /// <param name="chameleonContext">上下文实例</param>
        /// <returns>如果没有，返回null</returns>
        public static object GetRequestBody(this ChameleonContext chameleonContext)
        {
            return chameleonContext?.Get(ChameleonContext.Const.RequestBody);
        }
    }
}
