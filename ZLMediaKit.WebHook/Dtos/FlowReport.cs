using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    /// <summary>
    /// 流量统计
    /// </summary>
    public class FlowReport : EventBase
    {
        /// <summary>
        /// 流应用名
        /// </summary>
        public string App { get; set; }

        /// <summary>
        /// tcp链接维持时间，单位秒
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 推流或播放url参数
        /// </summary>
        public string Params {get;set;}

        /// <summary>
        /// true为播放器，false为推流器
        /// </summary>
        public bool Player { get; set; }
        /// <summary>
        /// 播放或推流的协议，可能是rtsp、rtmp、http
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 流ID
        /// </summary>
        public string Stream { get; set; }

        /// <summary>
        /// 耗费上下行流量总和，单位字节
        /// </summary>
        public long TotalBytes { get; set; }

        /// <summary>
        /// 流虚拟主机
        /// </summary>
        public string Vhost { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 客户端端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// TCP链接唯一ID
        /// </summary>
        public string Id { get; set; }
    }
}
