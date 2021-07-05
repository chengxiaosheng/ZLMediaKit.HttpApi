using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class StartSendRtpResult
    {
        /// <summary>
        /// 执行结果类型枚举
        /// </summary>
        public ApiCodeEnum Code { get; set; }

        /// <summary>
        /// 使用的本地端口号 
        /// </summary>
        public int Local_port { get; set; }
    }
}
