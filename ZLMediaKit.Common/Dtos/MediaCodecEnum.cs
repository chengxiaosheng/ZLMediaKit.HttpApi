using System.ComponentModel;

namespace ZLMediaKit.Common.Dtos
{
    public enum MediaCodecEnum
    {
        [Description("H264")]
        H264 = 0,
        [Description("H265")]
        H265 = 1,
        [Description("AAC")]
        AAC = 2,
        [Description("G711A")]
        G711A = 3,
        [Description("G711U")]
        G711U = 4
    }

    public enum MediaCodecTypeEnum
    {
        [Description("Video")]
        Video = 0,
        [Description("Audio")]
        Audio = 1,
    }
}
