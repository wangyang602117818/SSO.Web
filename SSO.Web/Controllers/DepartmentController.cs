using SSO.Data.Models;
using SSO.Model;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class DepartmentController : BaseController
    {
        Business.Department department = new Business.Department();
        public ActionResult Add(DepartmentModel departmentModel)
        {
            if (department.GetByCode(departmentModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            InfoLog("0", "AddDepartment", departmentModel.Name);
            if (department.Insert(departmentModel.Code, departmentModel.Name, departmentModel.Description, departmentModel.CompanyCode, departmentModel.Order, departmentModel.Layer, departmentModel.ParentCode ?? "") > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetDepartments(string companyCode)
        {
            return new ResponseModel<List<DepartmentData>>(ErrorCode.success, department.GetDepartment(companyCode));
        }
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
        public ActionResult Update(UpdateDepartmentModel updateDepartmentModel)
        {
            InfoLog(updateDepartmentModel.Id.ToString(), "UpdateDepartment", updateDepartmentModel.Name);
            if (updateDepartmentModel.ParentCode == null) updateDepartmentModel.ParentCode = "";
            if (updateDepartmentModel.ParentCode == "")
            {
                updateDepartmentModel.Layer = 0;
            }
            else
            {
                updateDepartmentModel.Layer = department.GetByCode(updateDepartmentModel.ParentCode).Layer + 1;
            }
            if (department.Update(updateDepartmentModel.Id, updateDepartmentModel.Code, updateDepartmentModel.Name, updateDepartmentModel.Description, updateDepartmentModel.Order, updateDepartmentModel.ParentCode, updateDepartmentModel.Layer) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult Delete(int id)
        {
            InfoLog(id.ToString(), "DeleteDepartment");
            return new ResponseModel<int>(ErrorCode.success, department.Delete(id));
        }
    }
}