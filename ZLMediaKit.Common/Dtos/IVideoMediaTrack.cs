using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos
{
    /// <summary>
    /// 媒体轨道
    /// </summary>
    public interface IMediaTrack
    {
        /// <summary>
        /// H264 = 0, H265 = 1, AAC = 2, G711A = 3, G711U = 4
        /// </summary>
        [JsonPropertyName("codec_id")]
        public MediaCodecEnum Codec { get; set; }
        /// <summary>
        /// Video = 0, Audio = 1
        /// </summary>
        [JsonPropertyName("codec_type")]
        public MediaCodecTypeEnum CodecType { get; set; }
        /// <summary>
        /// 编码类型名称
        /// </summary>
        [JsonPropertyName("codec_id_name")]
        public string CodecName { get; set; }
        /// <summary>
        /// 轨道是否准备就绪
        /// </summary>
        [JsonPropertyName("ready")]
        public bool Ready { get; set; }
    }

    public interface IVideoMediaTrack : IMediaTrack
    {
        /// <summary>
        /// 视频fps
        /// </summary>
        [JsonPropertyName("fps")]
        public int Fps { get; set; }

        /// <summary>
        /// 视频高
        /// </summary>
        [JsonPropertyName("height")]
        public int Height { get; set; }

        /// <summary>
        /// 视
        /// </summary>
        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public interface IAudioMediaTrack : IMediaTrack
    {
        /// <summary>
        /// 音频通道数
        /// </summary>
        [JsonPropertyName("channels")]
        public int Channels { get; set; }
        /// <summary>
        /// 音频采样位数
        /// </summary>
        [JsonPropertyName("sample_bit")]
        public int SampleBit { get; set; }
        /// <summary>
        /// 音频采样
        /// </summary>
        [JsonPropertyName("sample_rate")]
        public int SampleRate { get; set; }
    }

    public class MediaTrack : IMediaTrack
    {
        /// <summary>
        /// H264 = 0, H265 = 1, AAC = 2, G711A = 3, G711U = 4
        /// </summary>
        [JsonPropertyName("codec_id")]
        public MediaCodecEnum Codec { get;  set; }
        /// <summary>
        /// Video = 0, Audio = 1
        /// </summary>
        [JsonPropertyName("codec_type")]
        public MediaCodecTypeEnum CodecType { get;  set; }
        /// <summary>
        /// 编码类型名称
        /// </summary>
        [JsonPropertyName("codec_id_name")]
        public string CodecName { get;  set; }
        /// <summary>
        /// 轨道是否准备就绪
        /// </summary>
        [JsonPropertyName("ready")]
        public bool Ready { get;  set; }
    }

    public class VideoMediaTrack: MediaTrack,IVideoMediaTrack
    {
        /// <summary>
        /// 视频fps
        /// </summary>
        [JsonPropertyName("fps")]
        public int Fps { get;  set; }

        /// <summary>
        /// 视频高
        /// </summary>
        [JsonPropertyName("height")]
        public int Height { get;  set; }

        /// <summary>
        /// 视
        /// </summary>
        [JsonPropertyName("width")]
        public int Width { get;  set; }
    }

    public class AudioMediaTrack: MediaTrack,IAudioMediaTrack
    {
        /// <summary>
        /// 音频通道数
        /// </summary>
        [JsonPropertyName("channels")]
        public int Channels { get;  set; }
        /// <summary>
        /// 音频采样位数
        /// </summary>
        [JsonPropertyName("sample_bit")]
        public int SampleBit { get;  set; }
        /// <summary>
        /// 音频采样
        /// </summary>
        [JsonPropertyName("sample_rate")]
        public int SampleRate { get;  set; }
    }
}
