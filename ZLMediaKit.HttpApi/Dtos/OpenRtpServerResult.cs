using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenRtpServerResult
    {
        /// <summary>
        /// 执行结果类型枚举
        /// </summary>
        public ApiCodeEnum Code { get; set; }

        /// <summary>
        /// 接收端口，方便获取随机端口号
        /// </summary>
        public int Port { get; set; }
    }
}
