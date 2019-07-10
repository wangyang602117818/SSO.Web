using SSO.Util;
using SSO.Web.Models;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Get()
        {
            //if (userModel.UserName == "wang" && userModel.PassWord == "123")
            //{
                return new ResponseModel<string>(ErrorCode.success, JwtManager.GenerateToken("123", "wang", new string[] { "read","edit" }, Request.UserHostAddress));
            //}
            //else
            //{
            //    return new ResponseModel<string>(ErrorCode.authorize_fault, "");
            //}
            
        }



    }
}