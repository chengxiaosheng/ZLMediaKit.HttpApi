using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookCommonResult : IResultBase
    {
        /// <summary>
        /// 返回消息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; }
    }

    public class HookCommonResult : ResultBase, IHookCommonResult
    {
        /// <summary>
        /// 返回消息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Message { get; set; }
    }
}
