using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class SearchController : BaseController
    {
        public ActionResult Suggest(string word)
        {
            return Json(searchService.Suggest(word), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string word, int pageIndex = 1, int pageSize = 10)
        {
            return Json(searchService.Search(word, pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}