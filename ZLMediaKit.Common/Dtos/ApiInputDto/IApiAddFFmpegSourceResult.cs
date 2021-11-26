using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiAddFFmpegSourceResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 流的唯一标识
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
    public interface IApiAddFFmpegSourceResult: IApiResultBase<IApiAddFFmpegSourceResultItem>
    {
    }
    public class ApiAddFFmpegSourceResultItem : IApiAddFFmpegSourceResultItem
    {
        /// <summary>
        /// 流的唯一标识
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    public class ApiAddFFmpegSourceResult : ApiResultBase<IApiAddFFmpegSourceResultItem>, IApiAddFFmpegSourceResult
    {

    }
}
