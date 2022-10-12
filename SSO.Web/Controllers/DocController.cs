using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    [NoneLogRecord]
    public class DocController : Controller
    {
        public ActionResult Index(string id)
        {
            
            //HttpContext.Request
            return View(id);
        }
    }
}