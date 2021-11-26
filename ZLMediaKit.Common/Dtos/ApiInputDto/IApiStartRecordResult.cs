using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiStartRecordResult : IApiResultBase
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }
    }
    public class ApiStartRecordResult : ApiResultBase, IApiStartRecordResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }
    }
}
