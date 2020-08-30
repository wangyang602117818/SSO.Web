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
            var result = logService.GetList(from, controllerName, actionName, startTime, endTime, userId, userName, sorts, exception, pageIndex, pageSize);
            return Content(JsonSerializerHelper.Serialize(result));
        }
        public ActionResult Detail(string id)
        {
            var result = logService.GetById(id);
            return Content(JsonSerializerHelper.Serialize(result));
        }
        public ActionResult GetFromList()
        {
            var result = logService.GetFromList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetControllersByFrom(string from)
        {
            var result = logService.GetControllersByFrom(from);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActionsByController(string from, string controllerName)
        {
            var result = logService.GetActionsByController(from, controllerName);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOperations()
        {
            var result = logService.GetOperations();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}