using System.Web;
using System.Web.Http;

namespace Zerg
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.EnableCors();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
