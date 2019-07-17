using SSO.Util;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class SSOController : Controller
    {
        [AllowAnonymous]
        public ActionResult GetToken(string ticket)
        {
            string userId = JwtManager.DecodeTicket(ticket);
            if (userId == "") return new ResponseModel<string>(ErrorCode.success, "");
            string token = JwtManager.GenerateToken(userId, "", new string[] { "read", "edit" }, Request.UserHostAddress, 60);
            return new ResponseModel<string>(ErrorCode.success, token);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var authorization = Request.Cookies[Request.Url.Host + ".auth"];
            //其他系统登录
            if (authorization != null)
            {
                try
                {
                    var userId = JwtAuthorizeAttribute.ParseToken(authorization.Value).Identity.Name;
                    string ticket = JwtManager.GenerateTicket(userId);
                    returnUrl = AppendTicket(returnUrl, ticket);
                    return Redirect(returnUrl);
                }
                catch (Exception ex)
                {

                }
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserModel userModel, string returnUrl)
        {
            if (userModel.UserName == "wang" && userModel.PassWord == "123")
            {
                string userId = "CN445379";
                string ticket = JwtManager.GenerateTicket(userId);
                returnUrl = AppendTicket(returnUrl, ticket);
                string token = JwtManager.GenerateToken(userId, "", null, Request.UserHostAddress, 60 * 24);
                Response.Cookies.Add(new System.Web.HttpCookie(Request.Url.Host + ".auth", token));
                return Redirect(returnUrl);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
        }
        private string AppendTicket(string url, string ticket)
        {
            if (url.Contains("ticket"))
            {
                var index = url.IndexOf("ticket");
                url = url.Substring(0, index - 1);
            }
            if (url.Contains("?"))
            {
                url += "&ticket=" + ticket;
            }
            else
            {
                url += "?ticket=" + ticket;
            }
            return url;
        }

    }
}