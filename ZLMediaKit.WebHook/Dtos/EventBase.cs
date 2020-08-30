using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class EventBase
    {
        /// <summary>
        /// ZLM服务器IP
        /// </summary>
        public IPAddress ServerIp { get; set; }

        /// <summary>
        /// ZLM请求端口
        /// </summary>
        public int ServerPort { get; set; }
    }
}
