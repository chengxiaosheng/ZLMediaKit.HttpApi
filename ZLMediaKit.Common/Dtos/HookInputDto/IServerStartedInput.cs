using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    /// <summary>
    /// 服务器启动事件，可以用于监听服务器崩溃重启；此事件对回复不敏感。
    /// </summary>
    public interface IServerStartedInput : IServerConfig, IHookBase
    {
    }
    public class ServerStartedInput : ServerConfig, IServerStartedInput
    {
        public string MediaServerId { get; set; }

        public string ZlMediaKitAddress { get; set; }

        public int ZlMediaKitPort { get; set; }

        public IHookBase Clone() => this.MemberwiseClone() as IHookBase;
    }
}
