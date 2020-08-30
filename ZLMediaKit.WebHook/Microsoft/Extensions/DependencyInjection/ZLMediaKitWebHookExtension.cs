using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using ZLMediaKit.WebHook;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZLMediaKitWebHookExtension
    {
        public static IServiceCollection AddMediaKitWebHook(this IServiceCollection services, APIRouteOptions routeOptions)
        {
            services.AddHttpContextAccessor();

            if (string.IsNullOrEmpty(routeOptions.CorsPolicyName))
            {
                routeOptions.CorsPolicyName = "ZLMWebHook";
                services.AddCors(options =>
                {
                    options.AddPolicy(routeOptions.CorsPolicyName, builder =>
                    {
                        builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS");
                    });
                });
            }

            services.AddMvcCore(options =>
            {
                options.Conventions.Add(new ZLMediaKitWebHookApplicationModelConvention(routeOptions));
            }).PartManager.FeatureProviders.Add(new ZLMediaKitControllerFeatureProvider());
            

            return services;
        }
    }
}
