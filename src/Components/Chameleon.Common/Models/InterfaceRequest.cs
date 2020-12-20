//using SevenTiny.Bantina.Validation;
//using System.Collections.Generic;

//namespace Chameleon.Common.Models
//{
//    /// <summary>
//    /// 接口请求（代码中httprequest请求，保证走触发器）
//    /// </summary>
//    public class InterfaceRequest
//    {
//        private DataApiRequest _DataApiRequest;
//        public InterfaceRequest(Dictionary<string, string> triggerContext)
//        {
//            _DataApiRequest = new DataApiRequest(triggerContext);
//        }

//        public ResponseModel CloudDataGet(string interfaceCode)
//        {
//            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

//            string url = $"/api/CloudData?_interface={interfaceCode}";

//            return _DataApiRequest.Get(url);
//        }

//        public ResponseModel CloudDataPost(string interfaceCode, Dictionary<string, string> arguments)
//        {
//            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

//            string url = $"/api/CloudData?_interface={interfaceCode}";

//            return _DataApiRequest.Post(url, arguments);
//        }

//        public ResponseModel CloudDataPut(string interfaceCode, Dictionary<string, string> arguments)
//        {
//            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

//            string url = $"/api/CloudData?_interface={interfaceCode}";

//            return _DataApiRequest.Put(url, arguments);
//        }

//        public ResponseModel CloudDataDelete(string interfaceCode, Dictionary<string, string> arguments)
//        {
//            Ensure.ArgumentNotNullOrEmpty(interfaceCode, nameof(interfaceCode));

//            string url = $"/api/CloudData?_interface={interfaceCode}";

//            return _DataApiRequest.Delete(url, arguments);
//        }
//    }
//}
