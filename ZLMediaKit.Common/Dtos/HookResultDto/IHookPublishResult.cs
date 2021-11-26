using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookPublishResult : IHookCommonResult
    {
        /// <summary>
        /// 是否转换成hls协议
        /// </summary>
        [JsonPropertyName("enableHls")]
        public bool EnableHls { get; set; }

        /// <summary>
        /// 是否允许mp4录制
        /// </summary>
        [JsonPropertyName("enableMP4")]
        public bool EnableMP4 { get; set; }
    }

    public class HookPublishResult : HookCommonResult, IHookPublishResult
    {
        /// <summary>
        /// 是否转换成hls协议
        /// </summary>
        [JsonPropertyName("enableHls")]
        public bool EnableHls { get; set; } = false;

        /// <summary>
        /// 是否允许mp4录制
        /// </summary>
        [JsonPropertyName("enableMP4")]
        public bool EnableMP4 { get; set; } = false;
    }
}
