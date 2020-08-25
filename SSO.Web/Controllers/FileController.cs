using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class FileController : Controller
    {
        public ActionResult Upload(HttpPostedFileBase file)
        {

            return View();
        }
    }
}