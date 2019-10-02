using Newtonsoft.Json;
using SSO.Util;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class SSOController : Controller
    {
        public static int cookieTime = int.Parse(ConfigurationManager.AppSettings["cookieTime"]);
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(Request.Cookies["ssourls"].Value.Base64ToStr());
            ViewBag.ssoUrls = ssoUrls;
            return View();
        }
        //[AllowAnonymous]
        //public ActionResult GetToken(string ticket, string ip)
        //{
        //    string userId = JwtManager.DecodeTicket(ticket);
        //    string token = userId == "" ? "" : JwtManager.GenerateToken(userId, null, null, null, null, ip, 20);
        //    return new ResponseModel<string>(ErrorCode.success, token);
        //}
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var authorization = Request.Cookies[Request.Url.Host + ".auth"];
            if (authorization != null)  //sso login
            {
                try
                {
                    var userId = JwtAuthorizeAttribute.ParseToken(authorization.Value).Identity.Name;
                    string ticket = JwtManager.GenerateTicket(userId);
                    returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
                    authorization.Expires = DateTime.Now.AddMinutes(cookieTime);
                    Response.Cookies.Add(authorization);
                    //sso退出用
                    JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl, cookieTime);
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
        [AllowAnonymous]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            if (loginModel.UserId == "wang" && loginModel.PassWord == "123")
            {
                string userId = "CN445379";
                string ticket = JwtManager.GenerateTicket(userId);
                returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
                string token = JwtManager.GenerateToken(userId, null, null, null, null, Request.UserHostAddress, 24 * 60);
                HttpCookie httpCookie = new HttpCookie(Request.Url.Host + ".auth", token);
                httpCookie.Expires = DateTime.Now.AddMinutes(cookieTime);
                Response.Cookies.Add(httpCookie);
                JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl, cookieTime);  //sso退出用
                return Redirect(returnUrl);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
        }
        public ActionResult LogOut()
        {
            var authorization = Request.Cookies[Request.Url.Host + ".auth"];
            if (authorization != null)
            {
                authorization.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authorization);
            }
            var ssoUrl = Request.Cookies["ssourls"];
            List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrl.Value.Base64ToStr());
            return Redirect(ssoUrls[0] + "?ssourls=" + ssoUrl.Value);
        }


    }
}