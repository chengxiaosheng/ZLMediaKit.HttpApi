using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookResultDto
{
    public interface IHookStreamNonReaderResult : IResultBase
    {
        /// <summary>
        /// 是否关闭推流或拉流
        /// </summary>
        [JsonPropertyName("close")]
        public bool Close { get; set; }
    }

    public class HookStreamNonReaderResult : ResultBase, IHookStreamNonReaderResult
    {
        /// <summary>
        /// 是否关闭推流或拉流
        /// </summary>
        [JsonPropertyName("close")]
        public bool Close { get; set; } = true;
    }
}
