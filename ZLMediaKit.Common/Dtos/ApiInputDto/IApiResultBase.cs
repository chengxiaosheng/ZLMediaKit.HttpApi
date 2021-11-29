using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.ApiInputDto
{
    public interface IApiResultBase
    {
        [JsonPropertyName("code")]
        public ApiResultCodeType Code { get; set; }

        [JsonPropertyName("msg")]
        public string Messag { get; set; }
    }

    public interface IApiResultDataBase
    {

    }

    public interface IApiResultBase<T> : IApiResultBase //where T : IApiResultDataBase
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public interface IApiResultListBase<T> : IApiResultBase where T : IApiResultDataBase
    {
        [JsonPropertyName("data")]
        public IList<T> Data { get; set; }
    }

    public interface IApiResultDictBase<T,T1> : IApiResultBase
    { 
        [JsonPropertyName("data")]
        public Dictionary<T,T1> Data { get; set; }
    }

    public class ApiResultBase: IApiResultBase
    {
        [JsonPropertyName("code")]
        public ApiResultCodeType Code { get; set; }

        [JsonPropertyName("msg")]
        public string Messag { get; set; }
    }

    public class ApiResultBase<T> : ApiResultBase, IApiResultBase<T> //where T : IApiResultDataBase
    {
        public T Data { get; set; }
    }

    public class ApiResultListBase<T> : ApiResultBase, IApiResultListBase<T> where T : IApiResultDataBase
    {
        [JsonPropertyName("data")]
        public IList<T> Data { get; set; }
    }

    public class ApiResultDictBase<T,T1> : ApiResultBase, IApiResultDictBase<T,T1> 
    {
        [JsonPropertyName("data")]
        public Dictionary<T,T1> Data { get; set; }
    }





    public enum ApiResultCodeType
    {
        /// <summary>
        /// 代码抛异常
        /// </summary>
        Exception = -400,
        /// <summary>
        /// 参数不合法
        /// </summary>
        InvalidArgs = -300,//
        /// <summary>
        /// sql执行失败
        /// </summary>
        SqlFailed = -200,//
        /// <summary>
        /// 鉴权失败
        /// </summary>
        AuthFailed = -100,//
        /// <summary>
        /// 业务代码执行失败
        /// </summary>
        OtherFailed = -1,//，
        /// <summary>
        /// 执行成功
        /// </summary>
        Success = 0//
    }
}
