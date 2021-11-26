using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiListRtpServerResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 绑定的端口号
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// 绑定的流ID
        /// </summary>
        [JsonPropertyName("stream_id")]
        public string StreamId { get; set; }
    }
    public interface IApiListRtpServerResult : IApiResultListBase<IApiListRtpServerResultItem>
    {

    }

    public class ApiListRtpServerResultItem : IApiListRtpServerResultItem
    {
        /// <summary>
        /// 绑定的端口号
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// 绑定的流ID
        /// </summary>
        [JsonPropertyName("stream_id")]
        public string StreamId { get; set; }
    }

    public class ApiListRtpServerResult : ApiResultListBase<IApiListRtpServerResultItem>, IApiListRtpServerResult
    {

    }


}
