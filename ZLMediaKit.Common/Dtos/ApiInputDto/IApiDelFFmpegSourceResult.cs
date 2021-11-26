using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiDelFFmpegSourceResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }
    }
    public interface IApiDelFFmpegSourceResult : IApiResultBase<IApiDelFFmpegSourceResultItem>
    {

    }

    public class ApiDelFFmpegSourceResultItem : IApiDelFFmpegSourceResultItem
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }
    }

    public class ApiDelFFmpegSourceResult: ApiResultBase<IApiDelFFmpegSourceResultItem>, IApiDelFFmpegSourceResult
    {

    }
}
