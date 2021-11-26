using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookRtspAuthResult : IHookCommonResult
    {
        /// <summary>
        /// 用户密码是否已加密
        /// </summary>
        [JsonPropertyName("encrypted")]
        public bool Encrypted { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonPropertyName("passwd")]
        public string Passwd { get; set; }
    }

    public class HookRtspAuthResult : HookCommonResult, IHookRtspAuthResult
    {
        /// <summary>
        /// 用户密码是否已加密
        /// </summary>
        [JsonPropertyName("encrypted")]
        public bool Encrypted { get; set; } = false;

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonPropertyName("passwd")]
        public string Passwd { get; set; }
    }
}
