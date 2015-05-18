using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using YooPoon.Core.Autofac;
using YooPoon.WebFramework.Dependency;

namespace Zerg
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.EnableCors();
            GlobalConfiguration.Configure(WebApiConfig.Register);

//            //注册依赖容器
//            var initialize = new InitializeContainer();
//            initialize.Initializing();
//
//            var resolver = new YpDependencyResolver(initialize);
//            DependencyResolver.SetResolver(resolver);

            //注册全局Filter
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
