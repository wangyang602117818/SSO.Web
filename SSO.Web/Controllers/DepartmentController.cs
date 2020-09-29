using SSO.Data.Models;
using SSO.Model;
using SSO.Util.Client;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class DepartmentController : BaseController
    {
        Business.Department department = new Business.Department();
        [PermissionDescription("AddDepartment")]
        public ActionResult Add(DepartmentModel departmentModel)
        {
            if (department.GetByCode(departmentModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (departmentModel.ParentCode == null || departmentModel.ParentCode.Trim() == "")
            {
                departmentModel.Layer = 0;
            }
            else
            {
                var dept = department.GetByCode(departmentModel.ParentCode);
                departmentModel.Layer = dept.Layer + 1;
            }
            if (department.Insert(departmentModel.Code, departmentModel.Name, departmentModel.Description, departmentModel.CompanyCode, departmentModel.Order, departmentModel.Layer, departmentModel.ParentCode ?? "") > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [PermissionDescription("GetDepartment")]
        public ActionResult GetDepartments(string companyCode)
        {
            return new ResponseModel<List<DepartmentData>>(ErrorCode.success, department.GetDepartment(companyCode));
        }
        [PermissionDescription("GetDepartment")]
        public ActionResult Get(string code)
        {
            var dept = department.GetByCode(code);
            var departmentData = new DepartmentData()
            {
                Id = dept.Id,
                Code = dept.Code,
                Name = dept.Name,
                Order = dept.Order,
                Layer = dept.Layer,
                ParentCode = dept.ParentCode,
                Description = dept.Description
            };
            return new ResponseModel<DepartmentData>(ErrorCode.success, departmentData);
        }
        [PermissionDescription("UpdateDepartment")]
        public ActionResult Update(UpdateDepartmentModel updateDepartmentModel)
        {
            if (updateDepartmentModel.ParentCode == null) updateDepartmentModel.ParentCode = "";
            if (updateDepartmentModel.ParentCode == "")
            {
                updateDepartmentModel.Layer = 0;
            }
            else
            {
                updateDepartmentModel.Layer = department.GetByCode(updateDepartmentModel.ParentCode).Layer + 1;
            }
            if (department.Update(updateDepartmentModel.Id, updateDepartmentModel.Code, updateDepartmentModel.Name, updateDepartmentModel.Description, updateDepartmentModel.Order, updateDepartmentModel.ParentCode, updateDepartmentModel.CompanyCode, updateDepartmentModel.Layer) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [PermissionDescription("DeleteDepartment")]
        public ActionResult Delete(int id)
        {
            return new ResponseModel<int>(ErrorCode.success, department.Delete(id));
        }
    }
}