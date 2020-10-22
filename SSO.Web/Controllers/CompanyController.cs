using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class CompanyController : BaseController
    {
        Business.Company company = new Business.Company();
        [JwtAuthorize("AddCompany")]
        public ActionResult Add(CompanyModel companyModel)
        {
            if (company.GetByCode(companyModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            Data.Models.Company com = new Data.Models.Company()
            {
                Code = companyModel.Code,
                Name = companyModel.Name,
                Description = companyModel.Description,
                Order = companyModel.Order
            };
            if (company.Insert(com) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize("UpdateCompany")]
        public ActionResult Update(UpdateCompanyModel updateCompanyModel)
        {
            Data.Models.Company com = new Data.Models.Company()
            {
                Id = updateCompanyModel.Id,
                Code = updateCompanyModel.Code,
                Name = updateCompanyModel.Name,
                Description = updateCompanyModel.Description,
                Order = updateCompanyModel.Order,
                UpdateTime = DateTime.Now
            };
            if (company.Update(com) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.record_exist, "");
            }
        }
        [JwtAuthorize("GetCompany")]
        public ActionResult GetById(int id)
        {
            return new ResponseModel<Data.Models.Company>(ErrorCode.success, company.GetById(id));
        }
        [JwtAuthorize("GetCompany")]
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            Data.Models.Company com = new Data.Models.Company()
            {
                Name = filter,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var result = company.GetPageList(ref count, com);
            return new ResponseModel<IEnumerable<Data.Models.Company>>(ErrorCode.success, result, count);
        }
        [JwtAuthorize("GetCompany")]
        public ActionResult GetAll(string filter = "")
        {
            Data.Models.Company com = new Data.Models.Company()
            {
                Name = filter
            };
            var result = company.GetAll(com);
            return new ResponseModel<IEnumerable<Data.Models.Company>>(ErrorCode.success, result, result.Count());
        }
        [JwtAuthorize("DeleteCompany")]
        public ActionResult Delete(IEnumerable<int> ids)
        {
            if (ids == null || ids.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            return new ResponseModel<int>(ErrorCode.success, company.Delete(ids));
        }
    }
}