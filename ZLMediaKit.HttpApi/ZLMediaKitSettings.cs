using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi
{
    /// <summary>
    /// 
    /// </summary>
    public class ZLMediaKitSettings
    {
        internal static  Dictionary<string, ZLMediaKitSettings>  ZLMediaKitSettingsDict =  new Dictionary<string, ZLMediaKitSettings>();
        internal string HttpUrl => $"{HttpSchema.ToString()}://{IpAddress}:{ApiPort}/index/api/";
        
        /// <summary>
        /// Api Ip
        /// </summary>
        public string IpAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// API 端口
        /// </summary>
        public int ApiPort { get; set; } = 80;

        /// <summary>
        /// Api Secret
        /// </summary>
        public string ApiSecret { get; set; } = "035c73f7-bb6b-4889-a715-d9eb2d1925cc";

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; } = 2000;

        /// <summary>
        /// 服务器唯一id，用于触发hook时区别是哪台服务器
        /// </summary>
        public string MediaServerId { get; set; } = "your_server_id";

        /// <summary>
        /// Http/Https
        /// </summary>
        public HttpSchema HttpSchema { get; set; } = HttpSchema.http;

    }

    /// <summary>
    /// Http Schema
    /// </summary>
    public enum HttpSchema:byte
    {
        /// <summary>
        /// Http
        /// </summary>
        http = 0,
        /// <summary>
        /// Https
        /// </summary>
        https
    }
}
