using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetRtpInfoResult : IApiResultBase
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        [JsonPropertyName("exist")]
        public bool exist { get; set; }

        /// <summary>
        /// 本级IP
        /// </summary>
        [JsonPropertyName("local_ip")]
        public string LocalIp { get; set; }
        /// <summary>
        /// 本级端口
        /// </summary>
        [JsonPropertyName("local_port")]
        public int LocalPort { get; set; }
        /// <summary>
        /// 对端IP
        /// </summary>
        [JsonPropertyName("peer_ip")]
        public string PeerIp { get; set; }
        /// <summary>
        /// 对端端口
        /// </summary>
        [JsonPropertyName("peer_port")]
        public int PeerPort { get; set; }
    }

    public class ApiGetRtpInfoResult : ApiResultBase, IApiGetRtpInfoResult
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        [JsonPropertyName("exist")]
        public bool exist { get; set; }

        /// <summary>
        /// 本级IP
        /// </summary>
        [JsonPropertyName("local_ip")]
        public string LocalIp { get; set; }
        /// <summary>
        /// 本级端口
        /// </summary>
        [JsonPropertyName("local_port")]
        public int LocalPort { get; set; }
        /// <summary>
        /// 对端IP
        /// </summary>
        [JsonPropertyName("peer_ip")]
        public string PeerIp { get; set; }
        /// <summary>
        /// 对端端口
        /// </summary>
        [JsonPropertyName("peer_port")]
        public int PeerPort { get; set; }
    }
}
