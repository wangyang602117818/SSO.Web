﻿using SSO.Util;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Filters
{
    public class MyHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            Log4Net.ErrorLog(filterContext.Exception);
            filterContext.Result = new ResponseModel<string>(ErrorCode.server_exception, filterContext.Exception.Message);
        }
    }
}