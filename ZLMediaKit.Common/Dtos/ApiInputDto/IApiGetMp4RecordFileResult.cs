using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiGetMp4RecordFileResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("paths")]
        public IList<string> Paths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("rootPath")]
        public string RootPath { get; set; }
    }

    public interface IApiGetMp4RecordFileResult : IApiResultListBase<IApiGetMp4RecordFileResultItem>
    {

    }

    public class ApiGetMp4RecordFileResultItem : IApiGetMp4RecordFileResultItem
    {
        /// 
        /// </summary>
        [JsonPropertyName("paths")]
        public IList<string> Paths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("rootPath")]
        public string RootPath { get; set; }
    }

    public class ApiGetMp4RecordFileResult : ApiResultListBase<IApiGetMp4RecordFileResultItem>, IApiGetMp4RecordFileResult
    {

    }
}
