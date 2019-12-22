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
        /// 最近几个月操作记录
        /// </summary>
        /// <param name="type">类型 day month year</param>
        /// <param name="last">最近多少 天 月 年</param>
        /// <returns></returns>
        public ActionResult OpRecord(string type = "day", int last = 30)
        {
            IQueryable<DateCountItem> result = null;
            if (type == "day") result = log.OpRecordByDay(DateTime.Now.AddDays(-last));
            if (type == "month") result = log.OpRecordByMonth(DateTime.Now.AddMonths(-last));
            if (type == "year") result = log.OpRecordByYear(DateTime.Now.AddYears(-last));
            return new ResponseModel<List<DateCountItem>>(ErrorCode.success, result.ToList());
        }
        /// <summary>
        /// 用户录入删除操作记录
        /// </summary>
        /// <param name="type">类型 day month year</param>
        /// <param name="last">最近多少 天 月 年</param>
        /// <returns></returns>
        public ActionResult UserRecord(string type = "day", int last = 30)
        {
            IQueryable<DateCountItem> input = null;
            IQueryable<DateCountItem> output = null;
            if (type == "day")
            {
                input = userBasic.UserRecordInByDay(DateTime.Now.AddDays(-last), false);
                output = userBasic.UserRecordInByDay(DateTime.Now.AddDays(-last), true);
            }
            if (type == "month")
            {
                input = userBasic.UserRecordByMonth(DateTime.Now.AddMonths(-last), false);
                output = userBasic.UserRecordByMonth(DateTime.Now.AddMonths(-last), true);
            }
            if (type == "year")
            {
                input = userBasic.UserRecordByYear(DateTime.Now.AddYears(-last), false);
                output = userBasic.UserRecordByYear(DateTime.Now.AddYears(-last), true);
            }
            List<DateCountItem> result = input.ToList();
            result.AddRange(output.ToList());
            return new ResponseModel<List<DateCountItem>>(ErrorCode.success, result, result.Count);
        }
    }
}