using SSO.Business;
using SSO.Util;
using SSO.Web.Filters;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            new Role().Insert("dd", "sss");
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
        [AllowAnonymous]
        public string M()
        {
            Response.Cookies.Add(new System.Web.HttpCookie("name", "wang"));
            return "ok";
        }
        [AllowAnonymous]
        public string M2()
        {
            string str = "none";
            if (Request.Cookies["name"] != null) str = Request.Cookies["name"].Value;
            return str;
        }
    }
}