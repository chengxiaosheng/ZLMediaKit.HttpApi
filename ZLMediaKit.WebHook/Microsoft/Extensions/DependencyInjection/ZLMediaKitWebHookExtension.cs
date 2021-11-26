using ZLMediaKit.WebHook;
using ZLMediaKit.WebHook.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZLMediaKitWebHookExtension
    {
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
