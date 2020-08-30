using System;
using System.Collections.Generic;
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
            options?.Invoke(ZLMediaKitSettings.ZLMediaKitSetting);
            return services.AddTransient<ZLHttpClient>();
        }
    }
}
