using MongoDB.Bson;
using Newtonsoft.Json;
using SSO.Data.Models;
using SSO.Model;
using SSO.Util;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class SSOController : BaseController
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
        public ActionResult GetToken(string ticket, string ip)
        {
            string token = "";
            string userId = JwtManager.DecodeTicket(ticket);
            if (!userId.IsNullOrEmpty())
            {
                UserBasic userBasic = user.GetUser(userId);
                if (userBasic == null)
                {
                    if (userId == AppSettings.admin[0])
                    {
                        token = JwtManager.GenerateToken(userId, userId, null, null, new List<string>() { AppSettings.admin[2] }, ip ?? Request.UserHostAddress, 20);
                    }
                }
                else
                {
                    string[] departments = userBasic.DepartmentName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] roles = userBasic.RoleName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    token = JwtManager.GenerateToken(userId, userBasic.UserName, userBasic.CompanyName, departments, roles, ip ?? Request.UserHostAddress, 20);
                }
            }
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
            if (loginModel.UserId == AppSettings.admin[0] && loginModel.PassWord == AppSettings.admin[1])
            {
                userBasic = new UserBasic()
                {
                    UserId = loginModel.UserId,
                    UserName = loginModel.UserId,
                    RoleName = AppSettings.admin[2],
                    DepartmentName = ""
                };
            }
            else
            {
                userBasic = user.Login(loginModel.UserId, loginModel.PassWord.GetSha256());
            }
            if (userBasic == null)
            {
                InfoLog("0", "LoginFault");
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
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
            if (returnUrl.IsNullOrEmpty()) returnUrl = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port + Request.ApplicationPath;
            InfoLog("0", "Login");
            return new ResponseModel<string>(ErrorCode.success, returnUrl);
        }
        [JwtAuthorize]
        public ActionResult LogOut()
        {
            InfoLog("0", "LogOut");
            var authorization = Request.Cookies[AppSettings.cookieName];
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
            BsonDocument userRole = new BsonDocument()
            {
                {"UserId",User.Identity.Name },
                {"UserName",User.Identity.Name },
                {"Role",new BsonArray(roles) },
            };
            return new ResponseModel<BsonDocument>(ErrorCode.success, userRole);
        }
        [JwtAuthorize]
        public ActionResult AddNavigation(NavigationModel navigationModel)
        {
            InfoLog("0", "AddNavigation");
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
            InfoLog(updateNavigationModel.Id.ToString(), "UpdateNavigation");
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
            InfoLog(ids.Select(s=>s.ToString()), "DeleteNavigation");
            return new ResponseModel<int>(ErrorCode.success, navigation.Delete(ids));
        }
    }
}