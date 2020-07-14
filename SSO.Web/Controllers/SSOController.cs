using Newtonsoft.Json;
using SSO.Data.Models;
using SSO.Model;
using SSO.Util;
using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class SSOController : BaseController
    {
        Business.UserBasic user = new Business.UserBasic();
        Business.Settings settings = new Business.Settings();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetToken(string from, string ticket, string ip)
        {
            string token = "";
            string userId = jwtManager.DecodeTicket(ticket);
            Dictionary<string, string> extra = new Dictionary<string, string>();
            extra.Add("from", from);
            if (!userId.IsNullOrEmpty())
            {
                UserBasic userBasic = user.GetUser(userId);
                if (userBasic == null)
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
                    token = jwtManager.GenerateToken(userId, userBasic.UserName, lang, ip ?? Request.UserHostAddress, 20, extra);
                }
            }
            return new ResponseModel<string>(ErrorCode.success, token);
        }
        public ActionResult Login(string returnUrl)
        {
            if (!returnUrl.IsNullOrEmpty())
                returnUrl = HttpUtility.UrlDecode(returnUrl);
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
        [HttpPost]
        [LogRecord(RecordContent = false)]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (!returnUrl.IsNullOrEmpty())
                returnUrl = HttpUtility.UrlDecode(returnUrl);
            UserBasic userBasic = null;
            if (loginModel.UserId == admin[0] && loginModel.PassWord == admin[1])
            {
                userBasic = new UserBasic()
                {
                    UserId = loginModel.UserId,
                    UserName = loginModel.UserId,
                    RoleName = admin[2]
                };
            }
            else
            {
                userBasic = user.Login(loginModel.UserId, loginModel.PassWord.GetSha256());
            }
            if (userBasic == null)
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
            user.UpdateLoginTime(userBasic.Id);
            Settings setting = settings.GetSetting(User.Identity.Name);
            if (setting != null) lang = setting.Lang;
            Dictionary<string, string> extra = new Dictionary<string, string>();
            extra.Add("from", issuer);
            string token = jwtManager.GenerateToken(userBasic.UserId, userBasic.UserName, lang, Request.UserHostAddress, 24 * 60, extra);
            HttpCookie httpCookie = new HttpCookie(ssoCookieKey, token);
            if (ssoCookieTime != "session")
            {
                httpCookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(ssoCookieTime));
            }
            Response.Cookies.Add(httpCookie);
            JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
            if (returnUrl.IsNullOrEmpty()) returnUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + Request.ApplicationPath;
            //登录成功,替换掉旧的ticket
            string ticket = jwtManager.GenerateTicket(userBasic.UserId);
            returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
            return new ResponseModel<string>(ErrorCode.success, returnUrl);
        }
        /// <summary>
        /// 退出带上 returnUrl 方便再次登录进入当前页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [JwtAuthorize]
        public ActionResult LogOut(string returnUrl)
        {
            if (!returnUrl.IsNullOrEmpty())
                returnUrl = HttpUtility.UrlDecode(returnUrl);
            var authorization = Request.Cookies[ssoCookieKey];
            if (authorization != null)
            {
                authorization.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(authorization);
            }
            var ssoUrlCookie = Request.Cookies["ssourls"];
            if (ssoUrlCookie != null)
            {
                ssoUrlCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(ssoUrlCookie);
            }
            if (ssoUrlCookie == null) return RedirectToAction("Login", new { returnUrl = returnUrl });
            List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrlCookie.Value.Base64ToStr());
            return Redirect(ssoUrls[0] + "?ssourls=" + ssoUrlCookie.Value + "&returnUrl=" + returnUrl);
        }
        [JwtAuthorize]
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