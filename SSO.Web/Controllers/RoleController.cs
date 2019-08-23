using SSO.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{

    public class RoleController : Controller
    {
        Business.Role role = new Business.Role();
        [AllowAnonymous]
        public ActionResult Add(RoleModel roleModel)
        {
            if (role.Insert(roleModel.Name, roleModel.Description) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [AllowAnonymous]
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var result = role.GetList(ref count, filter, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.Role>>(ErrorCode.success, result, count);
        }
    }
}