using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class RtspAuthInfo : EventBase
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
        /// 请求的密码是否必须为明文(base64鉴权需要明文密码)
        /// </summary>
        public bool Must_no_encrypt { get; set; }
        /// <summary>
        /// rtsp url参数
        /// </summary>
        public string Params { get; set; }
        /// <summary>
        /// rtsp播放器端口号
        /// </summary>
        public ushort Port { get; set; }
        /// <summary>
        /// rtsp播放鉴权加密realm
        /// </summary>
        public string Realm { get; set; }
        /// <summary>
        /// rtsp或rtsps
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 流ID
        /// </summary>
        public string Stream { get; set; }
        /// <summary>
        /// 播放用户名
        /// </summary>
        public string User_name { get; set; }
        /// <summary>
        /// 流虚拟主机
        /// </summary>
        public string Vhost { get; set; }
        
    }

    public class RtspAuthResult : ResultBase
    {
        /// <summary>
        /// 错误代码，0代表允许播放
        /// </summary>
        public new int Code { get; set; }
        /// <summary>
        /// 用户密码是否已加密
        /// </summary>
        public bool Encrypted { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Passwd { get; set; }
    }
}
