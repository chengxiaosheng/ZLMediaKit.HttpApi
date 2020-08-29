using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class CloseStreamResult : CloseBaseResult
    {
        /// <summary>
        /// 被关闭的流个数，可能小于count_hit
        /// </summary>
        public int Count_Closed { get; set; }
    }
}
