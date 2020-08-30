using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class HttpAccess : EventBase
    {
        /// <summary>
        /// http客户端请求header
        /// </summary>
        public Dictionary<string, string> Header { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// TCP链接唯一ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 客户端Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// http 访问路径是文件还是目录
        /// </summary>
        public bool Is_Dir { get; set; }

        /// <summary>
        /// http url参数
        /// </summary>
        public string Params { get; set; }
        /// <summary>
        /// http客户端端口号
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        /// 请求访问的文件或目录
        /// </summary>
        public string Path { get; set; }


    }

    public class HttpAccessResult : ResultBase
    {
        /// <summary>
        /// 不允许访问的错误提示，允许访问请置空
        /// </summary>
        public string Err { get; set; }
        /// <summary>
        /// 该客户端能访问或被禁止的顶端目录，如果为空字符串，则表述为当前目录
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 本次授权结果的有效期，单位秒
        /// </summary>
        public int Second { get; set; }
    }
}
