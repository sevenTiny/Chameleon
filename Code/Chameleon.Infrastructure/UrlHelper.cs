namespace Chameleon.Infrastructure
{
    public static class UrlHelper
    {
        /// <summary>
        /// 移除URL上指定的参数,不区分参数大小写
        /// </summary>
        public static string RemoveUrlParam(string url, string param)
        {
            var lowerUrl = url.ToLower();
            var lowerParam = param.ToLower();
            if (lowerUrl.IndexOf("&" + lowerParam) > 0)
            {
                var beginUrl = url.Substring(0, lowerUrl.IndexOf("&" + lowerParam));
                var endUrl = url.Substring(lowerUrl.IndexOf("&" + lowerParam) + 1, url.Length - lowerUrl.IndexOf("&" + lowerParam) - 1);
                if (endUrl.IndexOf("&") > 0)
                    endUrl = endUrl.Substring(endUrl.IndexOf("&"), endUrl.Length - endUrl.IndexOf("&"));
                else
                    endUrl = "";
                return beginUrl + endUrl;
            }
            else if (lowerUrl.IndexOf("?" + lowerParam) > 0)
            {
                var beginUrl = url.Substring(0, lowerUrl.IndexOf("?" + lowerParam));
                var endUrl = url.Substring(lowerUrl.IndexOf("?" + lowerParam) + 1, url.Length - lowerUrl.IndexOf("?" + lowerParam) - 1);
                if (endUrl.IndexOf("&") > 0)
                    endUrl = "?" + endUrl.Substring(endUrl.IndexOf("&") + 1, endUrl.Length - endUrl.IndexOf("&") - 1);
                else
                    endUrl = "";
                return beginUrl + endUrl;
            }
            return url;
        }
    }
}
