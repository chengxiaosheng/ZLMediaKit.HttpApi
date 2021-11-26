using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApKillSessionsResult : IApiResultBase
    {
        /// <summary>
        /// 筛选命中客户端个数
        /// </summary>
        [JsonPropertyName("count_hit")]
        public int CountHit { get; set; }
    }

    public class ApKillSessionsResult : ApiResultBase, IApKillSessionsResult
    {
        /// <summary>
        /// 筛选命中客户端个数
        /// </summary>
        [JsonPropertyName("count_hit")]
        public int CountHit { get; set; }
    }
}
