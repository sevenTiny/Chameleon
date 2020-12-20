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
        private IDictionary<string, object> _map;

        private static readonly string _key = "Chameleon.Context";

        public static ChameleonContext Current
        {
            get
            {
                ChameleonContext current = (ChameleonContext)CallContext.GetData(ChameleonContext._key);
                if (current == null)
                {
                    current = new ChameleonContext();
                    CallContext.SetData(ChameleonContext._key, current);
                }
                return current;
            }
            set
            {
                CallContext.SetData(ChameleonContext._key, value);
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
                return this.Get("Chameleon.Context.XApplicationName");
            }
            set
            {
                this.Put("Chameleon.Context.XApplicationName", value);
            }
        }

        public int TenantId
        {
            get
            {
                string value = this.Get("Chameleon.Context.XTenantId");
                if (value != null)
                {
                    return System.Convert.ToInt32(value);
                }
                return -1;
            }
            set
            {
                this.Put("Chameleon.Context.XTenantId", value);
            }
        }

        public int UserId
        {
            get
            {
                string value = this.Get("Chameleon.Context.XUserId");
                if (value != null)
                {
                    return System.Convert.ToInt32(value);
                }
                return -1;
            }
            set
            {
                this.Put("Chameleon.Context.XUserId", value);
            }
        }

        public bool HasValue
        {
            get
            {
                return this._map != null && this._map.Count > 0;
            }
        }

        internal void Put(string key, int value)
        {
            this._map[key] = value.ToString();
        }

        public void Put(string key, string value)
        {
            this._map[key] = value;
        }

        public void Put(string key, object value)
        {
            this._map[key] = JsonConvert.SerializeObject(value);
        }

        public bool Contains(string key)
        {
            return this._map.ContainsKey(key);
        }

        public string Get(string key)
        {
            object value;
            this._map.TryGetValue(key, out value);
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }

        public T Get<T>(string key)
        {
            object value;
            this._map.TryGetValue(key, out value);
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value.ToString());
            }
            return default(T);
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
            CallContext.Remove(ChameleonContext._key);
        }
    }
}
