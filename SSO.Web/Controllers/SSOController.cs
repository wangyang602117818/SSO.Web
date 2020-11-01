using Newtonsoft.Json;
using SSO.Data;
using SSO.Data.Models;
using SSO.Model;
using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class SSOController : BaseController
    {
        Business.User user = new Business.User();
        Business.Settings settings = new Business.Settings();
        [AllowAnonymous]
        public ActionResult Index()
        {

            return View();
        }
        [AllowAnonymous]
        public ActionResult GetToken(string from, string ticket, string ip)
        {
            string token = "";
            string userId = jwtManager.DecodeTicket(ticket);
            Dictionary<string, string> extra = new Dictionary<string, string>();
            extra.Add("from", from.ReplaceHttpPrefix().ToLower());
            if (!userId.IsNullOrEmpty())
            {
                User u = user.GetUser(userId);
                if (u == null)
                {
                    if (userId == admin[0])
                    {
                        token = jwtManager.GenerateToken(userId, userId, lang, ip ?? Request.UserHostAddress, 20, extra);
                    }
                }
                else
                {
                    Settings setting = settings.GetSetting(userId);
                    if (setting != null) lang = setting.Lang;
                    token = jwtManager.GenerateToken(userId, u.UserName, lang, ip ?? Request.UserHostAddress, 20, extra);
                }
            }
            return new ResponseModel<string>(ErrorCode.success, token);
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!returnUrl.IsNullOrEmpty())
            {
                foreach (string key in Request.QueryString.Keys)
                {
                    if (key == "returnUrl") continue;
                    if (returnUrl.Contains("?"))
                    {
                        returnUrl += "&" + key + "=" + Request.QueryString[key];
                    }
                    else
                    {
                        returnUrl += "?" + key + "=" + Request.QueryString[key].ToString();
                    }
                }
                returnUrl = HttpUtility.UrlDecode(returnUrl);
            }
            var authorization = Request.Cookies[ssoCookieKey];
            if (authorization != null)  //sso login
            {
                try
                {
                    var userId = JwtManager.ParseAuthorization(authorization.Value, ssoSecretKey).Identity.Name;
                    string ticket = jwtManager.GenerateTicket(userId);
                    returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
                    if (ssoCookieTime != "session")
                    {
                        authorization.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(ssoCookieTime));
                    }
                    Response.Cookies.Add(authorization);
                    //sso退出用
                    JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                }
                catch (Exception ex)
                {
                    Log4Net.InfoLog(ex);
                }
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [LogRecord(true, false)]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (!returnUrl.IsNullOrEmpty())
            {
                foreach (string key in Request.QueryString.Keys)
                {
                    if (key == "returnUrl") continue;
                    if (returnUrl.Contains("?"))
                    {
                        returnUrl += "&" + key + "=" + Request.QueryString[key];
                    }
                    else
                    {
                        returnUrl += "?" + key + "=" + Request.QueryString[key].ToString();
                    }
                }
                returnUrl = HttpUtility.UrlDecode(returnUrl);
            }
            User u = null;
            if (loginModel.UserId == admin[0] && loginModel.PassWord == admin[1])
            {
                u = new User()
                {
                    UserId = loginModel.UserId,
                    UserName = loginModel.UserId,
                    RoleName = admin[2]
                };
            }
            else
            {
                u = user.Login(loginModel.UserId, loginModel.PassWord.GetSha256());
            }
            if (u == null)
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
            Settings setting = settings.GetSetting(User.Identity.Name);
            if (setting != null) lang = setting.Lang;
            Dictionary<string, string> extra = new Dictionary<string, string>();
            extra.Add("from", issuer.ReplaceHttpPrefix().ToLower());
            string token = jwtManager.GenerateToken(u.UserId, u.UserName, lang, Request.UserHostAddress, 24 * 60, extra);
            HttpCookie httpCookie = new HttpCookie(ssoCookieKey, token);
            if (ssoCookieTime != "session")
            {
                httpCookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(ssoCookieTime));
            }
            Response.Cookies.Add(httpCookie);
            JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
            if (returnUrl.IsNullOrEmpty()) returnUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + Request.ApplicationPath;
            //登录成功,替换掉旧的ticket
            string ticket = jwtManager.GenerateTicket(u.UserId);
            returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
            return new ResponseModel<string>(ErrorCode.success, returnUrl);
        }
        /// <summary>
        /// 退出带上 returnUrl 方便再次登录进入当前页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public ActionResult LogOut(string returnUrl)
        {
            if (!returnUrl.IsNullOrEmpty())
                returnUrl = HttpUtility.UrlDecode(returnUrl);
            var authorization = Request.Cookies[ssoCookieKey];
            if (authorization != null)
            {
                HttpCookie cookie = new HttpCookie(ssoCookieKey);
                cookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie);
            }
            var ssoUrlCookie = Request.Cookies["ssourls"];
            if (ssoUrlCookie != null)
            {
                HttpCookie cookieUrl = new HttpCookie("ssourls");
                cookieUrl.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookieUrl);
            }
            if (ssoUrlCookie == null) return RedirectToAction("Login", new { returnUrl = returnUrl });
            List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrlCookie.Value.Base64ToStr());
            return Redirect(ssoUrls[0] + "?ssourls=" + ssoUrlCookie.Value + "&returnUrl=" + returnUrl);
        }
        public ActionResult DecodeToken()
        {
            var userName = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "StaffName").Select(s => s.Value).FirstOrDefault();
            var lang = Request.Cookies["lang"] == null ? ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "Lang").Select(s => s.Value).FirstOrDefault() : Request.Cookies["lang"].Value;
            object user = new
            {
                UserId = User.Identity.Name,
                UserName = userName,
                Lang = lang
            };
            return new ResponseModel<object>(ErrorCode.success, user);
        }

    }
}