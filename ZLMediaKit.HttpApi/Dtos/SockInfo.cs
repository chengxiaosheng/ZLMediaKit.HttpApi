using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class SockInfo
    {
        /// <summary>
        /// 本地IP
        /// </summary>
        public string Local_ip { get; set; }
        /// <summary>
        /// 本地端口
        /// </summary>
        public string Local_port { get; set; }
        /// <summary>
        /// 对端IP
        /// </summary>
        public string Peer_ip { get; set; }
        /// <summary>
        /// 对端端口
        /// </summary>
        public string Peer_port { get; set; }
        /// <summary>
        /// tcp 唯一标识
        /// </summary>
        public string Identifier { get; set; }
    }
}
