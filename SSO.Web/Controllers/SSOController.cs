using Newtonsoft.Json;
using SSO.Data.Models;
using SSO.Model;
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
    [AllowAnonymous]
    public class SSOController : Controller
    {
        Business.UserBasic user = new Business.UserBasic();
        Business.Navigation navigation = new Business.Navigation();
        HttpRequestHelper requestHelper = new HttpRequestHelper();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetUrlMeta()
        {
            return new ResponseModel<IEnumerable<Data.Models.Navigation>>(ErrorCode.success, navigation.GetAll());
        }
        [JwtAuthorize]
        public ActionResult GetUser()
        {
            return new ResponseModel<string>(ErrorCode.success, User.Identity.Name);
        }
        public ActionResult GetToken(string ticket, string ip)
        {
            string userId = JwtManager.DecodeTicket(ticket);
            string token = userId == "" ? "" : JwtManager.GenerateToken(userId, null, null, null, null, ip ?? Request.UserHostAddress, 20);
            return new ResponseModel<string>(ErrorCode.success, token);
        }
        public ActionResult Login(string returnUrl)
        {
            var authorization = Request.Cookies[AppSettings.cookieName];
            if (authorization != null)  //sso login
            {
                try
                {
                    var userId = JwtAuthorizeAttribute.ParseToken(authorization.Value).Identity.Name;
                    string ticket = JwtManager.GenerateTicket(userId);
                    returnUrl = JwtAuthorizeAttribute.AppendTicket(returnUrl, ticket);
                    if (AppSettings.cookieTime != "session")
                    {
                        authorization.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(AppSettings.cookieTime));
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
            string[] admin = AppSettings.admin.Split(';');
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
            if (userBasic == null) return new ResponseModel<string>(ErrorCode.login_fault, "");
            string[] roles = userBasic.RoleName.Split(',');
            string[] departments = userBasic.DepartmentName.Split(',');
            string token = JwtManager.GenerateToken(userBasic.UserId, userBasic.UserName, userBasic.CompanyName, departments, roles, Request.UserHostAddress, 24 * 60);
            HttpCookie httpCookie = new HttpCookie(AppSettings.cookieName, token);
            if (AppSettings.cookieTime != "session")
            {
                httpCookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(AppSettings.cookieTime));
            }
            Response.Cookies.Add(httpCookie);
            JwtAuthorizeAttribute.AddUrlToCookie(HttpContext, returnUrl);
            return new ResponseModel<string>(ErrorCode.success, returnUrl ?? "");
        }
        public ActionResult LogOut()
        {
            var authorization = Request.Cookies[AppSettings.cookieName];
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

        //[JwtAuthorize]
        public ActionResult AddNavigation(NavigationModel navigationModel)
        {
            if (navigation.Insert(navigationModel.Title, navigationModel.BaseUrl) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize]
        public ActionResult UpdateNavigation(UpdateNavigationModel updateNavigationModel)
        {
            if (navigation.Update(updateNavigationModel.Id, updateNavigationModel.Title, updateNavigationModel.BaseUrl) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.record_exist, "");
            }

        }
        [JwtAuthorize]
        public ActionResult GetNavigationById(int id)
        {
            return new ResponseModel<Data.Models.Navigation>(ErrorCode.success, navigation.GetById(id));
        }
        [JwtAuthorize]
        public ActionResult DeleteNavigation(IEnumerable<int> ids)
        {
            return new ResponseModel<int>(ErrorCode.success, navigation.Delete(ids));
        }
    }
}