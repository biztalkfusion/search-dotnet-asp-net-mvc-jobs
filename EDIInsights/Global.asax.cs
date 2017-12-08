using System.Web.Mvc;
using System.Web.Routing;

namespace BizTalkFusion.Solutions.Integration
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
