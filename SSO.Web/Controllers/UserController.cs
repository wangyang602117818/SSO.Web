using SSO.Model;
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
        public ActionResult Update(UpdateUserModel updateUserModel)
        {
            if (updateUserModel.Departments == null) updateUserModel.Departments = new List<string>();
            if (updateUserModel.Roles == null) updateUserModel.Roles = new List<string>();
            int count = user.Update(updateUserModel.Id, updateUserModel.UserId, updateUserModel.UserName, updateUserModel.Password, updateUserModel.Mobile, updateUserModel.Email, updateUserModel.CompanyCode, updateUserModel.IdCard, updateUserModel.Sex, updateUserModel.Departments, updateUserModel.Roles);
            if (count == 0) return new ResponseModel<string>(ErrorCode.record_exist, "");
            return new ResponseModel<string>(ErrorCode.success, "");
        }
        public ActionResult GetBasic(string filter, int pageIndex = 1, int pageSize = 10, bool delete = false)
        {
            int count = 0;
            var result = user.GetBasic(ref count, filter, delete, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.UserBasic>>(ErrorCode.success, result, count);
        }
        public ActionResult Delete(IEnumerable<string> userIds)
        {
            return new ResponseModel<int>(ErrorCode.success, user.DeleteUser(userIds));
        }
        public ActionResult Restore(IEnumerable<string> userIds)
        {
            return new ResponseModel<int>(ErrorCode.success, user.RestoreUser(userIds));
        }
        public ActionResult GetByUserId(string userId)
        {
            return new ResponseModel<UserBasicData>(ErrorCode.success, user.GetUserUpdate(userId));
        }
    }
}