using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class CloseBaseResult : ResultBase
    {
        /// <summary>
        /// 筛选命中的流个数
        /// </summary>
        public int Count_Hit { get; set; }

        /// <summary>
        /// 是否找到记录并关闭
        /// </summary>
        public int Hit { get; set; }
    }
}
