using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Business;
using SSO.Util;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var reflectedActionDescriptor = (ReflectedActionDescriptor)filterContext.ActionDescriptor;
            IEnumerable<CustomAttributeData> methodAttributes = reflectedActionDescriptor.MethodInfo.CustomAttributes;
            var controllerAttributes = reflectedActionDescriptor.ControllerDescriptor.GetCustomAttributes(true);
            bool isAuthorization = true;
            foreach (var item in controllerAttributes)
            {
                if (item.GetType().Name == "AllowAnonymousAttribute") isAuthorization = false;
                if (item.GetType().Name == "JwtAuthorizeAttribute") isAuthorization = true;
            }
            foreach (CustomAttributeData c in methodAttributes)
            {
                if (c.AttributeType.Name == "AllowAnonymousAttribute") isAuthorization = false;
                if (c.AttributeType.Name == "JwtAuthorizeAttribute") isAuthorization = true;
            }
            //如果设置了匿名访问直接返回
            if (!isAuthorization) return;
            HttpRequestBase request = filterContext.HttpContext.Request;
            string cookieName = request.Url.Host + ".auth";
            string authorization = request.Cookies[cookieName] == null ? "" : request.Cookies[cookieName].Value;
            if (string.IsNullOrEmpty(authorization)) authorization = request.Headers["Authorization"] ?? "";
            if (string.IsNullOrEmpty(authorization))
            {
                filterContext.Result = new ResponseModel<string>(ErrorCode.authorize_fault, "");
            }
            else
            {
                try
                {
                    filterContext.HttpContext.User = ParseToken(authorization);
                    if (!CheckRole(filterContext)) filterContext.Result = new ResponseModel<string>(ErrorCode.authorize_fault, "");
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
        public static ClaimsPrincipal ParseToken(string authorization)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = Convert.FromBase64String(AppSettings.secretKey);
            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateLifetime = false,
                ValidateIssuer = false,
                ValidAudience = HttpContext.Current.Request.UserHostAddress,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
            };
            SecurityToken securityToken;
            return tokenHandler.ValidateToken(authorization, validationParameters, out securityToken);
        }
        public static string AppendTicket(string url, string ticket)
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
        public static void AddUrlToCookie(HttpContextBase httpContext, string returnUrl)
        {
            if (returnUrl == null) return;
            HttpCookie ssoUrlCookie = httpContext.Request.Cookies["ssourls"];
            Uri uri = new Uri(returnUrl);
            if (uri.Query.Length > 0)
                returnUrl = returnUrl.Replace(uri.Query, "");

            Data.Models.Navigation navigation = new Navigation().GetBaseUrl(returnUrl);
            if (navigation != null) returnUrl = navigation.BaseUrl;
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
        private bool CheckRole(AuthorizationContext filterContext)
        {
            bool access = true;
            IEnumerable<CustomAttributeData> customAttributes = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.CustomAttributes;
            foreach (CustomAttributeData c in customAttributes)
            {
                if (c.AttributeType.Name == "JwtAuthorizeAttribute")
                {
                    if (c.NamedArguments.Count == 0) return access;
                    access = false;
                    foreach (var item in c.NamedArguments)
                    {
                        string[] name = item.TypedValue.Value.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string n in name)
                        {
                            if (filterContext.HttpContext.User.IsInRole(n)) access = true;
                        }
                    }
                }
            }
            return access;
        }
        private string DecodeBase64(string secureUrlBase64)
        {
            secureUrlBase64 = secureUrlBase64.Replace('-', '+').Replace('_', '/');
            switch (secureUrlBase64.Length % 4)
            {
                case 2:
                    secureUrlBase64 += "==";
                    break;
                case 3:
                    secureUrlBase64 += "=";
                    break;
            }
            return secureUrlBase64;
        }
    }
}