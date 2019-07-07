using SSO.Util;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost]
        public ActionResult Get(UserModel userModel)
        {
            if (userModel.UserName == "wang" && userModel.PassWord == "123")
            {
                return new ResponseModel<string>(ErrorCode.success, JwtManager.GenerateToken(userModel.UserName));
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.authorize_fault, "");
            }
        }
        [HttpGet]
        public ActionResult ValidateToken(string token)
        {
            string userName = "";
            if (JwtManager.ValidateToken(token, out userName))
            {
                return new ResponseModel<string>(ErrorCode.success, userName);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.invalid_token, userName);
            }
        }



    }
}