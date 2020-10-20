using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [NoneLogRecord]
    public class LogController : BaseController
    {
        public ActionResult GetList(string from = "", string to = "", string controllerName = "", string actionName = "", DateTime? startTime = null, DateTime? endTime = null, string userId = "", string userName = null, Dictionary<string, string> sorts = null, bool? exception = null, int pageIndex = 1, int pageSize = 10)
        {
            var result = logService.GetList(from, to, controllerName, actionName, startTime, endTime, userId, userName, sorts, exception, pageIndex, pageSize);
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
        public ActionResult GetToList()
        {
            var result = logService.GetToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetControllersByTo(string to)
        {
            var result = logService.GetControllersByTo(to);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActionsByController(string to, string controllerName)
        {
            var result = logService.GetActionsByController(to, controllerName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOperations()
        {
            var result = logService.GetOperations();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}