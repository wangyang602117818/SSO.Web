using SSO.Util;
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
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserModel userModel, string returnUrl)
        {
            if (userModel.UserName == "wang" && userModel.PassWord == "123")
            {
                string token = JwtManager.GenerateToken("cn445379", userModel.UserName, new string[] { "read", "edit" }, Request.UserHostAddress);
               
                return Redirect(returnUrl);
                return new ResponseModel<string>(ErrorCode.success, token);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }
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