using SSO.Web.Filters;
using SSO.Web.Models;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult IsLogin()
        {
            bool b = User.Identity.IsAuthenticated;
            return new ResponseModel<string>(ErrorCode.success, User.Identity.Name);
        }
        public ActionResult CheckAdmin()
        {
            return new ResponseModel<string>(ErrorCode.success, User.Identity.Name);
        }
    }
}