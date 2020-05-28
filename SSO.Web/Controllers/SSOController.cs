using MongoDB.Bson;
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
        public static string[] admin = AppSettings.GetValue("admin").Split(';');
        public static string lang = AppSettings.GetValue("lang");
        public static string cookieName = AppSettings.GetValue("cookieName");
        public static string cookieTime = AppSettings.GetValue("cookieTime");
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetToken(string ticket, string ip)
        {
            string token = "";
            string userId = JwtManager.DecodeTicket(ticket);
            if (!userId.IsNullOrEmpty())
            {
                UserBasic userBasic = user.GetUser(userId);
                if (userBasic == null)
                {
                    if (userId == admin[0])
                    {
                        token = JwtManager.GenerateToken(userId, userId, lang, null, null, new List<string>() { admin[2] }, ip ?? Request.UserHostAddress, 20);
                    }
                }
                else
                {
                    Settings setting = settings.GetSetting(userId);
                    if (setting != null) lang = setting.Lang;
                    string[] departments = userBasic.DepartmentName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] roles = userBasic.RoleName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    token = JwtManager.GenerateToken(userId, userBasic.UserName, lang, userBasic.CompanyName, departments, roles, ip ?? Request.UserHostAddress, 20);
                }
            }
            return new ResponseModel<string>(ErrorCode.success, token);
        }
        public ActionResult Login(string returnUrl)
        {
            var authorization = Request.Cookies[cookieName];
            if (authorization != null)  //sso login
            {
                try
                {
                    var userId = JwtAuthorizeAttribute.ParseToken(authorization.Value).Identity.Name;
                    string ticket = JwtManager.GenerateTicket(userId);
                    returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
                    if (cookieName != "session")
                    {
                        authorization.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(cookieName));
                    }
                    Response.Cookies.Add(authorization);
                    //sso退出用
                    JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
                    return Redirect(returnUrl);
                }
                catch (Exception ex)
                {
                    Log4Net.InfoLog(ex);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            UserBasic userBasic = null;
            if (loginModel.UserId == admin[0] && loginModel.PassWord == admin[1])
            {
                userBasic = new UserBasic()
                {
                    UserId = loginModel.UserId,
                    UserName = loginModel.UserId,
                    RoleName = admin[2],
                    DepartmentName = ""
                };
            }
            else
            {
                userBasic = user.Login(loginModel.UserId, loginModel.PassWord.GetSha256());
            }
            if (userBasic == null)
            {
                InfoLog("0", "LoginFault", loginModel.UserId);
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
            Settings setting = settings.GetSetting(User.Identity.Name);
            string[] roles = userBasic.RoleName.Split(',');
            string[] departments = userBasic.DepartmentName.Split(',');
            if (setting != null) lang = setting.Lang;
            string token = JwtManager.GenerateToken(userBasic.UserId, userBasic.UserName, lang, userBasic.CompanyName, departments, roles, Request.UserHostAddress, 24 * 60);
            HttpCookie httpCookie = new HttpCookie(cookieName, token);
            if (cookieTime != "session")
            {
                httpCookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(cookieTime));
            }
            Response.Cookies.Add(httpCookie);
            JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
            if (returnUrl.IsNullOrEmpty()) returnUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + Request.ApplicationPath;
            InfoLog("0", "Login", loginModel.UserId, loginModel.UserId);
            return new ResponseModel<string>(ErrorCode.success, returnUrl);
        }
        [JwtAuthorize]
        public ActionResult LogOut()
        {
            InfoLog("0", "LogOut", User.Identity.Name);
            var authorization = Request.Cookies[cookieName];
            if (authorization != null)
            {
                authorization.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authorization);
            }
            var ssoUrlCookie = Request.Cookies["ssourls"];
            if (ssoUrlCookie != null)
            {
                ssoUrlCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ssoUrlCookie);
            }
            if (ssoUrlCookie == null) return RedirectToAction("Index");
            List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrlCookie.Value.Base64ToStr());
            return Redirect(ssoUrls[0] + "?ssourls=" + ssoUrlCookie.Value);
        }
        [JwtAuthorize]
        public ActionResult DecodeToken()
        {
            var roles = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == ClaimTypes.Role).Select(s => s.Value);
            var userName = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "StaffName").Select(s => s.Value).FirstOrDefault();
            var lang = Request.Cookies["lang"] == null ? ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "Lang").Select(s => s.Value).FirstOrDefault() : Request.Cookies["lang"].Value;
            BsonDocument userRole = new BsonDocument()
            {
                {"UserId",User.Identity.Name },
                {"UserName",userName },
                {"Role",new BsonArray(roles) },
                {"Lang",lang }
            };
            return new ResponseModel<BsonDocument>(ErrorCode.success, userRole);
        }
    }
}