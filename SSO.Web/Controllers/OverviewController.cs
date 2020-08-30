using SSO.Model;
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
    public class OverviewController : BaseController
    {
        Business.UserBasic userBasic = new Business.UserBasic();
        Business.UserDepartmentMapping userDepartmentMapping = new Business.UserDepartmentMapping();
        public ActionResult Total()
        {
            return new ResponseModel<OverviewTotal>(ErrorCode.success, userBasic.GetOverviewTotal());
        }
        public ActionResult UserRatio()
        {
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, userBasic.GetUserRatio());
        }
        public ActionResult UserCompanyRatio()
        {
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, userBasic.GetUserCompanyRatio());
        }
        public ActionResult UserDepartmentRatio()
        {
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, userDepartmentMapping.GetUserDepartmentRatio());
        }
        /// <summary>
        /// 最近几个月操作记录
        /// </summary>
        /// <param name="type">类型 day month year</param>
        /// <param name="last">最近多少 天 月 年</param>
        /// <returns></returns>
        public ActionResult OpRecord(int last = 30)
        {
            var result = logService.GetOpRecordByDay(last);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 用户录入删除操作记录
        /// </summary>
        /// <param name="type">类型 day month year</param>
        /// <param name="last">最近多少 天 月 年</param>
        /// <returns></returns>
        public ActionResult UserRecord(string type = "day", int last = 30)
        {
            IEnumerable<DateCountItem> input = null;
            IEnumerable<DateCountItem> output = null;
            if (type == "day")
            {
                input = userBasic.UserRecordInByDay(DateTime.Now.AddDays(-last), false).ToList().Select(s => new DateCountItem() { date = DateTime.Parse(s.date).ToString("yyyy-MM-dd"), count = s.count, type = s.type });
                output = userBasic.UserRecordInByDay(DateTime.Now.AddDays(-last), true).ToList().Select(s => new DateCountItem() { date = DateTime.Parse(s.date).ToString("yyyy-MM-dd"), count = s.count , type = s.type });
            }
            if (type == "month")
            {
                input = userBasic.UserRecordByMonth(DateTime.Now.AddMonths(-last), false).ToList().Select(s => new DateCountItem() { date = DateTime.Parse(s.date).ToString("yyyy-MM"), count = s.count, type = s.type });
                output = userBasic.UserRecordByMonth(DateTime.Now.AddMonths(-last), true).ToList().Select(s => new DateCountItem() { date = DateTime.Parse(s.date).ToString("yyyy-MM"), count = s.count, type = s.type });
            }
            if (type == "year")
            {
                input = userBasic.UserRecordByYear(DateTime.Now.AddYears(-last), false);
                output = userBasic.UserRecordByYear(DateTime.Now.AddYears(-last), true);
            }
            List<DateCountItem> result = input.ToList();
            result.AddRange(output.ToList());
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, result.OrderBy(o => o.date), result.Count);
        }
        
    }
}