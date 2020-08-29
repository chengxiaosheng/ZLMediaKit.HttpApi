using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi
{
    public class ZLMediaKitSettings
    {
        internal static ZLMediaKitSettings ZLMediaKitSetting = new ZLMediaKitSettings();
        internal static string HttpUrl => $"http://{ZLMediaKitSetting.IpAddress}:{ZLMediaKitSetting.ApiPort}/index/api/";
        
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
    }
}
