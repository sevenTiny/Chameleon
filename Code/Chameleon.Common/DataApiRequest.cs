using Chameleon.Common.Configs;
using Newtonsoft.Json;
using SevenTiny.Bantina.Net.Http;
using SevenTiny.Bantina.Validation;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Common
{
    /// <summary>
    /// 接口请求（代码中httprequest请求dataapi，内部处理了token）
    /// </summary>
    public class DataApiRequest
    {
        private string _AccessToken;
        public DataApiRequest(Dictionary<string, string> triggerContext)
        {
            if (!triggerContext.TryGetValue("_AccessToken", out _AccessToken))
                throw new KeyNotFoundException("_AccessToken key not found in triggerContext, please set triggerContext value from trigger script template default method argument.");
        }

        private Dictionary<string, string> GetHeaders()
        {
            return new Dictionary<string, string>
            {
                { "Authorization",$"Bearer {_AccessToken}"}
            };
        }

        public string Get(string apiRouteNoHost)
        {
            Ensure.ArgumentNotNullOrEmpty(apiRouteNoHost, nameof(apiRouteNoHost));

            var result = HttpHelper.Get(new GetRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}{apiRouteNoHost}"
            });

            return result;
        }

        public ResponseModel Post(string apiRouteNoHost, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(apiRouteNoHost, nameof(apiRouteNoHost));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Post(new PostRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}{apiRouteNoHost}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }

        public ResponseModel Put(string apiRouteNoHost, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(apiRouteNoHost, nameof(apiRouteNoHost));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Put(new PutRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}{apiRouteNoHost}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }

        public ResponseModel Delete(string apiRouteNoHost, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(apiRouteNoHost, nameof(apiRouteNoHost));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Delete(new DeleteRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}{apiRouteNoHost}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }
    }
}
