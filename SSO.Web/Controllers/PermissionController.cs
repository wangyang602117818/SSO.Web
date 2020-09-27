using SSO.Business;
using SSO.Util.Client;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class PermissionController : BaseController
    {
        Permission permission = new Permission();
        public ActionResult Add(PermissionModel permissionModel)
        {
            permission.DeleteMany(permissionModel.Origin);
            if (permission.InsertMany(permissionModel.Origin, permissionModel.Names) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
    }
}