﻿using Newtonsoft.Json;
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
        public ActionResult GetToken(string ticket, string from, string audience = null)
        {
            string token = "";
            string userId = jwtManager.DecodeTicket(ticket);
            if (audience.IsNullOrEmpty()) audience = SSOAuthorizeAttribute.GetRemoteIp(Request);
            if (!userId.IsNullOrEmpty())
            {
                User u = user.GetUser(userId);
                if (u == null)
                {
                    if (userId == admin[0])
                    {
                        token = jwtManager.GenerateToken(userId, userId, lang, audience, from, null);
                    }
                }
                else
                {
                    Settings setting = settings.GetSetting(userId);
                    if (setting != null) lang = setting.Lang;
                    token = jwtManager.GenerateToken(userId, u.UserName, lang, audience, from, null);
                }
            }
            return new ResponseModel<string>(ErrorCode.success, token);
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty()) return View();
            returnUrl = returnUrl.TrimEnd('/', '#');
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
            var authorization = Request.Cookies[ssoCookieKey];
            if (authorization != null)  //sso login
            {
                try
                {
                    HttpCookie ssoUrlCookie = Request.Cookies["ssourls"];
                    if (ssoUrlCookie != null)
                    {
                        List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrlCookie.Value.Base64ToStr());
                        Uri uri = new Uri(returnUrl);
                        foreach (var ssoUrl in ssoUrls)
                        {
                            if (ssoUrl == uri.Scheme + "://" + uri.Authority) return View(); //之前登录过,cookie过期
                        }
                    }
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
            string from = "";
            if (!returnUrl.IsNullOrEmpty())
            {
                returnUrl = returnUrl.TrimEnd('/', '#');
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
                Uri uri = new Uri(HttpUtility.UrlDecode(returnUrl));
                from = (uri.Scheme + "://" + uri.Authority).ReplaceHttpPrefix().TrimEnd('/');
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
            string audience = SSOAuthorizeAttribute.GetRemoteIp(Request);
            string token = jwtManager.GenerateToken(u.UserId, u.UserName, lang, audience, from, null);
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
        [AllowAnonymous]
        [HttpPost]
        [LogRecord(true, false)]
        public ActionResult LoginGetToken(LoginReturnTokenModel login)
        {
            User u = null;
            if (login.UserId == admin[0] && login.PassWord == admin[1])
            {
                u = new User()
                {
                    UserId = login.UserId,
                    UserName = login.UserId,
                    RoleName = admin[2]
                };
            }
            else
            {
                u = user.Login(login.UserId, login.PassWord.GetSha256());
            }
            if (u == null)
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
            Settings setting = settings.GetSetting(User.Identity.Name);
            if (setting != null) lang = setting.Lang;
            string audience = SSOAuthorizeAttribute.GetRemoteIp(Request);
            string token = jwtManager.GenerateToken(u.UserId, u.UserName, lang, audience, login.From, null);
            return new ResponseModel<string>(ErrorCode.success, token);
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
            //List<string> newSSoUrls = new List<string>();
            //foreach (string url in ssoUrls)
            //{
            //    if (CheckSiteAvailable(url)) newSSoUrls.Add(url);
            //}
            return Redirect(ssoUrls[0] + "?ssourls=" + ssoUrlCookie.Value + "&returnUrl=" + returnUrl);
        }
        public ActionResult DecodeToken()
        {
            return new ResponseModel<object>(ErrorCode.success, JwtManager.GetUserData());
        }

    }
}