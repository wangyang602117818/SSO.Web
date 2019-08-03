using SSO.Util;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content(User.Identity.Name);
            return View();
        }
        [AllowAnonymous]
        public ActionResult Ticket()
        {
            return Content(JwtManager.GenerateTicket("CN445379"));
        }
        [AllowAnonymous]
        public ActionResult Decode(string ticket)
        {
            return Content(JwtManager.DecodeTicket(ticket));
        }
        [AllowAnonymous]
        public ActionResult DecodeToken(string token)
        {
            var result = JwtAuthorizeAttribute.ParseToken(token);
            return Content(result.Identity.Name);
        }
        
    }
}