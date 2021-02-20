using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class RoleController : BaseController
    {
        Business.Role role = new Business.Role();
        [JwtAuthorize("AddRole")]
        public ActionResult Add(RoleModel roleModel)
        {
            if (role.GetByName(roleModel.Name) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            Data.Models.Role r = new Data.Models.Role()
            {
                Name = roleModel.Name,
                PermissionCount = 0,
                Description = roleModel.Description,
                CreateTime = DateTime.Now
            };
            if (role.Insert(r) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize("UpdateRole")]
        public ActionResult Update(UpdateRoleModel updateRoleModel)
        {
            Data.Models.Role r = new Data.Models.Role()
            {
                Id = updateRoleModel.Id,
                Name = updateRoleModel.Name,
                Description = updateRoleModel.Description,
                UpdateTime = DateTime.Now
            };
            if (role.Update(r) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.record_exist, "");
            }
        }
        [JwtAuthorize("GetRole")]
        public ActionResult GetById(int id)
        {
            return new ResponseModel<Data.Models.Role>(ErrorCode.success, role.GetById(id));
        }
        [JwtAuthorize("GetRole")]
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var r = new Data.Models.Role()
            {
                Name = filter,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = role.GetPageList<Data.Models.Role>(ref count, r);
            return new ResponseModel<IEnumerable<Data.Models.Role>>(ErrorCode.success, result, count);
        }
        [JwtAuthorize("GetRole")]
        public ActionResult GetAll()
        {
            var result = role.GetAll(null);
            return new ResponseModel<IEnumerable<Data.Models.Role>>(ErrorCode.success, result, result.Count());
        }
        [JwtAuthorize("DeleteRole")]
        public ActionResult Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            var count = role.Delete(ids);
            if (count == -1) return new ResponseModel<string>(ErrorCode.record_has_been_used, "");
            if (count > 0)
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