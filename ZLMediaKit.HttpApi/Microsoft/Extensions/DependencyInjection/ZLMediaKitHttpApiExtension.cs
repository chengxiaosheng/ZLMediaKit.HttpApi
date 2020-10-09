using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZLMediaKit.HttpApi;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public static class ZLMediaKitHttpApiExtension
    {
        /// <summary>
        /// 添加ZLMediaKit Http Api Client
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddZLMediaKitHttpClient(this IServiceCollection services,Action<ZLMediaKitSettings> options)
        {
            ZLMediaKitSettings config = new ZLMediaKitSettings();
            options?.Invoke(config);
            if (string.IsNullOrEmpty(config.MediaServerId)) throw new InvalidDataException("MediaServerId 不能为空");
            ZLMediaKitSettings.ZLMediaKitSettingsDict[config.MediaServerId] = config;
            return services.AddTransient<ZLHttpClient>();
        }
        /// <summary>
        /// 添加ZLMediaKit Http Api Client
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddZLMediaKitHttpClient(this IServiceCollection services,Action<List<ZLMediaKitSettings>> options)
        {
            var configs = new List<ZLMediaKitSettings>();
            options.Invoke(configs);
            if(configs.Count == 0) throw new InvalidDataException("MediaKit HttpApi 服务配置不能为空");
            foreach(var item in configs)
            {
                ZLMediaKitSettings.ZLMediaKitSettingsDict[item.MediaServerId] = item;
            }
            return services.AddTransient<ZLHttpClient>();

        }

        /// <summary>
        /// 添加ZLMediaKit Http Api Client
        /// <para>不推荐使用此方法</para>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <remarks>用于与WebHook配合，通过WebHook主动产生ZLMediakitWebClient,需要调用ZLHttpClientRegister.Register 方法进行注册</remarks>
        public static IServiceCollection AddZLMediaKitHttpClient(this IServiceCollection services)
        {
            return services.AddTransient<ZLHttpClient>();
        }
    }
}
