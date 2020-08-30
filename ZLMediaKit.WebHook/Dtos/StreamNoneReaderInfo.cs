using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class StreamNoneReaderInfo : EventBase
    {
        /// <summary>
        /// 流应用名
        /// </summary>
        public string App { get; set; }
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

    public class StreamNoneReaderInfoResult : ResultBase
    {
        /// <summary>
        /// 是否关闭推流或拉流
        /// </summary>
        public bool Close { get; set; } = true;
    }
}
