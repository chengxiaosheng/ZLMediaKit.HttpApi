using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiStopRecordResult : IApiResultBase
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }
    }

    public class ApiStopRecordResult: ApiResultBase, IApiStopRecordResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }
    }
}
