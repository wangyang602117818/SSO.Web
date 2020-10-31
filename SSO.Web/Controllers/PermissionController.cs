using SSO.Business;
using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class PermissionController : BaseController
    {
        Permission permission = new Permission();
        PermissionRoleMapping roleMapping = new PermissionRoleMapping();
        PermissionUserMapping userMapping = new PermissionUserMapping();
        [JwtAuthorize("AddPermission")]
        public ActionResult Add(PermissionModel permissionModel)
        {
            if (permission.DeleteAndInsertMany(permissionModel.Origin, permissionModel.Names) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize("GetPermission")]
        public ActionResult GetPermission()
        {
            var list = permission.GetAll().OrderBy(o => o.Name);
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            foreach (var item in list)
            {
                if (!result.ContainsKey(item.Origin)) result.Add(item.Origin, new List<string>());
                result[item.Origin].Add(item.Name);
            }
            return new ResponseModel<Dictionary<string, List<string>>>(ErrorCode.success, result);
        }
        [JwtAuthorize("AddPermission")]
        public ActionResult AddRolePermission(RolePermissionModel rolePermissionModel)
        {
            int count = roleMapping.DeleteAndInsertMany(rolePermissionModel.Role, rolePermissionModel.Names);
            if (count > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "", count);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "", count);
            }
        }
        [JwtAuthorize("GetPermission")]
        public ActionResult GetRolePermission(string role)
        {
            var result = roleMapping.GetByRole(role).Select(s => s.Permission);
            return new ResponseModel<IEnumerable<string>>(ErrorCode.success, result);
        }
        [JwtAuthorize("AddPermission")]
        public ActionResult AddUserPermission(UserPermissionModel userPermissionModel)
        {
            int count = userMapping.DeleteAndInsertMany(userPermissionModel.User, userPermissionModel.Names);
            if (count > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "", count);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "", count);
            }
        }
        [JwtAuthorize("GetPermission")]
        public ActionResult GetUserPermission(string userId)
        {
            var result = userMapping.GetByUser(userId).Select(s => s.Permission);
            return new ResponseModel<IEnumerable<string>>(ErrorCode.success, result);
        }
        //[OutputCache(Duration = 60 * 25, VaryByParam = "*", VaryByHeader = "Authorization")]
        public ActionResult CheckPermission(string permissionName)
        {
            if (permission.CheckPermission(User.Identity.Name, permissionName) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.error_permission, "");
            }
        }
    }
}