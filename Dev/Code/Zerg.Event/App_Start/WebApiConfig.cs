using System.Web.Http;

namespace Zerg.Event
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //允许跨域请求
            config.EnableCors();
            
            #region 路由配置
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
            #endregion
        }
    }
}
