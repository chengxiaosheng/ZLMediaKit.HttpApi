using System.Text.Json.Serialization;
using ZLMediaKit.Common.Dtos.ApiInputDto;

namespace ZLMediaKit.Common.Dtos
{
    /// <summary>
    /// Socket 连接信息
    /// </summary>
    public interface ISocketInfo : IApiResultDataBase
    {
        /// <summary>
        /// tcp 唯一标识
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
        /// <summary>
        /// 本级IP
        /// </summary>
        [JsonPropertyName("local_ip")]
        public string LocalIp { get; set; }
        /// <summary>
        /// 本级端口
        /// </summary>
        [JsonPropertyName("local_port")]
        public string LocalPort { get; set; }
        /// <summary>
        /// 对端IP
        /// </summary>
        [JsonPropertyName("peer_ip")]
        public string PeerIp { get; set; }
        /// <summary>
        /// 对端端口
        /// </summary>
        [JsonPropertyName("peer_port")]
        public string PeerPort { get; set; }

        /// <summary>
        /// 客户端TCPSession typeid
        /// </summary>
        [JsonPropertyName("typeid")]
        public string TypeId { get; set; }
    }

    /// <summary>
    /// Socket 连接信息
    /// </summary>
    public class SocketInfo : ISocketInfo
    {
        /// <summary>
        /// tcp 唯一标识
        /// </summary>
        [JsonPropertyName("identifier")]
        public string Identifier { get;  set; }
        /// <summary>
        /// 本级IP
        /// </summary>
        [JsonPropertyName("local_ip")]
        public string LocalIp { get;  set; }
        /// <summary>
        /// 本级端口
        /// </summary>
        [JsonPropertyName("local_port")]
        public string LocalPort { get;  set; }
        /// <summary>
        /// 对端IP
        /// </summary>
        [JsonPropertyName("peer_ip")]
        public string PeerIp { get;  set; }
        /// <summary>
        /// 对端端口
        /// </summary>
        [JsonPropertyName("peer_port")]
        public string PeerPort { get;  set; }

        /// <summary>
        /// 客户端TCPSession typeid
        /// </summary>
        [JsonPropertyName("typeid")]
        public string TypeId { get; set; }
    }
}
