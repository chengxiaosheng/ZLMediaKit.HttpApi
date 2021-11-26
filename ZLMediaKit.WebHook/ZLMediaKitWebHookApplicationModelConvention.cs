using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using ZLMediaKit.WebHook.Services;

namespace ZLMediaKit.WebHook
{
    internal class ZLMediaKitWebHookApplicationModelConvention : IApplicationModelConvention
    {
        private readonly APIRouteOptions _routeOptions;

        public ZLMediaKitWebHookApplicationModelConvention(APIRouteOptions routeOptions)
        {
            this._routeOptions = routeOptions;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                if (controller.ControllerType == typeof(ZLMediaKitWebHookServcies))
                {
                    controller.ApiExplorer.GroupName = "ZLMediaKit-WebHook";
                    controller.ApiExplorer.IsVisible = true;
                    foreach (var action in controller.Actions)
                    {
                        action.ApiExplorer.IsVisible = true;
                        var actionName = action.ActionName;
                        foreach (var item in action.Attributes)
                        {
                            if (item is HttpMethodAttribute route)
                            {
                                if (!string.IsNullOrEmpty(route.Name))
                                {
                                    actionName = route.Name;
                                }
                            }
                        }
                        action.Selectors[0].AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"{_routeOptions.RoutePrefix}/{actionName}"));
                    }
                }
            }
        }
    }
}
