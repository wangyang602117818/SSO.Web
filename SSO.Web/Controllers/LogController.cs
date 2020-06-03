using SSO.Util.Client;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [NoneLogRecord]
    public class LogController : BaseController
    {
        public ActionResult GetList(string from = "", string controllerName = "", string actionName = "", DateTime? startTime = null, DateTime? endTime = null, string userId = "", int pageIndex = 1, int pageSize = 10)
        {
            Dictionary<string, string> sorts = new Dictionary<string, string>();
            sorts.Add("CreateTime", "desc");
            var result = logService.GetListJson(from, controllerName, actionName, startTime, endTime, userId, sorts, pageIndex, pageSize);
            return Content(result, "application/json");
        }
        public ActionResult GetFromList()
        {
            var result = logService.GetFromListJson();
            return Content(result, "application/json");
        }
    }
}