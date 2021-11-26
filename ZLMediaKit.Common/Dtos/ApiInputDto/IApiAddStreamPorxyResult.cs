using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiAddStreamProxyResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 流的唯一标识
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    public interface IApiAddStreamPorxyResult : IApiResultBase<IApiAddStreamProxyResultItem> 
    {

    }

    public class ApiAddStreamPorxyResultItem : IApiAddStreamProxyResultItem
    {
        /// <summary>
        /// 流的唯一标识
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    public class ApiAddStreamPorxyResult: ApiResultBase<IApiAddStreamProxyResultItem>, IApiAddStreamPorxyResult
    {

    }
}
