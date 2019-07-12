using SSO.Util;
using SSO.Web.Models;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Get(UserModel userModel)
        {
            if (userModel.UserName == "wang" && userModel.PassWord == "123")
            {
                string token = JwtManager.GenerateToken("cn445379", userModel.UserName, new string[] { "read", "edit" }, Request.UserHostAddress);
                return new ResponseModel<string>(ErrorCode.success, token);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.login_fault, "");
            }

        }



    }
}