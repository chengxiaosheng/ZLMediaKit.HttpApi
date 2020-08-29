using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class ClientSession
    {
        /// <summary>
        /// 该tcp链接唯一id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 本机网卡ip
        /// </summary>
        public string Local_Ip { get; set; }
        /// <summary>
        /// 本机端口号	(这是个rtmp播放器或推流器)
        /// </summary>
        public int Local_Port { get; set; }
        /// <summary>
        /// 客户端ip
        /// </summary>
        public string Peer_Ip { get; set; }
        /// <summary>
        /// 客户端端口号
        /// </summary>
        public string Peer_Port { get; set; }
        /// <summary>
        /// 客户端TCPSession typeid
        /// </summary>
        public string TypeId { get; set; }
    }
}
