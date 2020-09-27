﻿using SSO.Business;
using SSO.Data.Models;
using SSO.Util;
using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        [PermissionDescription("Index")]
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(w => w.FullName.Contains("SSO.Web.Controllers"));
            var actions = PermissionDescriptionAttribute.GetPermissionDescription(controllers);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ticket()
        {
            return Content(jwtManager.GenerateTicket("CN445379"));
        }
        public ActionResult Decode(string ticket)
        {
            return Content(jwtManager.DecodeTicket(ticket));
        }
        public ActionResult DecodeToken(string token)
        {
            var result = JwtManager.ParseAuthorization(token, ssoSecretKey);
            return Content(result.Identity.Name);
        }
    }
}