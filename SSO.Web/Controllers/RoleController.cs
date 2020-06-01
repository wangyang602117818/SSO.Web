using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class RoleController : BaseController
    {
        Business.Role role = new Business.Role();
        public ActionResult Add(RoleModel roleModel)
        {
            if (role.GetByRoleName(roleModel.Name) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (role.Insert(roleModel.Name, roleModel.Description) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult Update(UpdateRoleModel updateRoleModel)
        {
            if (role.Update(updateRoleModel.Id, updateRoleModel.Name, updateRoleModel.Description) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.record_exist, "");
            }
        }
        public ActionResult GetById(int id)
        {
            return new ResponseModel<Data.Models.Role>(ErrorCode.success, role.GetById(id));
        }
        [NoneLogRecord]
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var result = role.GetList(ref count, filter, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.Role>>(ErrorCode.success, result, count);
        }
        [NoneLogRecord]
        public ActionResult GetAll()
        {
            var result = role.GetAll();
            return new ResponseModel<IEnumerable<Data.Models.Role>>(ErrorCode.success, result, result.Count());
        }
        public ActionResult Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            return new ResponseModel<int>(ErrorCode.success, role.Delete(ids));
        }
    }
}