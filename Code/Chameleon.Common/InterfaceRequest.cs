using Chameleon.Common.Configs;
using Newtonsoft.Json;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Net.Http;
using SevenTiny.Bantina.Validation;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Common
{
    /// <summary>
    /// 接口请求（代码中httprequest请求，保证走触发器）
    /// </summary>
    public class InterfaceRequest
    {
        private string _AccessToken;
        public InterfaceRequest(Dictionary<string, string> triggerContext)
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

        private static string UrlArgumentsBuilder(Dictionary<string, string> arguments)
        {
            StringBuilder urlArg = new StringBuilder();

            if (arguments != null)
            {
                foreach (var item in arguments)
                {
                    urlArg.Append("&");
                    urlArg.Append(item.Key);
                    urlArg.Append("=");
                    urlArg.Append(item.Value);
                }
            }

            return urlArg.ToString();
        }

        public ResponseModel CloudDataGet(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            var result = HttpHelper.Get(new GetRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}{UrlArgumentsBuilder(arguments)}"
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }

        public ResponseModel CloudDataPost(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Post(new PostRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }

        public ResponseModel CloudDataPut(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Put(new PutRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }

        public ResponseModel CloudDataDelete(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Delete(new DeleteRequestArgs
            {
                Headers = GetHeaders(),
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }
    }
}
