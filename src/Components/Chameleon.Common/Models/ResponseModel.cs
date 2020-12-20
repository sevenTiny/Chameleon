using Newtonsoft.Json;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;

namespace Chameleon.Common.Models
{
    /// <summary>
    /// 通用的前端交互实体
    /// </summary>
    [Serializable]
    public class ResponseModel
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; } = 200;
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("more_msg")]
        public List<string> MoreMessage { get; set; }
        [JsonProperty("tip_type")]
        public TipType TipType { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// 快速返回成功模型
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseModel Success(string message, object data = null)
        {
            return new ResponseModel
            {
                IsSuccess = true,
                Code = 200,
                Message = message,
                TipType = TipType.Success,
                Data = data
            };
        }

        /// <summary>
        /// 快速返回成功模型
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseModel Info(string message, object data = null)
        {
            return new ResponseModel
            {
                IsSuccess = true,
                Code = 200,
                Message = message,
                TipType = TipType.Info,
                Data = data
            };
        }

        /// <summary>
        /// 快速返回成功模型
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseModel Warning(string message, object data = null)
        {
            return new ResponseModel
            {
                IsSuccess = true,
                Code = 200,
                Message = message,
                TipType = TipType.Warning,
                Data = data
            };
        }

        /// <summary>
        /// 快速返回失败模型
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="moreMessage"></param>
        /// <returns></returns>
        public static ResponseModel Error(int code, string message, List<string> moreMessage = null)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Code = code,
                Message = message,
                TipType = TipType.Error,
                MoreMessage = moreMessage
            };
        }
    }
}