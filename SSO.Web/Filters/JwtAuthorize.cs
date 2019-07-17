using Microsoft.IdentityModel.Tokens;
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
            IEnumerable<CustomAttributeData> customAttributes = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.CustomAttributes;
            foreach (CustomAttributeData c in customAttributes)
            {
                if (c.AttributeType.Name == "AllowAnonymousAttribute") return;
            }
            HttpRequestBase request = filterContext.HttpContext.Request;
            string authorization = request.Cookies[request.Url.Host + ".auth"] == null ? "" : request.Cookies[request.Url.Host + ".auth"].Value;
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
                    filterContext.Result = new ResponseModel<string>(ErrorCode.token_expired, "");
                }
                catch (SecurityTokenExpiredException ex) //expried token
                {
                    filterContext.Result = new ResponseModel<string>(ErrorCode.token_expired, "");
                }
                catch (Exception ex)
                {
                    filterContext.Result = new ResponseModel<string>(ErrorCode.invalid_token, "");
                }
            }
        }
        public static ClaimsPrincipal ParseToken(string authorization)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = Convert.FromBase64String(JwtManager.secretKey);
            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidAudience = HttpContext.Current.Request.UserHostAddress,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
            };
            SecurityToken securityToken;
            return tokenHandler.ValidateToken(authorization, validationParameters, out securityToken);
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
    }
}