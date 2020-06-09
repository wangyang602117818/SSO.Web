using SSO.Util;
using SSO.Util.Client;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class CompanyController : BaseController
    {
        Business.Company company = new Business.Company();
        Business.Department department = new Business.Department();
        public ActionResult Add(CompanyModel companyModel)
        {
            if (company.GetByCode(companyModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (company.Insert(companyModel.Code, companyModel.Name, companyModel.Description, companyModel.Order) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult Update(UpdateCompanyModel updateCompanyModel)
        {
            if (company.Update(updateCompanyModel.Id, updateCompanyModel.Code, updateCompanyModel.Name, updateCompanyModel.Description, updateCompanyModel.Order) > 0)
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
            return new ResponseModel<Data.Models.Company>(ErrorCode.success, company.GetById(id));
        }
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var result = company.GetList(ref count, filter, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.Company>>(ErrorCode.success, result, count);
        }
        public ActionResult GetAll(string filter = "")
        {
            var result = company.GetAll(filter);
            return new ResponseModel<IEnumerable<Data.Models.Company>>(ErrorCode.success, result, result.Count());
        }
        public ActionResult Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            foreach (var id in ids)
            {
                Data.Models.Company com = company.GetById(id);
                int count = department.CountDepartmentByCompanyCode(com.Code);
                if (count > 0)
                {
                    return new ResponseModel<string>(ErrorCode.record_exist,"");
                }
            }
            return new ResponseModel<int>(ErrorCode.success, company.Delete(ids));
        }
    }
}