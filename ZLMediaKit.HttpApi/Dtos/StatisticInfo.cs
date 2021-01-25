using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    /// <summary>
    /// 对象统计
    /// </summary>
    public class StatisticInfo
    {
        /// <summary>
        /// 媒体源数量
        /// </summary>
        public virtual UInt64 MediaSource { get; internal set; }

        /// <summary>
        /// 解
        /// </summary>
        public virtual UInt64 MultiMediaSourceMuxer { get; internal set; }

        /// <summary>
        /// TCP 服务器数量
        /// </summary>
        public virtual UInt64 TcpServer { get; internal set; }

        /// <summary>
        /// TCP 会话数量
        /// </summary>
        public virtual UInt64 TcpSession { get; internal set; }

        /// <summary>
        /// TCP 客户端数量
        /// </summary>
        public virtual UInt64 TcpClient { get; internal set; }
        /// <summary>
        /// Socket对象
        /// </summary>
        public virtual UInt64 Socket { get;  set; }

        /// <summary>
        /// 帧实现
        /// </summary>
        public virtual UInt64 FrameImp { get;  set; }

        /// <summary>
        /// 帧抽象接口
        /// </summary>
        public virtual UInt64 Frame { get;  set; }

        /// <summary>
        /// 缓存基类
        /// </summary>
        public virtual UInt64 Buffer { get;  set; }

        /// <summary>
        /// 指针式缓存对象
        /// </summary>
        public virtual UInt64 BufferRaw { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual UInt64 BufferLikeString { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual UInt64 BufferList { get;  set; }
    }
}
