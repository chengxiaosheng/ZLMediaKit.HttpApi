using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IWorkThreadsLoadApiResultItem : IApiResultDataBase
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

    public interface IWorkThreadsLoadApiResult : IApiResultListBase<IWorkThreadsLoadApiResultItem>
    {

    }

    public class WorkThreadsLoadApiResultItem : IWorkThreadsLoadApiResultItem
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
    public class WorkThreadsLoadApiResult : ApiResultListBase<IWorkThreadsLoadApiResultItem>, IWorkThreadsLoadApiResult
    {

    }
}
