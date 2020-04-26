using SSO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class BaseController : Controller
    {
        Business.Log log = new Business.Log();
        protected int InfoLog(IEnumerable<string> recordId, string content, string remark = null, string userId = null)
        {
            var from = Request.Headers["Origin"];
            var userName = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "StaffName").Select(s => s.Value).FirstOrDefault();
            return log.Insert(from, LogType.Info, recordId, content, remark, userId ?? User.Identity.Name, userName, Request.UserHostAddress, Request.UserAgent);
        }
        protected int InfoLog(string recordId, string content, string remark = null, string userId = null)
        {
            return InfoLog(new List<string>() { recordId }, content, remark, userId);
        }
    }
}