using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookHttpAccessResult : IResultBase
    {
        /// <summary>
        /// 请固定返回0
        /// </summary>
        [JsonPropertyName("err")]
        public string Message { get; set; }

        /// <summary>
        /// 不允许访问的错误提示，允许访问请置空
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; }

        /// <summary>
        /// 该客户端能访问或被禁止的顶端目录，如果为空字符串，则表述为当前目录
        /// </summary>
        [JsonPropertyName("second")]
        public int Second { get; set; }

    }

    public class HookHttpAccessResult : ResultBase, IHookHttpAccessResult
    {
        /// <summary>
        /// 请固定返回0
        /// </summary>
        [JsonPropertyName("err")]
        public string Message { get; set; }

        /// <summary>
        /// 不允许访问的错误提示，允许访问请置空
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; } = "";

        /// <summary>
        /// 该客户端能访问或被禁止的顶端目录，如果为空字符串，则表述为当前目录
        /// </summary>
        [JsonPropertyName("second")]
        public int Second { get; set; } = 600;
    }
}
