using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiSetRecordSpeedResult : IApiResultBase
    {
        /// <summary>
        /// 0 成功， -1 失败，-2 流不存在
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }
    }

    public class ApiSetRecordSpeedResult : ApiResultBase, IApiSetRecordSpeedResult
    {
        /// 0 成功， -1 失败，-2 流不存在
        /// </summary>
        [JsonPropertyName("result")]
        public int Result { get; set; }
    }
}
