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
        public ActionResult GetList(string from = "", string controllerName = "", string actionName = "", DateTime? startTime = null, DateTime? endTime = null, string userId = "", string userName = null, Dictionary<string, string> sorts = null, bool? exception = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = logService.GetListJson(from, controllerName, actionName, startTime, endTime, userId, userName, sorts, exception, pageIndex, pageSize);
            return Content(result, "application/json");
        }
        public ActionResult GetListSimple(string from = "", string controllerName = "", string actionName = "", DateTime? startTime = null, DateTime? endTime = null, string userId = "", string userName = null, Dictionary<string, string> sorts = null, bool? exception = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = logService.GetListSimpleJson(from, controllerName, actionName, startTime, endTime, userId, userName, sorts, exception, pageIndex, pageSize);
            return Content(result, "application/json");
        }
        public ActionResult Detail(string id)
        {
            return Content(logService.GetByIdJson(id), "application/json");
        }
        public ActionResult GetFromList()
        {
            var result = logService.GetFromListJson();
            return Content(result, "application/json");
        }
        public ActionResult GetControllersByFrom(string from)
        {
            var result = logService.GetControllersByFromJson(from);
            return Content(result, "application/json");
        }
        public ActionResult GetActionsByController(string from, string controllerName)
        {
            var result = logService.GetActionsByControllerJson(from, controllerName);
            return Content(result, "application/json");
        }
        public ActionResult GetOperations()
        {
            var result = logService.GetOperationsJson();
            return Content(result, "application/json");
        }
    }
}