using SSO.Util.Client;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class LogController : BaseController
    {
        [NoneLogRecord]
        public ActionResult GetList(string from = "", string userId = "", int pageIndex = 1, int pageSize = 10)
        {
            Dictionary<string, string> sorts = new Dictionary<string, string>();
            sorts.Add("CreateTime", "desc");
            var result = logService.GetListJson(from, userId, sorts, pageIndex, pageSize);
            return Content(result, "application/json");
        }
    }
}