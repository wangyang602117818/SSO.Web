using MongoDB.Bson;
using SSO.Model;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class OverviewController : BaseController
    {
        Business.UserBasic userBasic = new Business.UserBasic();
        public ActionResult Total()
        {
            return new ResponseModel<OverviewTotal>(ErrorCode.success, userBasic.GetOverviewTotal());
        }
    }
}