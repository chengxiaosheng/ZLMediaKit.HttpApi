using System;
using System.Collections.Generic;
using System.Text;

namespace ZLMediaKit.WebHook.Dtos
{
    public class RecordInfo : EventBase
    {
        /// <summary>
        /// 录制的流应用名
        /// </summary>
        public string App { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string File_name { get; set; }
        /// <summary>
        /// 文件绝对路径
        /// </summary>
        public string File_path { get; set; }
        /// <summary>
        /// 文件大小，单位字节
        /// </summary>
        public long File_size { get; set; }
        /// <summary>
        /// 文件所在目录路径
        /// </summary>
        public string Folder { get; set; }
        /// <summary>
        /// 开始录制事件戳
        /// </summary>
        public long Start_time { get; set; }

        public DateTime StartTime => DateTimeOffset.FromUnixTimeSeconds(Start_time*1000).DateTime;
        /// <summary>
        /// 录制的流ID
        /// </summary>
        public string Stream { get; set; }
        /// <summary>
        /// 录制时长，单位秒
        /// </summary>
        public int Time_len { get; set; }
        /// <summary>
        /// http/rtsp/rtmp点播相对url路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 流虚拟主机
        /// </summary>
        public string Vhost { get; set; }
    }

}
