using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// rtsp/rtmp流注册或注销时触发此事件；此事件对回复不敏感。
    /// </summary>
    public interface IStreamChangedInput : IHookBase, IMediaInfo
    {
       
    }

    /// <summary>
    /// rtsp/rtmp流注册或注销时触发此事件；此事件对回复不敏感。
    /// </summary>
    public class StreamChangedInput : MediaInfo, IStreamChangedInput
    {
        public string MediaServerId { get; set; }

        public string ZlMediaKitAddress { get; set; }

        public int ZlMediaKitPort { get; set; }

        public IHookBase Clone() => this.MemberwiseClone() as StreamChangedInput;
    }
}
