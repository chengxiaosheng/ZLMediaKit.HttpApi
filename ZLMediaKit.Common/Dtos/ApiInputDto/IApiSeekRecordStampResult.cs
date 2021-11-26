using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiSeekRecordStampResult : IApiResultBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }
    }

    public class ApiSeekRecordStampResult: ApiResultBase, IApiSeekRecordStampResult
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }
    }
}
