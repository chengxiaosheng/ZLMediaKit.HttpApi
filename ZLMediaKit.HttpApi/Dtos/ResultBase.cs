using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class ResultBase
    {
        /// <summary>
        /// 执行结果类型枚举
        /// </summary>
        public ApiCodeEnum Code { get; set; }

        /// <summary>
        /// 信息提示
        /// </summary>
        public string Msg { get; set; }

        public int Result { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// false:未录制,true:正在录制
        /// </summary>
        public bool Status { get; set; }
    }

    public class ResultBase<T> :ResultBase  where T: new ()
    {
        public T Data { get; set; }
    }

    public class ResultListBase<T> : ResultBase where T : new ()
    {
        public List<T> Data { get; set; }
    }

    /// <summary>
    /// 执行结果类型枚举
    /// </summary>
    [Description("执行结果类型枚举")]
    public enum ApiCodeEnum
    {
        /// <summary>
        /// 代码抛异常
        /// </summary>
        [Description("代码抛异常")]
        Exception = -400,
        /// <summary>
        /// 参数不合法
        /// </summary>
        [Description("参数不合法")]
        InvalidArgs = -300,
        /// <summary>
        /// sql执行失败
        /// </summary>
        [Description("sql执行失败")]
        SqlFailed = -200,
        /// <summary>
        /// 鉴权失败
        /// </summary>
        [Description("鉴权失败")]
        AuthFailed = -100,
        /// <summary>
        /// 业务代码执行失败
        /// </summary>
        [Description("业务代码执行失败")]
        OtherFailed = -1,
        /// <summary>
        /// 执行成功
        /// </summary>
        [Description("执行成功")]
        Success = 0
    }
}
