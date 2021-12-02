using Quartz;
using SSO.Util.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class BaseController : Controller
    {
        public static string[] admin = AppSettings.GetValue("admin").Split(';');
        public static string lang = AppSettings.GetValue("lang");
        public static string ssoCookieKey = AppSettings.GetValue("ssoCookieKey");
        public static string ssoCookieTime = AppSettings.GetValue("ssoCookieTime");
        public static string issuer = AppSettings.GetValue("issuer");
        public static string ssoSecretKey = AppSettings.GetValue("ssoSecretKey");
        public static string ssoTicketTime = AppSettings.GetValue("ssoTicketTime");
        public static string messageBaseUrl = AppSettings.GetValue("messageBaseUrl");
        protected JwtManager jwtManager = new JwtManager(ssoSecretKey, issuer, int.Parse(ssoTicketTime));
        protected LogService logService = new LogService(messageBaseUrl);
        protected SearchService searchService = new SearchService(messageBaseUrl);
        protected DateTime GetNextRunTimeByCrons(IEnumerable<string> crons)
        {
            List<DateTimeOffset> nextRunTimes = new List<DateTimeOffset>();
            foreach (var item in crons)
            {
                var nextRunTime = new CronExpression(item).GetNextValidTimeAfter(DateTime.Now);
                nextRunTimes.Add(nextRunTime.Value);
            }
            nextRunTimes.Sort();
            return nextRunTimes[0].LocalDateTime;
        }
        protected bool CheckSiteAvailable(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = 3000;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
    }
}