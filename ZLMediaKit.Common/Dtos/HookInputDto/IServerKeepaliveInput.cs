using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 服务器定时上报时间，上报间隔可配置，默认10s上报一次
    /// </summary>
    public interface IServerKeepaliveInput : IHookBase,IKeepalive
    {
        [JsonPropertyName("data")]
        public IKeepalive Data { get;  }
    }

    /// <summary>
    /// 服务器定时上报时间，上报间隔可配置，默认10s上报一次
    /// </summary>
    public class ServerKeepaliveInput : Keepalive, IServerKeepaliveInput
    {
        public IKeepalive Data { get;  set; }

        public string MediaServerId { get;  set; }

        public string ZlMediaKitAddress { get; set; }

        public int ZlMediaKitPort { get; set; }

        public virtual IHookBase Clone() => this.MemberwiseClone() as ServerKeepaliveInput;
    }
}
