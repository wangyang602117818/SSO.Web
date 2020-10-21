using SSO.Business;
using SSO.Util.Client;
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
        public ActionResult AddRolePermission(RolePermissionModel rolePermissionModel)
        {
            roleMapping.DeleteMany(rolePermissionModel.Role);
            if (roleMapping.InsertMany(rolePermissionModel.Role, rolePermissionModel.Names) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetRolePermission(string role)
        {
            var result = roleMapping.GetByRole(role).Select(s => s.Permission);
            return new ResponseModel<IEnumerable<string>>(ErrorCode.success, result);
        }
        public ActionResult AddUserPermission(UserPermissionModel userPermissionModel)
        {
            userMapping.DeleteMany(userPermissionModel.User);
            if (userMapping.InsertMany(userPermissionModel.User, userPermissionModel.Names) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetUserPermission(string userId)
        {
            var result = userMapping.GetByUser(userId).Select(s => s.Permission);
            return new ResponseModel<IEnumerable<string>>(ErrorCode.success, result);
        }
    }
}