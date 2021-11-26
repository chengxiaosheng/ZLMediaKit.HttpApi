using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 流量统计事件
    /// </summary>
    public interface IReportFlowInput : IHookInputWithMediaBaseAndClient
    {
        /// <summary>
        /// tcp链接维持时间，单位秒
        /// </summary>
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// true为播放器，false为推流器
        /// </summary>
        [JsonPropertyName("player")]
        public bool Player { get; set; }

        /// <summary>
        /// 耗费上下行流量总和，单位字节
        /// </summary>
        [JsonPropertyName("totalBytes")]
        public int TotalBytes { get; set; }
    }

    public class ReportFlowInput : HookWithMediaBaseAndClient, IReportFlowInput
    {
        public int Duration { get;  set; }

        public bool Player { get;  set; }

        public int TotalBytes { get;  set; }
    }
}
