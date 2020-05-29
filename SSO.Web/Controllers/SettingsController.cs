using SSO.Util;
using SSO.Util.Client;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class SettingsController : BaseController
    {
        Business.Settings settings = new Business.Settings();
        public ActionResult SetLang(string lang)
        {
            if (settings.UpdateLang(User.Identity.Name, lang) >= 0)
            {
                string token = jwtManager.ModifyTokenLang(HttpContext.Items["Authorization"].ToString(), lang, 24 * 60);
                return new ResponseModel<string>(ErrorCode.success, token);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult SetLangLocal(string lang)
        {
            if (settings.UpdateLang(User.Identity.Name, lang) >= 0)
            {
                string token = jwtManager.ModifyTokenLang(HttpContext.Items["Authorization"].ToString(), lang, 24 * 60);
                HttpCookie httpCookie = new HttpCookie(cookieName, token);
                if (cookieTime != "session")
                {
                    httpCookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(cookieTime));
                }
                Response.Cookies.Add(httpCookie);
                return new ResponseModel<string>(ErrorCode.success, token);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
    }
}