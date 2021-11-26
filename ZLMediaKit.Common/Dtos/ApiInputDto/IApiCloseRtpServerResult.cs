using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiCloseRtpServerResult : IApiResultBase
    {
        /// <summary>
        /// CloseRtpServer
        /// </summary>
        [JsonPropertyName("hit")]
        public int Hit { get; set; }
    }

    public class ApiCloseRtpServerResult : ApiResultBase, IApiCloseRtpServerResult
    {
        /// <summary>
        /// CloseRtpServer
        /// </summary>
        [JsonPropertyName("hit")]
        public int Hit { get; set; }
    }
}
