using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiDelStreamProxyResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }
    }
    public interface IApiDelStreamProxyResult : IApiResultBase<IApiDelStreamProxyResultItem>
    {

    }

    public class ApiDelStreamProxyResultItem : IApiDelStreamProxyResultItem
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }
    }

    public class ApiDelStreamProxyResult :ApiResultBase<IApiDelStreamProxyResultItem>, IApiDelStreamProxyResult
    {

    }
}
