using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class ShellLoginInfo : EventBase
    {
        /// <summary>
        /// TCP链接唯一ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// telnet 终端ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// telnet 终端登录用户密码
        /// </summary>
        public bool Passwd { get; set; }
        /// <summary>
        /// telnet 终端端口号
        /// </summary>
        public ushort Port { get; set; }
        /// <summary>
        /// telnet 终端登录用户名
        /// </summary>
        public string User_name { get; set; }
    }

    public class ShellLonginResult : ResultBase
    {
        public ShellLonginResult()
        {
            this.Msg = "禁止访问";
        }
        public new int Code { get; set; } = -1;

    }
}
