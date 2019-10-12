using Newtonsoft.Json;
using SSO.Model;
using SSO.Util;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class SSOController : Controller
    {
        public static int cookieTime = int.Parse(ConfigurationManager.AppSettings["cookieTime"]);
        Business.UserBasic user = new Business.UserBasic();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetUrlMeta()
        {
            List<string> ssoUrls = new List<string>() { JwtAuthorizeAttribute.web };
            var ssoUrlsCookie = Request.Cookies["ssourls"];
            if (ssoUrlsCookie != null)
                ssoUrls.AddRange(JsonConvert.DeserializeObject<List<string>>(ssoUrlsCookie.Value.Base64ToStr()));
            List<WebsiteMeta> websiteMetas = new List<WebsiteMeta>();
            foreach (string url in ssoUrls)
            {
                websiteMetas.Add(url.GetWebSiteMeta());
            }
            return new ResponseModel<List<WebsiteMeta>>(ErrorCode.success, websiteMetas);
        }
        //public ActionResult GetIcon(string url)
        //{
        //    Stream stream = new MemoryStream();
        //    try
        //    {
        //        stream = new HttpRequestHelper().GetFile(url, null);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    if(stream.Length==0) return ;
        //}
        public ActionResult GetToken(string ticket, string ip)
        {
            string userId = JwtManager.DecodeTicket(ticket);
            string token = userId == "" ? "" : JwtManager.GenerateToken(userId, null, null, null, null, ip, 20);
            return new ResponseModel<string>(ErrorCode.success, token);
        }
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
            var userBasic = user.Login(loginModel.UserId, loginModel.PassWord.GetSha256());
            if (userBasic == null) return new ResponseModel<string>(ErrorCode.login_fault, "");
            string[] roles = userBasic.RoleName.Split(',');
            string[] departments = userBasic.DepartmentName.Split(',');
            string token = JwtManager.GenerateToken(userBasic.UserId, userBasic.UserName, userBasic.CompanyName, departments, roles, Request.UserHostAddress, 24 * 60);
            HttpCookie httpCookie = new HttpCookie(Request.Url.Host + ".auth", token);
            Response.Cookies.Add(httpCookie);
            JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
            return new ResponseModel<string>(ErrorCode.success, returnUrl ?? "");
        }
        public ActionResult LogOut()
        {
            var authorization = Request.Cookies[Request.Url.Host + ".auth"];
            if (authorization != null)
            {
                authorization.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authorization);
            }
            var ssoUrlCookie = Request.Cookies["ssourls"];
            if (ssoUrlCookie == null) return RedirectToAction("Index");
            List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrlCookie.Value.Base64ToStr());
            return Redirect(ssoUrls[0] + "?ssourls=" + ssoUrlCookie.Value);
        }

    }
}