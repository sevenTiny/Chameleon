using Newtonsoft.Json;
using SevenTiny.Bantina.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.Common.Context
{
    /// <summary>
    /// Faas组件上下文
    /// </summary>
    [Serializable]
    public class ChameleonContext
    {
        /// <summary>
        /// 常量
        /// </summary>
        public static class Const
        {
            /// <summary>
            /// 上下文
            /// </summary>
            public const string ChameleonContext = "Chameleon.Context";
            /// <summary>
            /// 应用名
            /// </summary>
            public const string ApplicationName = "Chameleon.Context.XApplicationName";
            /// <summary>
            /// 租户Id
            /// </summary>
            public const string TenantId = "Chameleon.Context.XTenantId";
            /// <summary>
            /// 用户Id
            /// </summary>
            public const string UserId = "Chameleon.Context.XUserId";
            /// <summary>
            /// 请求参数
            /// </summary>
            public const string RequestQuery = "Chameleon.RequestQuery";
            /// <summary>
            /// 请求体
            /// </summary>
            public const string RequestBody = "Chameleon.RequestBody";
        }

        private IDictionary<string, object> _map;

        public static ChameleonContext Current
        {
            get
            {
                ChameleonContext current = (ChameleonContext)CallContext.GetData(Const.ChameleonContext);
                if (current == null)
                {
                    current = new ChameleonContext();
                    CallContext.SetData(Const.ChameleonContext, current);
                }
                return current;
            }
            set
            {
                CallContext.SetData(Const.ChameleonContext, value);
            }
        }

        public ChameleonContext()
        {
            this._map = new Dictionary<string, object>();
        }

        internal ChameleonContext(IDictionary<string, object> values)
        {
            this._map = values;
        }

        public string ApplicationName
        {
            get
            {
                return Convert.ToString(this.Get(Const.ApplicationName));
            }
            set
            {
                this.Put(Const.ApplicationName, value);
            }
        }

        public int TenantId
        {
            get
            {
                object value = this.Get(Const.TenantId);
                if (value != null)
                {
                    return Convert.ToInt32(value);
                }
                return -1;
            }
            set
            {
                this.Put(Const.TenantId, value);
            }
        }

        public int UserId
        {
            get
            {
                object value = this.Get(Const.UserId);
                if (value != null)
                {
                    return Convert.ToInt32(value);
                }
                return -1;
            }
            set
            {
                this.Put(Const.UserId, value);
            }
        }

        public bool HasValue
        {
            get
            {
                return this._map != null && this._map.Count > 0;
            }
        }

        public void Put(string key, object value)
        {
            this._map[key] = value;
        }

        public bool Contains(string key)
        {
            return this._map.ContainsKey(key);
        }

        public object Get(string key)
        {
            this._map.TryGetValue(key, out object value);
            return value;
        }

        public bool Remove(string key)
        {
            return this._map.Remove(key);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this._map);
        }

        public static ChameleonContext FromJson(string json)
        {
            IDictionary<string, object> map = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
            return new ChameleonContext(map);
        }

        public ChameleonContext Clone()
        {
            return new ChameleonContext(this._map.ToDictionary((KeyValuePair<string, object> k) => k.Key, (KeyValuePair<string, object> k) => k.Value));
        }

        public static void Clear()
        {
            CallContext.Remove(Const.ChameleonContext);
        }
    }
}
