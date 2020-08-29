using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class StreamProxy
    {
        /// <summary>
        /// 动态添加rtsp/rtmp/hls拉流代理(只支持H264/H265/aac/G711负载)
        /// </summary>
        public string Key { get; set; }
    }

    public class DeleteStreamProxy
    {
        /// <summary>
        /// 成功与否
        /// </summary>
        public bool Flag { get; set; }
    }
}
