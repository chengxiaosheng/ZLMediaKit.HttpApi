using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiStartSendRtpResult : IApiResultBase
    {
        /// <summary>
        /// 使用的本地端口号 
        /// </summary>
        [JsonPropertyName("local_port")]
        public int LocalPort { get; set; }
    }

    public class ApiStartSendRtpResult : ApiResultBase, IApiStartSendRtpResult
    {
        /// <summary>
        /// 使用的本地端口号 
        /// </summary>
        [JsonPropertyName("local_port")]
        public int LocalPort { get; set; }
    } 
}
