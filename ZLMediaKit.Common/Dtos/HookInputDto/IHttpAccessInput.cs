using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// http 访问路径是文件还是目录
    /// </summary>
    public interface IHttpAccessInput : IHookInputWithClient
    {
        /// <summary>
        /// http 访问路径是文件还是目录
        /// </summary>
        [JsonPropertyName("is_dir")]
        public bool IsDir { get; set; }
        /// <summary>
        /// 请求访问的文件或目录
        /// </summary>
        [JsonPropertyName("path")]
        public  string Path { get; set; }
        /// <summary>
        /// http header
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        void InitHeader(Dictionary<string,string> form);
    }

    /// <summary>
    /// http 访问路径是文件还是目录
    /// </summary>
    public class HttpAccessInput : HookInputWithClient, IHttpAccessInput
    {
        public bool IsDir { get;  set; }

        public string Path { get;  set; }

        public Dictionary<string, string> Headers { get;  set; }

        public void InitHeader(Dictionary<string, string> form)
        {
            this.Headers = form.Select(s => new { Key = s.Key.Replace("header.", String.Empty, StringComparison.OrdinalIgnoreCase), Value = s.Value }).ToDictionary(s => s.Key, v => v.Value);
        }
    }


}
