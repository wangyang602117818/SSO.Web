using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class DocController : Controller
    {
        // GET: Doc
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SSOIndex()
        {
            return View();
        }
        public ActionResult AspNet()
        {
            return View();
        }
        public ActionResult NetCore()
        {
            return View();
        }
        public ActionResult Vue()
        {
            return View();
        }
        public ActionResult React()
        {
            return View();
        }
    }
}