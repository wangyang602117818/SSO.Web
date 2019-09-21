using SSO.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        Business.User user = new Business.User();
        public ActionResult Add(AddUserModel addUserModel)
        {
            if (user.GetUser(addUserModel.UserId) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (user.Insert(addUserModel.UserId, addUserModel.UserName, addUserModel.Password, addUserModel.Mobile, addUserModel.Email, addUserModel.CompanyCode, addUserModel.IdCard, addUserModel.Sex, addUserModel.Departments, addUserModel.Roles) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetBasic(string filter, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var result = user.GetBasic(ref count, filter, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.UserBasic>>(ErrorCode.success, result, count);
        }
        public ActionResult Delete(IEnumerable<string> userIds)
        {
            return new ResponseModel<int>(ErrorCode.success, user.DeleteUser(userIds));
        }
    }
}