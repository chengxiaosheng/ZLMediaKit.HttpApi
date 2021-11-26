using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiOpenRtpServerResult : IApiResultBase
    {
        /// <summary>
        /// 接收端口，方便获取随机端口号
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
    }

    public class ApiOpenRtpServerResult : ApiResultBase, IApiOpenRtpServerResult
    {
        /// <summary>
        /// 接收端口，方便获取随机端口号
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
    }
}
