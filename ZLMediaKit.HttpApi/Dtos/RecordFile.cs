using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.HttpApi.Dtos
{
    public class RecordFile
    {
        /// <summary>
        /// 文件或文件夹名称列表
        /// </summary>
        public List<string> Paths { get; set; }

        /// <summary>
        /// 文件或文件夹所在目录的目录完整路径
        /// </summary>
        public string RootPath { get; set; }
    }
}
