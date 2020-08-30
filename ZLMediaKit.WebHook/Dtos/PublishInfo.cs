using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class PublishInfo : EventBase
    {
        /// <summary>
        /// 流应用名
        /// </summary>
        public string App { get; set; }

        /// <summary>
        /// Tcp链接唯一Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 推流器Ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 推流Url参数
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// 推流器端口号
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        /// 推流的协议，可能是rtsp、rtmp、rtp
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 流Id
        /// </summary>
        public string Stream { get; set; }

        /// <summary>
        /// 流虚拟主机
        /// </summary>
        public string VHost { get; set; }
    }

    public class PublishResult : ResultBase
    {
        public PublishResult() { }
        public PublishResult(string err)
        {
            this.Code = -1;
            this.Msg = err;
            this.EnableHls = false;
            this.EnableMP4 = false;
            this.EnableRtxp = false;
        }

        /// <summary>
        /// 错误代码，0代表允许推流
        /// </summary>
        /// <value>Default Value : 0</value>
        public new int Code { get; set; } = 0;

        /// <summary>
        /// 是否转换成hls协议
        /// </summary>
        /// <value>Default Value : True</value>
        public bool EnableHls { get; set; } = true;

        /// <summary>
        /// 是否允许mp4录制
        /// </summary>
        /// <value>Default Value : False</value>
        public bool EnableMP4 { get; set; } = false;

        /// <summary>
        /// 是否允许转rtsp或rtmp
        /// </summary>
        /// <value>Default Value : True</value>
        public bool EnableRtxp { get; set; } = true;
    }
}
