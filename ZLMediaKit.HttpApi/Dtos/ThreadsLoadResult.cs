using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    /// <summary>
    /// epoll(或select)线程负载以及延时
    /// </summary>
    public class ThreadsLoad
    {
        /// <summary>
        /// 该线程延时
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// 该线程负载，0 ~ 100
        /// </summary>
        public int Load { get; set; }
    }
}
