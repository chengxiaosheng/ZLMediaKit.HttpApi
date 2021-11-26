using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZLMediaKit.Common.Dtos.ApiInputDto;

namespace ZLMediaKit.Common.Dtos
{
    public interface IMediaInfo: IMediaBase, IApiResultDataBase
    {
        /// <summary>
        /// 是否为注册
        /// </summary>
        [JsonPropertyName("regist")]
        public bool Regist { get; set; }
        /// <summary>
        /// 存活时间，单位秒
        /// </summary>
        [JsonPropertyName("aliveSecond")]
        public int AliveSecond { get; set; }
        /// <summary>
        /// 数据产生速度，单位byte/s
        /// </summary>
        [JsonPropertyName("bytesSpeed")]
        public int BytesSpeed { get; set; }
        /// <summary>
        /// GMT unix系统时间戳，单位秒
        /// </summary>
        [JsonPropertyName("createStamp")]
        public long CreateStamp { get; set; }

        /// <summary>
        /// 链接信息
        /// </summary>
        [JsonPropertyName("originSock")]
        public ISocketInfo OriginSock { get; set; }

        /// <summary>
        /// 产生源类型，包括 unknown = 0,rtmp_push=1,rtsp_push=2,rtp_push=3,pull=4,ffmpeg_pull=5,mp4_vod=6,device_chn=7,rtc_push=8
        /// </summary>
        [JsonPropertyName("originType")]
        public OriginTypeEnum OriginType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("originTypeStr")]
        public bool OriginTypeStr { get; set; }
        /// <summary>
        /// 产生源的url
        /// </summary>
        [JsonPropertyName("originUrl")]
        public bool OriginUrl { get; set; }
        /// <summary>
        ///  本协议观看人数
        /// </summary>
        [JsonPropertyName("readerCount")]
        public bool ReaderCount { get; set; }
        /// <summary>
        /// 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv/rtc
        /// </summary>
        [JsonPropertyName("totalReaderCount")]
        public bool TotalReaderCount { get; set; }
        /// <summary>
        /// 媒体轨道信息
        /// </summary>
        [JsonPropertyName("tracks")]
        public List<IMediaTrack> Tracks { get; set; }

    }

    public class MediaInfo: MediaBase, IMediaInfo
    {
        /// <summary>
        /// 是否为注册
        /// </summary>
        [JsonPropertyName("regist")]
        public bool Regist { get; set; }
        /// <summary>
        /// 存活时间，单位秒
        /// </summary>
        [JsonPropertyName("aliveSecond")]
        public int AliveSecond { get; set; }
        /// <summary>
        /// 数据产生速度，单位byte/s
        /// </summary>
        [JsonPropertyName("bytesSpeed")]
        public int BytesSpeed { get; set; }
        /// <summary>
        /// GMT unix系统时间戳，单位秒
        /// </summary>
        [JsonPropertyName("createStamp")]
        public long CreateStamp { get; set; }

        /// <summary>
        /// 链接信息
        /// </summary>
        [JsonPropertyName("originSock")]
        public ISocketInfo OriginSock { get; set; }

        /// <summary>
        /// 产生源类型，包括 unknown = 0,rtmp_push=1,rtsp_push=2,rtp_push=3,pull=4,ffmpeg_pull=5,mp4_vod=6,device_chn=7,rtc_push=8
        /// </summary>
        [JsonPropertyName("originType")]
        public OriginTypeEnum OriginType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("originTypeStr")]
        public bool OriginTypeStr { get; set; }
        /// <summary>
        /// 产生源的url
        /// </summary>
        [JsonPropertyName("originUrl")]
        public bool OriginUrl { get; set; }
        /// <summary>
        ///  本协议观看人数
        /// </summary>
        [JsonPropertyName("readerCount")]
        public bool ReaderCount { get; set; }
        /// <summary>
        /// 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv/rtc
        /// </summary>
        [JsonPropertyName("totalReaderCount")]
        public bool TotalReaderCount { get; set; }
        /// <summary>
        /// 媒体轨道信息
        /// </summary>
        [JsonPropertyName("tracks")]
        public List<IMediaTrack> Tracks { get; set; }
    }
}
