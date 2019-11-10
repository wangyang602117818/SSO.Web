using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace A.Web.Filters
{
    public class SSOAuthorizeAttribute : AuthorizeAttribute
    {
        public static string secretKey = System.Configuration.ConfigurationManager.AppSettings["ssoSecretKey"];
        public static string baseUrl = System.Configuration.ConfigurationManager.AppSettings["ssoBaseUrl"];
        public static string cookieKey = System.Configuration.ConfigurationManager.AppSettings["ssoCookieKey"];
        public static string cookieTime = System.Configuration.ConfigurationManager.AppSettings["ssoCookieTime"];
        public static string loginUrl = baseUrl.TrimEnd('/') + "/sso/login";
        public static string indexUrl = baseUrl.TrimEnd('/') + "/sso/index";
        public static string getTokenUrl = baseUrl.TrimEnd('/') + "/sso/gettoken";
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
            if (!isAuthorization) return;
            HttpRequestBase request = filterContext.HttpContext.Request;
            //string cookieName = request.Url.Host + ".auth";
            var ssourl = request.QueryString["ssourls"];
            if (!string.IsNullOrEmpty(ssourl)) //sso 退出
            {
                ////////清除本站cookie
                List<string> ssoUrls = JsonConvert.DeserializeObject<List<string>>(Encoding.UTF8.GetString(Convert.FromBase64String(DecodeBase64(ssourl))));
                var cookie = request.Cookies[cookieKey];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    filterContext.HttpContext.Response.Cookies.Add(cookie);
                }
                /////////////////////
                int index = 0;
                for (var i = 0; i < ssoUrls.Count; i++) if (request.Url.AbsoluteUri.Contains(ssoUrls[i])) index = i;
                if (index < ssoUrls.Count - 1)
                {
                    filterContext.Result = new RedirectResult(ssoUrls[index + 1] + "?ssourls=" + ssourl);
                }
                else //最后一个
                {
                    filterContext.Result = new RedirectResult(indexUrl);
                }
                return;
            }
            string authorization = request.Cookies[cookieKey] == null ? "" : request.Cookies[cookieKey].Value;
            if (string.IsNullOrEmpty(authorization)) authorization = request.Headers["Authorization"] ?? "";
            string ticket = request.QueryString["ticket"];
            if (string.IsNullOrEmpty(authorization))
            {
                if (string.IsNullOrEmpty(ticket))
                {
                    filterContext.Result = new RedirectResult(loginUrl + "?returnUrl=" + request.Url);
                    return;
                }
                else
                {
                    authorization = GetTokenByTicket(ticket, request.UserHostAddress);
                    if (!string.IsNullOrEmpty(authorization))
                    {
                        HttpCookie httpCookie = new HttpCookie(cookieKey, authorization);
                        if (cookieTime != "session")
                        {
                            httpCookie.Expires = DateTime.Now.AddMinutes(Convert.ToInt32(cookieTime));
                        }
                        filterContext.HttpContext.Response.Cookies.Add(httpCookie);
                    }
                }
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var symmetricKey = Convert.FromBase64String(secretKey);
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
                var principal = tokenHandler.ValidateToken(authorization, validationParameters, out securityToken);
                filterContext.HttpContext.User = principal;

                if (!CheckRole(filterContext)) filterContext.Result = new HttpUnauthorizedResult();
            }
            catch (Exception ex) //token失效
            {
                HttpCookie httpCookie = filterContext.HttpContext.Request.Cookies[cookieKey];
                if (httpCookie != null)
                {
                    httpCookie.Expires = DateTime.Now.AddDays(-1);
                    filterContext.HttpContext.Response.Cookies.Add(httpCookie);
                }
                filterContext.Result = new RedirectResult(loginUrl + "?returnUrl=" + request.Url);
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
        public static string GetTokenByTicket(string ticket, string audience)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getTokenUrl + "?ticket=" + ticket + "&ip=" + audience);
            request.Method = "get";
            using (WebResponse response = request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                var result = JsonConvert.DeserializeObject<ResponseItem<string>>(reader.ReadToEnd());
                if (result.code == 0) return result.result;
                return "";
            }
        }
        private string Encode(string text)
        {
            return text.Replace('+', '-').Replace('/', '_').TrimEnd('=');
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
    public class ResponseItem<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T result { get; set; }
    }
}