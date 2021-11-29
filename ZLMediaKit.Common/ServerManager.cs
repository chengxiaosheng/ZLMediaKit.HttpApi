using System;
using System.Collections.Generic;
using System.Linq;
using ZLMediaKit.Common.Dtos;

namespace ZLMediaKit.Common
{
    public interface IServerManager : IZLMediaKitSettings
    {

        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public DateTime? KeepaliveTime { get; set; }

        /// <summary>
        /// 服务器配置
        /// </summary>
        public IServerConfig ServerConfig { get; set; }



        /// <summary>
        /// 服务器心跳信息
        /// </summary>
        public IKeepalive Keepalive { get; set; }

        #region  static 

        public static Dictionary<string, IServerManager> Instances { get; set; } = new Dictionary<string, IServerManager>();

        public static IServerManager GetServerManager(IHookBase hookBase) => GetServerManager(hookBase?.MediaServerId);

        public static IServerManager GetServerManager(string mediaServerId = null)
        {
            if (string.IsNullOrEmpty(mediaServerId)) return null;
            Instances.TryGetValue(mediaServerId, out var serverManager);
            return serverManager;
        }

        public static IServerManager GetDefaultServerManager()
        {
            return Instances.FirstOrDefault(w => !string.IsNullOrEmpty(w.Value.ApiBaseUri)).Value;
        }

        /// <summary>
        /// 新增一个服务
        /// </summary>
        /// <param name="server"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void AddServer(IServerManager server)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (Instances.ContainsKey(server.MediaServerId)) throw new ArgumentException("此平台已存在");
            Instances.Add(server.MediaServerId, server);
        }

        public static void AddServer(IZLMediaKitSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            Instances[settings.MediaServerId] = settings as IServerManager;
        }

        /// <summary>
        /// 删除一个服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool RemoveServer(IServerManager server)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            return RemoveServer(server.MediaServerId);
        }

        /// <summary>
        /// 删除一个服务器
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool RemoveServer(string mediaServerId)
        {
            if (string.IsNullOrEmpty(mediaServerId)) throw new ArgumentNullException(nameof(mediaServerId));
            return IServerManager.Instances.Remove(mediaServerId);
        }



        /// <summary>
        /// 停止一个服务
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Stop(string mediaServerId) => GetServerManager(mediaServerId)?.Stop() ?? false;


        /// <summary>
        /// 停止一个服务
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool Stop(IServerManager server) => server?.Stop() ?? false;

        #endregion static

        /// <summary>
        /// 停止一个服务
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Stop();


    }
    /// <summary>
    /// 服务器管理
    /// </summary>
    public sealed class ServerManager : ZLMediaKitSettings, IServerManager
    {
        public IServerConfig ServerConfig { get; set; }

        public IKeepalive Keepalive { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? KeepaliveTime { get; set; }

        public string ServerAddress { get; set; }

        public bool Stop()
        {
            return true;
        }
    }
}
