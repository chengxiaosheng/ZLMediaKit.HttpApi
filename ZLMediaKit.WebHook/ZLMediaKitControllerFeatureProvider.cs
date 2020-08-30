using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using ZLMediaKit.WebHook.Services;

namespace ZLMediaKit.WebHook
{
    internal class ZLMediaKitControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            if (typeInfo == typeof(ZLMediaKitWebHookServcies))
            {
                return true;
            }
            return base.IsController(typeInfo);
        }
    }
}
