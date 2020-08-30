using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class RtspRealmInfo : EventBase
    {
        /// <summary>
        /// 流应用名
        /// </summary>
        public string App { get; set; }
        /// <summary>
        /// TCP链接唯一ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// rtsp播放器ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 播放rtsp url参数
        /// </summary>
        public string Params	 { get; set; }
        /// <summary>
        /// rtsp播放器端口号
        /// </summary>
        public ushort Port { get; set; }
        /// <summary>
        /// rtsp或rtsps
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

    public class RtspRealmInfoResult : ResultBase
    {
        /// <summary>
        /// 该rtsp流是否需要rtsp专有鉴权，空字符串代码不需要鉴权
        /// </summary>
        public string Realm { get; set; }
    }
}
