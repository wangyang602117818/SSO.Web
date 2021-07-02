using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.WeChat.Subscription.Controllers
{
    [AllowAnonymous]
    [NoneLogRecord]
    public class HomeController : Controller
    {
        public string token = "tvO3zPS9Fdc4EvF86";
        public ActionResult Index()
        {
            Log4Net.InfoLog("x");
            return View();
        }
        public ActionResult WX(WxModel wxModel)
        {
            Log4Net.InfoLog(JsonSerializerHelper.Serialize(wxModel));
            return Content("ok");
        }
    }
    public class WxModel
    {
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string echostr { get; set; }
        
    }
}