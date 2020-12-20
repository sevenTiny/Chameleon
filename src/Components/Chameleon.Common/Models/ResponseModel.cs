//using Newtonsoft.Json;
//using SevenTiny.Bantina;
//using System;
//using System.Collections.Generic;

//namespace Chameleon.Common.Models
//{
//    /// <summary>
//    /// 通用的前端交互实体
//    /// </summary>
//    [Serializable]
//    public class ResponseModel
//    {
//        [JsonProperty("success")]
//        public bool IsSuccess { get; set; }
//        [JsonProperty("code")]
//        public int Code { get; set; } = 200;
//        [JsonProperty("msg")]
//        public string Message { get; set; }
//        [JsonProperty("more_msg")]
//        public List<string> MoreMessage { get; set; }
//        [JsonProperty("tip_type")]
//        public TipType TipType { get; set; }
//        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
//        public object Data { get; set; }
//    }
//}