using SSO.Web.Models;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class DepartmentController : Controller
    {
        Business.Department department = new Business.Department();
        public ActionResult Add(DepartmentModel departmentModel)
        {
            if (department.GetByCode(departmentModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (department.Insert(departmentModel.Code, departmentModel.Name, departmentModel.Description, departmentModel.CompanyCode, departmentModel.Order, departmentModel.Layer, departmentModel.ParentCode ?? "") > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetDepartment(string companyCode)
        {
            return new ResponseModel<Data.Models.Department[]>(ErrorCode.success, department.GetDepartment(companyCode));
        }
    }
}