using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiClonseStreamsResult : IApiResultBase
    {
        /// <summary>
        /// 筛选条件命中数
        /// </summary>
        [JsonPropertyName("count_hit")]
        public int CountHit { get; set; }

        /// <summary>
        /// 被关闭的流个数，可能小于count_hit
        /// </summary>
        [JsonPropertyName("count_closed")]
        public int CountClose { get; set; }
    }

    public class ApiClonseStreamsResult : ApiResultBase, IApiClonseStreamsResult
    {
        /// <summary>
        /// 筛选条件命中数
        /// </summary>
        [JsonPropertyName("count_hit")]
        public int CountHit { get; set; }

        /// <summary>
        /// 被关闭的流个数，可能小于count_hit
        /// </summary>
        [JsonPropertyName("count_closed")]
        public int CountClose { get; set; }
    }
}
