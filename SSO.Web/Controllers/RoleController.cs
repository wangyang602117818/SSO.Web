using SSO.Web.Models;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class RoleController : Controller
    {
        Business.Role role = new Business.Role();
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
    }
}