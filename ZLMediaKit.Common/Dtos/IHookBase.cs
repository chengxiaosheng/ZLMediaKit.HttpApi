using System.Text.Json.Serialization;

namespace ZLMediaKit.Common.Dtos
{
    public interface IHookBase 
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        [JsonPropertyName("mediaServerId")]
        public string MediaServerId { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ZlMediaKitAddress { get; set; }

        public int ZlMediaKitPort { get; set; }

        public IHookBase Clone();
    }


    public interface IHookInputWithClient : IHookBase
    {
        /// <summary>
        /// 客户端IP
        /// </summary>
        [JsonPropertyName("ip")]
        public string IP { get; set; }
        /// <summary>
        /// TCP链接唯一ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [JsonPropertyName("port")]
        public int Port { get; set; }
        /// <summary>
        /// 推流或播放url参数
        /// </summary>
        [JsonPropertyName("params")]
        public string Params { get; set; }
    }

    public class HookBase : IHookBase
    {
        [JsonPropertyName("mediaServerId")]
        public string MediaServerId { get;  set; }

        public string ZlMediaKitAddress { get;  set; }

        public int ZlMediaKitPort { get;  set; }

        public IHookBase Clone()
        {
            return this.MemberwiseClone() as IHookBase;
        }
    }

    public class HookInputWithClient : HookBase, IHookInputWithClient
    {
        [JsonPropertyName("ip")]
        public string IP { get;  set; }
        [JsonPropertyName("id")]
        public string Id { get;  set; }
        [JsonPropertyName("port")]
        public int Port { get;  set; }
        [JsonPropertyName("params")]
        public virtual string Params { get;  set; }
    }

    public interface IHookInputWithMediaBaseAndClient : IHookInputWithClient,IMediaBase
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class HookWithMediaBaseAndClient : HookInputWithClient, IHookInputWithMediaBaseAndClient
    {
        public string Vhost { get; set; }

        public string App { get; set; }

        public string Schema { get; set; }

        public string Stream { get; set; }
    }

    public interface IHookInputWithMediaBase : IHookBase, IMediaBase
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class HookWithMediaBase : HookBase , IHookInputWithMediaBase
    {
        public string Vhost { get; set; }

        public string App { get; set; }

        public string Schema { get; set; }

        public string Stream { get; set; }
    }
}
