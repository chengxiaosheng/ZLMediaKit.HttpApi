using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class RtpInfoResult :ResultBase
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        public bool Exist { get; set; }
        /// <summary>
        /// 推流客户端IP
        /// </summary>
        public string Peer_Ip { get; set; }

        /// <summary>
        /// 客户端端口号
        /// </summary>
        public int Peer_Port { get; set; }

        /// <summary>
        /// 本地监听的网卡ip
        /// </summary>
        public string Local_Ip { get; set; }

        /// <summary>
        /// 本地监听的网卡端口
        /// </summary>
        public int Local_Port { get; set; }
    }
}
