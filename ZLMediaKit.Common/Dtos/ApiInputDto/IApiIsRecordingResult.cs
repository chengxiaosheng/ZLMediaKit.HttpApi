
using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiIsRecordingResult : IApiResultBase
    {
        /// <summary>
        /// false:未录制,true:正在录制
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }
    }

    public class ApiIsRecordingResult : ApiResultBase, IApiIsRecordingResult
    {
        /// <summary>
        /// false:未录制,true:正在录制
        /// </summary>
        [JsonPropertyName("status")]
        public bool Status { get; set; }
    }
}
