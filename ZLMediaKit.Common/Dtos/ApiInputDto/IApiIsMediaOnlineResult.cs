using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiIsMediaOnlineResult : IApiResultBase
    {
        /// <summary>
        /// 是否在线
        /// </summary>
        [JsonPropertyName("online")]
        public bool Online { get; set; }
    }

    public class ApiIsMediaOnlineResult : ApiResultBase, IApiIsMediaOnlineResult
    {
        /// <summary>
        /// 是否在线
        /// </summary>
        [JsonPropertyName("online")]
        public bool Online { get; set; }
    }
}
