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
    public static class InterfaceRequest
    {
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

        public static Result<TResult> CloudDataGet<TResult>(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            var result = HttpHelper.Get(new GetRequestArgs
            {
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}{UrlArgumentsBulder(arguments)}"
            });

            return JsonConvert.DeserializeObject<Result<TResult>>(result);
        }

        public static Result<TResult> CloudDataPost<TResult>(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Post(new PostRequestArgs
            {
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<Result<TResult>>(result);
        }

        public static Result<TResult> CloudDataPut<TResult>(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Put(new PutRequestArgs
            {
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<Result<TResult>>(result);
        }

        public static Result<TResult> CloudDataDelete<TResult>(string interfaceCode, Dictionary<string, string> arguments)
        {
            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

            string data = string.Empty;

            if (arguments != null)
                data = JsonConvert.SerializeObject(arguments);

            var result = HttpHelper.Delete(new DeleteRequestArgs
            {
                Url = $"{UrlsConfig.Instance.DataApi}/api/CloudData?_interface={interfaceCode}",
                Encoding = Encoding.UTF8,
                ContentType = "application/json",
                Data = data
            });

            return JsonConvert.DeserializeObject<Result<TResult>>(result);
        }
    }
}
