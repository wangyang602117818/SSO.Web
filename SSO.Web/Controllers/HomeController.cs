using SSO.Business;
using SSO.Data.Models;
using SSO.Util;
using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(w => w.FullName.Contains("SSO.Web.Controllers"));
            List<string> actions = new List<string>();
            foreach (var item in controllers)
            {
                var methods = item.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(DisplayNameAttribute));
                    foreach (Attribute att in attributes)
                    {
                        actions.Add(((DisplayNameAttribute)att).DisplayName);
                    }
                }
            }
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
        [DisplayName("生成Ticket")]
        public ActionResult Ticket()
        {
            return Content(jwtManager.GenerateTicket("CN445379"));
        }
        [DisplayName("解析Ticket")]
        public ActionResult Decode(string ticket)
        {
            return Content(jwtManager.DecodeTicket(ticket));
        }
        public ActionResult DecodeToken(string token)
        {
            var result = JwtManager.ParseAuthorization(token, ssoSecretKey);
            return Content(result.Identity.Name);
        }
    }
}