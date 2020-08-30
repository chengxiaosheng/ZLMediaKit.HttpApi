using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class RtpServer
    {
        /// <summary>
        /// 绑定的端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 绑定的流ID
        /// </summary>
        public string Stream_Id { get; set; }
    }
}
