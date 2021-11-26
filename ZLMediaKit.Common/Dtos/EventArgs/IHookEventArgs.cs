using Microsoft.AspNetCore.Http;

namespace ZLMediaKit.Common.Dtos.EventArgs
{
    public interface IHookEventArgs<T> :IEvnetBase where T:IHookBase
    {
        public HttpContext HttpContext { get; set; }

        /// <summary>
        /// 事件承载数据
        /// </summary>
        public T Payload { get; set; }
    }

    public class HookEventArgs<T> : EvnetBase, IHookEventArgs<T> where T : IHookBase
    {
        public HookEventArgs(HttpContext http, T payload) : base(IServerManager.GetServerManager(payload.MediaServerId))
        {
            this.HttpContext = http;
            this.Payload = payload;
        }
        /// <summary>
        /// Http 数据
        /// </summary>
        public HttpContext HttpContext { get; set; }

        /// <summary>
        /// 事件承载数据
        /// </summary>
        public T Payload { get; set; }
    }
}
