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

        private static string UrlArgumentsBulder(Dictionary<string, string> arguments)
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
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}&_AccessToken={_AccessToken}{UrlArgumentsBulder(arguments)}"
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
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}&_AccessToken={_AccessToken}",
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
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}&_AccessToken={_AccessToken}",
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
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}&_AccessToken={_AccessToken}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<ResponseModel>(result);
        }
    }
}
