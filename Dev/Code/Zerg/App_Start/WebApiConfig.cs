using System.Web.Http;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using YooPoon.Core.Autofac;
using YooPoon.WebFramework.API;

namespace Zerg
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //允许跨域请求
            config.EnableCors();
            
            //DI配置
            var initialize = new InitializeContainer();
            initialize.Initializing();
            //TODO:实现自己的Resolver，未实现前暂时使用Auto自带的
            config.DependencyResolver = new AutofacWebApiDependencyResolver(initialize.ContainerManager.Container);


            //filter
            //config.Filters.Add((IFilter)config.DependencyResolver.GetService(typeof(YpAPIHandleErrorAttribute)));
            //config.Filters.Add((IFilter)config.DependencyResolver.GetService(typeof(YpAPIAuthorizeAttribute)));
            #region 路由配置
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            #endregion
        }
    }
}
