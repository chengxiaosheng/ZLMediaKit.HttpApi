using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class ServerConfigResult
    {
        public ApiCodeEnum Code { get; set; }

        /// <summary>
        /// 配置项变更个数
        /// </summary>
        public int Changed { get; set; }

        public string Msg { get; set; }
    }
}
