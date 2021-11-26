
using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiAddStreamPusherProxyResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 流的唯一标识
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    public interface IApiAddStreamPusherProxyResult : IApiResultBase<IApiAddStreamPusherProxyResultItem>
    {

    }

    public class ApiAddStreamPusherProxyResultItem: IApiAddStreamPusherProxyResultItem
    {
        /// <summary>
        /// 流的唯一标识
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    public class ApiAddStreamPusherProxyResult: ApiResultBase<IApiAddStreamPusherProxyResultItem>, IApiAddStreamPusherProxyResult
    {

    }
}
