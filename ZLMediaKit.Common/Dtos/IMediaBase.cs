using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos
{
    public interface IMediaBase
    {
        /// <summary>
        /// 流虚拟主机
        /// </summary>
        [JsonPropertyName("vhost")]
        public string Vhost { get; set; }
        /// <summary>
        /// 流应用名
        /// </summary>
        [JsonPropertyName("app")]
        public string App { get; set; }
        /// <summary>
        /// 播放或推流的协议，可能是rtsp、rtmp、http
        /// </summary>
        [JsonPropertyName("schema")]
        public string Schema { get; set; }
        /// <summary>
        /// 流ID
        /// </summary>
        [JsonPropertyName("stream")]
        public string Stream { get; set; }
    }

    public class MediaBase : IMediaBase
    {
        public string Vhost { get; set; }

        public string App { get; set; }

        public string Schema { get; set; }

        public string Stream { get; set; }
    }
}
