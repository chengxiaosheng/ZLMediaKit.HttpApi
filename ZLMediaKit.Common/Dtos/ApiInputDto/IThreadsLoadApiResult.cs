using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IThreadsLoadApiResultItem : IApiResultDataBase
    {
        /// <summary>
        /// 该线程延时
        /// </summary>
        [JsonPropertyName("delay")]
        public int Delay { get; set; }

        /// <summary>
        /// 该线程负载，0 ~ 100
        /// </summary>
        [JsonPropertyName("load")]
        public byte Load { get; set; }
    }

    public interface IThreadsLoadApiResult : IApiResultListBase<IThreadsLoadApiResultItem>
    {

    }

    public class ThreadsLoadApiResultItem : IThreadsLoadApiResultItem
    {
        /// <summary>
        /// 该线程延时
        /// </summary>
        [JsonPropertyName("delay")]
        public int Delay { get; set; }

        /// <summary>
        /// 该线程负载，0 ~ 100
        /// </summary>
        [JsonPropertyName("load")]
        public byte Load { get; set; }
    }

    public class ThreadsLoadApiResult : ApiResultListBase<IThreadsLoadApiResultItem> , IThreadsLoadApiResult
    {

    }
}
