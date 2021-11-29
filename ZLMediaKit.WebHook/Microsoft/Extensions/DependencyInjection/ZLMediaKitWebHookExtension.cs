using ZLMediaKit.WebHook;
using ZLMediaKit.WebHook.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ZLMediaKitWebHookExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="routeOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediaKitWebHook(this IServiceCollection services, APIRouteOptions routeOptions)
        {
            services.AddHttpContextAccessor();

            services.AddTransient<ZLMediaKitWebHookServcies>();
            services.AddMvcCore(options =>
            {
                options.Conventions.Add(new ZLMediaKitWebHookApplicationModelConvention(routeOptions));

            }).PartManager.FeatureProviders.Add(new ZLMediaKitControllerFeatureProvider());

            return services;
        }
    }
}
