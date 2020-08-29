using System;
using System.Collections.Generic;
using System.Text;
using ZLMediaKit.HttpApi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZLMediaKitHttpApiExtension
    {
        public static IServiceCollection AddZLMediaKitHttpClient(this IServiceCollection services,Action<ZLMediaKitSettings> options)
        {
            options?.Invoke(ZLMediaKitSettings.ZLMediaKitSetting);
            return services.AddTransient<ZLHttpClient>();
        }
    }
}
