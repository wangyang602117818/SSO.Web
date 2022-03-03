using SSO.Util.Client;
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
            if (word.IsNullOrEmpty())
            {
                return new ResponseModel<string[]>(ErrorCode.success, new string[] { });
            }
            return Json(searchService.Suggest(word), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string word, bool highlight = false, int pageIndex = 1, int pageSize = 10)
        {
            if (word.IsNullOrEmpty())
            {
                return new ResponseModel<string[]>(ErrorCode.success, new string[] { });
            }
            var res = searchService.Search(word, DataBaseType.none, "", highlight, pageIndex, pageSize);
            return Content(JsonSerializerHelper.Serialize(res), "application/json");
        }
    }
}