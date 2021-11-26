using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookRtspRealmResult : IResultBase
    {
        /// <summary>
        /// 该rtsp流是否需要rtsp专有鉴权，空字符串代码不需要鉴权
        /// </summary>
        [JsonPropertyName("realm")]
        public string Realm { get; set; }
    }

    public class HookRtspRealmResult : ResultBase, IHookRtspRealmResult
    {
        /// <summary>
        /// 该rtsp流是否需要rtsp专有鉴权，空字符串代码不需要鉴权
        /// </summary>
        [JsonPropertyName("realm")]
        public string Realm { get; set; }
    }
}
