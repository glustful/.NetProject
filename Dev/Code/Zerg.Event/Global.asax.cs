using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.WebApi;
using YooPoon.Core.Autofac;
using YooPoon.WebFramework.Dependency;
using Yunjoy.Web.Admin;
using Zerg.Event.App_Start;

namespace Zerg.Event
{
    public class Global : HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {


            //注册依赖容器
            var initialize = new InitializeContainer();
            initialize.Initializing();

            var resolver = new YpDependencyResolver(initialize);
            DependencyResolver.SetResolver(resolver);

            //注册全局Filter
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configuration.EnableCors();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(initialize.ContainerManager.Container);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{

        //}

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{

        //}

        //protected void Application_AuthenticateRequest(object sender, EventArgs e)
        //{

        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{

        //}

        //protected void Session_End(object sender, EventArgs e)
        //{

        //}

        //protected void Application_End(object sender, EventArgs e)
        //{

        //}
    }
}