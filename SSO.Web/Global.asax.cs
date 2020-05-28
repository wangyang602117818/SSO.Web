using System;
using System.IO;
using System.Reflection;
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

            var assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "bin\\SSO.Util.Client.dll");
            var stream = assembly.GetManifestResourceStream("SSO.Util.Client.log4net.config");
            log4net.Config.XmlConfigurator.Configure(stream);
        }
        protected void Application_BeginRequest()
        {
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", HttpContext.Current.Request.Headers["Origin"] ?? "*");
        }
    }
}
