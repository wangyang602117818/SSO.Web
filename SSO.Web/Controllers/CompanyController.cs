﻿using SSO.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class CompanyController : Controller
    {
        Business.Company company = new Business.Company();
        public ActionResult Add(CompanyModel companyModel)
        {
            if (company.GetByCode(companyModel.Code) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (company.Insert(companyModel.Code, companyModel.Name, companyModel.Description) > 0)
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
            if (company.Update(updateCompanyModel.Id, updateCompanyModel.Code, updateCompanyModel.Name, updateCompanyModel.Description) > 0)
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
        public ActionResult Delete(IEnumerable<int> ids)
        {
            return new ResponseModel<int>(ErrorCode.success, company.Delete(ids));
        }
    }
}