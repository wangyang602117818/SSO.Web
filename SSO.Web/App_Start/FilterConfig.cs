using FileService.Web.Filters;
using SSO.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new JwtAuthorize());
            filters.Add(new MyHandleErrorAttribute());
            filters.Add(new ValidateModelStateAttribute());
        }
    }
}
