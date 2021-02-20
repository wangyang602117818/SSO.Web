﻿using SSO.Util.Client.SqlBatisLite;
using System.Web.Mvc;
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

        }
    }
}
