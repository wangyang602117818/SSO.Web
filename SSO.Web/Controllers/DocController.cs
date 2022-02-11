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
        public ActionResult UserInfo()
        {
            return View();
        }
        public ActionResult FileIndex()
        {
            return View();
        }
        public ActionResult FileUpload()
        {
            return View();
        }
        public ActionResult FileDownload()
        {
            return View();
        }
        public ActionResult FileOpr()
        {
            return View();
        }
        public ActionResult FileThumb()
        {
            return View();
        }
        public ActionResult FilePlay()
        {
            return View();
        }
    }
}