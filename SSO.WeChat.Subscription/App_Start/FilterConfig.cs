using SSO.Util.Client;
using System.Web;
using System.Web.Mvc;

namespace SSO.WeChat.Subscription
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SSOAuthorizeAttribute("",false));
            filters.Add(new MyHandleErrorAttribute());
            filters.Add(new ValidateModelStateAttribute());
            filters.Add(new LogRecordAttribute());
        }
    }
}
