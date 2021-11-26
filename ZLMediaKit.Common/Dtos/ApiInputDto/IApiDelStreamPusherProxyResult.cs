using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiDelStreamPusherProxyResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }
    }
    public interface IApiDelStreamPusherProxyResult : IApiResultBase<IApiDelStreamPusherProxyResultItem>
    {

    }
    public class ApiDelStreamPusherProxyResultItem : IApiDelStreamPusherProxyResultItem
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }
    }

    public class ApiDelStreamPusherProxyResult : ApiResultBase<IApiDelStreamPusherProxyResultItem>, IApiDelStreamPusherProxyResult
    {

    }
}
