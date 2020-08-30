using System;

namespace ZLMediaKit.WebHook
{
    public class APIRouteOptions
    {
        /// <summary>
        /// 默认路由
        /// </summary>
        public const string DefaultRoutePrefix = "index/hook";

        /// <summary>
        /// 路由
        /// </summary>
        /// <remarks>action 之前的所有路径 </remarks>
        public string RoutePrefix { get; set; }

        ///// <summary>
        ///// Cors策略名称
        ///// </summary>
        //public string CorsPolicyName { get; set; }

        /// <summary>
        /// api路由选项
        /// </summary>
        /// <param name="routePrefix"></param>
        /// <param name="corsPolicyName">null for default</param>
        public APIRouteOptions(string routePrefix = DefaultRoutePrefix/*, string corsPolicyName = null*/)
        {
            RoutePrefix = routePrefix ?? throw new ArgumentNullException(nameof(routePrefix));
            //CorsPolicyName = corsPolicyName;//??  throw new ArgumentNullException(nameof(corsPolicyName));
        }
    }
}
