using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SSO.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FileInfo fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "bin\\log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(fileInfo);
        }
        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", HttpContext.Current.Request.Headers["Origin"] ?? "*");
        }
    }
}
