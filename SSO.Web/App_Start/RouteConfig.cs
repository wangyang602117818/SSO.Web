using System.Web.Mvc;
using System.Web.Routing;

namespace SSO.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "SSO", action = "Login", id = UrlParameter.Optional }
           );
            //routes.MapRoute(
            //    name: "file",
            //    url: "{controller}/{action}/{id}/{filename}",
            //    defaults: new { controller = "File", action = "DownloadPic", id = UrlParameter.Optional, filename = UrlParameter.Optional }
            //);
            routes.MapMvcAttributeRoutes();
        }
    }
}
