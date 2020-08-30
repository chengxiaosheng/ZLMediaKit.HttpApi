using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class StreamChangedInfo : EventBase
    {
        /// <summary>
        /// 流应用名
        /// </summary>
        public string App { get; set; }
        /// <summary>
        /// 流注册或注销
        /// </summary>
        public bool Regist { get; set; }
        /// <summary>
        /// rtsp或rtmp
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 流ID
        /// </summary>
        public string Stream { get; set; }
        /// <summary>
        /// 流虚拟主机
        /// </summary>
        public string Vhost { get; set; }
        
    }
}
