using MongoDB.Bson;
using SSO.Model;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class OverviewController : BaseController
    {
        Business.UserBasic userBasic = new Business.UserBasic();
        Business.Log log = new Business.Log();
        public ActionResult Total()
        {
            return new ResponseModel<OverviewTotal>(ErrorCode.success, userBasic.GetOverviewTotal());
        }
        /// <summary>
        /// 最近几个月操作记录(天)
        /// </summary>
        /// <returns></returns>
        public ActionResult OpRecord(int month)
        {
            var result = log.OpRecordByDay(DateTime.Now.AddMonths(-month));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}