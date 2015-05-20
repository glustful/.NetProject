using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using YooPoon.Core.Autofac;
using YooPoon.WebFramework.Dependency;

namespace Zerg
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            // Web API 配置和服务
            var initialize = new InitializeContainer();
            initialize.Initializing();
            //TODO:实现自己的Resolver，未实现前暂时使用Auto自带的
            config.DependencyResolver = new AutofacWebApiDependencyResolver(initialize.ContainerManager.Container);
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // config.Routes.MapHttpRoute(
            //    name: "MessageDetailApi",
            //    routeTemplate: "api/MessageDetail/SearchMessageDetail"
            //);

        }
    }
}
