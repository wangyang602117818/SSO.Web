using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Util.Client;
using SSO.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Filters
{
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public static string ssoCookieKey = AppSettings.GetValue("ssoCookieKey");
        public static string ssoSecretKey = AppSettings.GetValue("ssoSecretKey");
        public string Name = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">权限名称,到数据库查询是否有权限</param>
        public JwtAuthorizeAttribute(string name = "")
        {
            Name = name;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var reflectedActionDescriptor = (ReflectedActionDescriptor)filterContext.ActionDescriptor;
            IEnumerable<CustomAttributeData> methodAttributes = reflectedActionDescriptor.MethodInfo.CustomAttributes;
            IEnumerable<CustomAttributeData> controllerAttributes = reflectedActionDescriptor.ControllerDescriptor.ControllerType.CustomAttributes;
            bool isAuthorization = true;
            string permissionName = "";
            foreach (CustomAttributeData item in controllerAttributes)
            {
                if (item.AttributeType.Name == "AllowAnonymousAttribute") isAuthorization = false;
                if (item.AttributeType.Name == "JwtAuthorizeAttribute")
                {
                    isAuthorization = true;
                    if (item.ConstructorArguments.Count > 0)
                    {
                        permissionName = item.ConstructorArguments[0].Value.ToString();
                    }
                }
            }
            foreach (CustomAttributeData item in methodAttributes)
            {
                if (item.AttributeType.Name == "AllowAnonymousAttribute") isAuthorization = false;
                if (item.AttributeType.Name == "JwtAuthorizeAttribute")
                {
                    isAuthorization = true;
                    if (item.ConstructorArguments.Count > 0)
                    {
                        permissionName = item.ConstructorArguments[0].Value.ToString();
                    }
                }
            }
            //如果设置了匿名访问直接返回
            if (!isAuthorization) return;
            HttpRequestBase request = filterContext.HttpContext.Request;
            string authorization = JwtManager.GetAuthorization(request, ssoCookieKey);
            if (string.IsNullOrEmpty(authorization))
            {
                filterContext.Result = new ResponseModel<string>(ErrorCode.authorize_fault, "");
            }
            else
            {
                if (!filterContext.HttpContext.Items.Contains("Authorization"))
                {
                    filterContext.HttpContext.Items.Add("Authorization", authorization);
                }
                try
                {
                    var principal = JwtManager.ParseAuthorization(authorization, ssoSecretKey);
                    filterContext.HttpContext.User = principal;
                    if (!CheckPermission(permissionName, filterContext.HttpContext.User.Identity.Name)) filterContext.Result = new ResponseModel<string>(ErrorCode.error_permission, "");
                }
                catch (SecurityTokenInvalidAudienceException ex) //Audience Error 
                {
                    Log4Net.ErrorLog(ex);
                    filterContext.Result = new ResponseModel<string>(ErrorCode.invalid_token, "");
                }
                catch (SecurityTokenExpiredException ex) //expried token
                {
                    Log4Net.ErrorLog(ex);
                    filterContext.Result = new ResponseModel<string>(ErrorCode.token_expired, "");
                }
                catch (Exception ex)
                {
                    Log4Net.ErrorLog(ex);
                    filterContext.Result = new ResponseModel<string>(ErrorCode.invalid_token, "");
                }
            }
        }
        public static string AppendTicket(string returnUrl, string ticket)
        {
            if (returnUrl.IsNullOrEmpty()) return returnUrl;
            if (returnUrl.Contains("ticket"))
            {
                var index = returnUrl.IndexOf("ticket");
                returnUrl = returnUrl.Substring(0, index - 1);
            }
            if (returnUrl.Contains("?"))
            {
                returnUrl += "&ticket=" + ticket;
            }
            else
            {
                returnUrl += "?ticket=" + ticket;
            }
            return returnUrl;
        }
        public static void AddUrlToCookie(HttpContextBase httpContext, string returnUrl)
        {
            if (returnUrl.IsNullOrEmpty()) return;
            HttpCookie ssoUrlCookie = httpContext.Request.Cookies["ssourls"];
            Uri uri = new Uri(returnUrl);
            if (uri.Query.Length > 0)
                returnUrl = returnUrl.Replace(uri.Query, "");
            returnUrl = returnUrl.TrimEnd('/');
            if (ssoUrlCookie == null)
            {
                string returnUrls = JsonConvert.SerializeObject(new List<string>() { returnUrl });
                HttpCookie cookie = new HttpCookie("ssourls", returnUrls.StrToBase64());
                httpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(ssoUrlCookie.Value.Base64ToStr());
                if (!ssoUrls.Contains(returnUrl)) ssoUrls.Add(returnUrl);
                string returnUrls = JsonConvert.SerializeObject(ssoUrls);
                HttpCookie cookie = new HttpCookie("ssourls", returnUrls.StrToBase64());
                httpContext.Response.Cookies.Add(cookie);
            }
        }
        private bool CheckPermission(string permission, string userId)
        {
            if (permission.IsNullOrEmpty()) return true;
            if (userId == BaseController.admin[0]) return true;
            Business.Permission per = new Business.Permission();
            if (per.CheckPermission(userId, permission) > 0) return true;
            return false;
        }
        /// <summary>
        /// 获取程序集所有带有 SSOAuthorizeAttribute 的名称列表
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static List<string> GetPermissionDescription(IEnumerable<Type> types)
        {
            List<string> actions = new List<string>();
            foreach (var item in types)
            {
                var methods = item.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(JwtAuthorizeAttribute));
                    foreach (Attribute att in attributes)
                    {
                        var name = ((JwtAuthorizeAttribute)att).Name;
                        if (!actions.Contains(name)) actions.Add(name);
                    }
                }
            }
            return actions;
        }
    }
}