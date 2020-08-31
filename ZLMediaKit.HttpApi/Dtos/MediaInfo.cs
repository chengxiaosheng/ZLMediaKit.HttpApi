using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class MediaInfo
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string App { get; set; }
        /// <summary>
        /// 本协议观看人数
        /// </summary>
        public int ReaderCount { get; set; }
        /// <summary>
        /// 观看总人数，包括hls/rtsp/rtmp/http-flv/ws-flv
        /// </summary>
        public int TotalReaderCount { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 流id
        /// </summary>
        public string Stream { get; set; }
        /// <summary>
        /// 虚拟主机名
        /// </summary>
        public string Vhost { get; set; }

        public List<MediaInfoTrack> Tracks = new List<MediaInfoTrack>();

    }
    /// <summary>
    /// 
    /// </summary>
    public class MediaInfoTrack
    {
        /// <summary>
        /// 音频通道数
        /// </summary>
        public int Channels { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public CodecEnum Codec_id { get; set; }

        /// <summary>
        /// 编码类型名称
        /// </summary>
        public string Codec_Id_Name { get; set; }

        /// <summary>
        /// Video = 0, Audio = 1
        /// </summary>
        public CodecTypeEnum Codec_Type { get; set; }

        /// <summary>
        /// 是否准备就绪
        /// </summary>
        public bool Ready { get; set; }

        /// <summary>
        /// 音频采样位数
        /// </summary>
        public int Sample_Bit { get; set; }

        /// <summary>
        /// 音频采样率
        /// </summary>
        public int Sample_Rate { get; set; }

        /// <summary>
        /// 视频高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 视频宽
        /// </summary>
        public int Width { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum CodecTypeEnum
    {
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video = 0,
        /// <summary>
        /// 音频
        /// </summary>
        [Description("音频")]
        Audio = 1
    }
    /// <summary>
    /// 
    /// </summary>
    public enum CodecEnum
    {
        /// <summary>
        /// H.264
        /// </summary>
        [Description("H.264")]
        H264 = 0,
        /// <summary>
        /// H.265
        /// </summary>
        [Description("H.265")]
        H265,
        /// <summary>
        /// AAC
        /// </summary>
        [Description("AAC")]
        AAC,
        /// <summary>
        /// G711.A
        /// </summary>
        [Description("G711.A")]
        G711A,
        /// <summary>
        /// G711.U
        /// </summary>
        [Description("G711.U")]
        G711U
    }
}
