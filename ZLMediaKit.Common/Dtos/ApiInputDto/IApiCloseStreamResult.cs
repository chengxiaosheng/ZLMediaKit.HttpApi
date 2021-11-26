using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiCloseStreamResult : IApiResultBase
    {
        /// <summary>
        /// 0:成功，-1:关闭失败，-2:该流不存在
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }
    }

    public class ApiCloseStreamResult : ApiResultBase , IApiCloseStreamResult
    {
        /// <summary>
        /// 0:成功，-1:关闭失败，-2:该流不存在
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }
    }
}
