﻿using SSO.Business;
using SSO.Util;
using SSO.Web.Filters;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ticket()
        {
            return Content(JwtManager.GenerateTicket("CN445379"));
        }
        public ActionResult Decode(string ticket)
        {
            return Content(JwtManager.DecodeTicket(ticket));
        }
        public ActionResult DecodeToken(string token)
        {
            var result = JwtAuthorizeAttribute.ParseToken(token);
            return Content(result.Identity.Name);
        }
        public string M()
        {
            Response.Cookies.Add(new System.Web.HttpCookie("name", "wang"));
            return "ok";
        }
        public string M2()
        {
            string str = "none";
            if (Request.Cookies["name"] != null) str = Request.Cookies["name"].Value;
            return str;
        }
    }
}