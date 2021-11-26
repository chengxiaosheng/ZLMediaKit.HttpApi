using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface ISetServerConfigResult : IApiResultBase
    {
        /// <summary>
        /// 配置项变更个数
        /// </summary>
        [JsonPropertyName("changed")]
        public int Changed { get; set; }
    }

    public class SetServerConfigResult : ApiResultBase, ISetServerConfigResult
    {
        /// <summary>
        /// 配置项变更个数
        /// </summary>
        [JsonPropertyName("changed")]
        public int Changed { get; set; }
    }
}
