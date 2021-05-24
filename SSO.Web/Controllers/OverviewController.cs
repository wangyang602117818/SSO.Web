using SSO.Model;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class OverviewController : BaseController
    {
        Business.User user = new Business.User();
        Business.Company company = new Business.Company();
        Business.Department department = new Business.Department();
        Business.Role role = new Business.Role();
        Business.UserDepartmentMapping userDepartmentMapping = new Business.UserDepartmentMapping();
        public ActionResult Total()
        {
            return new ResponseModel<Totals>(ErrorCode.success, new Totals()
            {
                Companys = company.RecordCount(),
                Departments = department.RecordCount(),
                Roles = role.RecordCount(),
                Users = user.RecordCount()
            });
        }
        public ActionResult UserRatio()
        {
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, user.GetUserRatio());
        }
        public ActionResult UserCompanyRatio()
        {
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, user.GetUserCompanyRatio());
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
        /// <param name="last">最近多少天</param>
        /// <returns></returns>
        public ActionResult UserRecord(int last = 30)
        {
            var input = user.UserRecordInByDay(DateTime.Now.AddDays(-last), false).Select(s => { s.type = "insert"; return s; });
            var output = user.UserRecordInByDay(DateTime.Now.AddDays(-last), true).Select(s => { s.type = "delete"; return s; });
            List<DateCountItem> result = input.ToList();
            result.AddRange(output.ToList());
            return new ResponseModel<IEnumerable<DateCountItem>>(ErrorCode.success, result.OrderBy(o => o.date), result.Count);
        }

    }
}