using System.Text.Json.Serialization;
using ZLMediaKit.Common.Dtos.ApiInputDto;

namespace ZLMediaKit.Common.Dtos
{
    public interface IKeepalive : IApiResultDataBase
    {
        [JsonPropertyName("Buffer")]
        public long Buffer { get;  set; }

        [JsonPropertyName("BufferRaw")]
        public long BufferRaw { get;  set; }

        [JsonPropertyName("BufferLikeString")]
        public long BufferLikeString { get;  set; }

        [JsonPropertyName("BufferList")]
        public long BufferList { get;  set; }

        [JsonPropertyName("TotalMemUsage")]
        public long TotalMemUsage { get;  set; }

        [JsonPropertyName("TtotalMemUsageMB")]
        public int TtotalMemUsageMB { get;  set; }

        [JsonPropertyName("Frame")]
        public long Frame { get;  set; }

        [JsonPropertyName("FrameImp")]
        public long FrameImp { get;  set; }

        [JsonPropertyName("MediaSource")]
        public long MediaSource { get;  set; }

        [JsonPropertyName("MultiMediaSourceMuxer")]
        public long MultiMediaSourceMuxer { get;  set; }

        [JsonPropertyName("RtmpPacket")]
        public long RtmpPacket { get;  set; }
        [JsonPropertyName("RtpPacket")]
        public long RtpPacket { get;  set; }

        [JsonPropertyName("Socket")]
        public long Socket { get;  set; }

        [JsonPropertyName("TcpClient")]
        public long TcpClient { get;  set; }
        [JsonPropertyName("TcpServer")]
        public long TcpServer { get;  set; }
        [JsonPropertyName("TcpSession")]
        public long TcpSession { get;  set; }

        [JsonPropertyName("UdpSession")]
        public long UdpSession { get;  set; }

        [JsonPropertyName("UdpServer")]
        public long UdpServer { get;  set; }

    }

    public class Keepalive : IKeepalive
    {
        public long Buffer { get;  set; }

        public long BufferRaw { get;  set; }

        public long BufferLikeString { get;  set; }

        public long BufferList { get;  set; }

        public long TotalMemUsage { get;  set; }

        public int TtotalMemUsageMB { get;  set; }

        public long Frame { get;  set; }

        public long FrameImp { get;  set; }

        public long MediaSource { get;  set; }

        public long MultiMediaSourceMuxer { get;  set; }

        public long RtmpPacket { get;  set; }
        public long RtpPacket { get;  set; }

        public long Socket { get;  set; }

        public long TcpClient { get;  set; }
        public long TcpServer { get;  set; }
        public long TcpSession { get;  set; }

        public long UdpSession { get;  set; }

        public long UdpServer { get;  set; }
    }
}
