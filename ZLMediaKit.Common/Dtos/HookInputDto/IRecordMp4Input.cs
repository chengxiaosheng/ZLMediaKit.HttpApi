using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 录制mp4完成后通知事件；此事件对回复不敏感
    /// </summary>
    public interface IRecordMp4Input : IHookInputWithMediaBase
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [JsonPropertyName("file_name")]
        public  string FileName { get;  }

        /// <summary>
        /// 文件绝对路径
        /// </summary>
        [JsonPropertyName("file_path")]
        public  string FilePath { get;  }
        /// <summary>
        /// 文件大小，单位字节
        /// </summary>
        [JsonPropertyName("file_size")]
        public  int FileSize { get;  }

        /// <summary>
        /// 文件所在目录路径
        /// </summary>
        [JsonPropertyName("folder")]
        public  string Folder { get;  }

        /// <summary>
        /// 开始录制时间戳
        /// </summary>
        [JsonPropertyName("start_time")]
        public  int StartTime { get;  }
        /// <summary>
        /// 录制时长，单位秒
        /// </summary>
        [JsonPropertyName("time_len")]
        public  int TimeLen { get;}
        /// <summary>
        /// http/rtsp/rtmp点播相对url路径
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get;  }
    }
    /// <summary>
    /// 录制mp4完成后通知事件；此事件对回复不敏感
    /// </summary>
    public class RecordMp4Input : HookWithMediaBase, IRecordMp4Input
    {
        public string FileName { get;  set; }

        public string FilePath { get;  set; }

        public int FileSize { get;  set; }

        public string Folder { get;  set; }

        public int StartTime { get;  set; }

        public int TimeLen { get;  set; }

        public string Url { get;  set; }
    }
}
