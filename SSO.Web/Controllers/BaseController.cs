using SSO.Model;
using SSO.Util;
using SSO.Util.Client;
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
        public static string[] admin = AppSettings.GetValue("admin").Split(';');
        public static string lang = AppSettings.GetValue("lang");
        public static string cookieName = AppSettings.GetValue("cookieName");
        public static string cookieTime = AppSettings.GetValue("cookieTime");
        public static string issuer = AppSettings.GetValue("issuer");
        public static string secretKey = AppSettings.GetValue("secretKey");
        public static string ticketTime = AppSettings.GetValue("ticketTime");
        protected JwtManager jwtManager = new JwtManager(secretKey, issuer, int.Parse(ticketTime));
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