using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    /// <summary>
    /// 访问http文件服务器上hls之外的文件的返回结果
    /// </summary>
    public interface IHookHttpAccessResult : IResultBase
    {
        /// <summary>
        /// 不允许访问的错误提示，允许访问请置空
        /// </summary>
        [JsonPropertyName("err")]
        public string Message { get; set; }

        /// <summary>
        /// 该客户端能访问或被禁止的顶端目录，如果为空字符串，则表述为当前目录
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; }

        /// <summary>
        /// 本次授权结果的有效期，单位秒
        /// </summary>
        [JsonPropertyName("second")]
        public int Second { get; set; }

    }
    /// <summary>
    /// 访问http文件服务器上hls之外的文件的返回结果
    /// </summary>
    public class HookHttpAccessResult : ResultBase, IHookHttpAccessResult
    {
        /// <summary>
        /// 请固定返回0
        /// </summary>
        [JsonPropertyName("code")]
        public override int Code => 0;

        /// <summary>
        /// 不允许访问的错误提示，允许访问请置空
        /// </summary>
        [JsonPropertyName("err")]
        public string Message { get; set; }

        /// <summary>
        /// 该客户端能访问或被禁止的顶端目录，如果为空字符串，则表述为当前目录
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; } = "";

        /// <summary>
        /// 本次授权结果的有效期，单位秒
        /// </summary>
        [JsonPropertyName("second")]
        public int Second { get; set; } = 600;
    }
}
