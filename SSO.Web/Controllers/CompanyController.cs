using SSO.Util;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class CompanyController : BaseController
    {
        Business.Company company = new Business.Company();
        public ActionResult Add(CompanyModel companyModel)
        {
            if (company.GetByCode(companyModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            InfoLog("0", "AddCompany", companyModel.Name);
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
            InfoLog(updateCompanyModel.Id.ToString(), "UpdateCompany", updateCompanyModel.Name);
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
            InfoLog(ids.Select(s => s.ToString()), "DeleteCompany");
            return new ResponseModel<int>(ErrorCode.success, company.Delete(ids));
        }
    }
}