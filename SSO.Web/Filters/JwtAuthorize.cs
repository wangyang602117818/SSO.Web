using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Filters
{
    public class JwtAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            string authorization = "";
            string authHeader = request.Headers["Authorization"];
            string authQuery = request["Authorization"];
            string authCookie = request.Cookies["Authorization"] == null ? "" : request.Cookies["Authorization"].Value;
            if (!string.IsNullOrEmpty(authHeader)) authorization = authHeader;
            if (!string.IsNullOrEmpty(authQuery)) authorization = authQuery;
            if (!string.IsNullOrEmpty(authCookie)) authorization = authCookie;
            if (string.IsNullOrEmpty(authorization))
            {
                filterContext.Result = new ResponseModel<string>(ErrorCode.authorize_fault, "");
            }
            else
            {

            }
            
        }
    }
}