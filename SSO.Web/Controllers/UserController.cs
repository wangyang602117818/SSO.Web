using SSO.Model;
using SSO.Util.Client;
using SSO.Web.Filters;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class UserController : BaseController
    {
        Business.User user = new Business.User();
        [JwtAuthorize("GetUser")]
        public ActionResult Add(AddUserModel addUserModel)
        {
            if (user.GetUser(addUserModel.UserId) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            if (user.InsertUserDepartmentRole(addUserModel.UserId, addUserModel.UserName, addUserModel.Mobile, addUserModel.Email, addUserModel.CompanyCode, addUserModel.IdCard, addUserModel.Sex, addUserModel.Departments, addUserModel.Roles) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize("UpdateUser")]
        public ActionResult Update(UpdateUserModel updateUserModel)
        {
            if (updateUserModel.Departments == null) updateUserModel.Departments = new List<string>();
            if (updateUserModel.Roles == null) updateUserModel.Roles = new List<string>();
            int count = user.UpdateUserDepartmentRole(updateUserModel.Id, updateUserModel.UserId, updateUserModel.UserName, updateUserModel.Mobile, updateUserModel.Email, updateUserModel.CompanyCode, updateUserModel.IdCard, updateUserModel.Sex, updateUserModel.Departments, updateUserModel.Roles);
            if (count == 0) return new ResponseModel<string>(ErrorCode.record_exist, "");
            return new ResponseModel<string>(ErrorCode.success, "");
        }
        [JwtAuthorize("UpdateUser")]
        public ActionResult UpdateBasicSetting(UpdateUserModel updateUserModel)
        {
            if (updateUserModel.Departments == null) updateUserModel.Departments = new List<string>();
            int count = user.UpdateUserDepartmentRole(updateUserModel.Id, User.Identity.Name, updateUserModel.UserName, updateUserModel.Mobile, updateUserModel.Email, updateUserModel.CompanyCode, updateUserModel.IdCard, updateUserModel.Sex, updateUserModel.Departments, null);
            if (count == 0) return new ResponseModel<string>(ErrorCode.record_exist, "");
            return new ResponseModel<string>(ErrorCode.success, "");
        }
        [JwtAuthorize("UpdateUser")]
        [LogRecord(true, false)]
        public ActionResult UpdatePassword(UpdatePasswordModel updatePasswordModel)
        {
            int count = user.UpdatePassword(User.Identity.Name, updatePasswordModel.oldPassword, updatePasswordModel.newPassword);
            if (count == -1) return new ResponseModel<string>(ErrorCode.old_password_error, "");
            if (count >= 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize("UpdateUser")]
        public ActionResult ResetPassword(IEnumerable<string> userIds)
        {
            return new ResponseModel<int>(ErrorCode.success, user.ResetPassword(userIds));
        }
        [JwtAuthorize("GetUser")]
        public ActionResult GetBasic(string companyCode = "", string filter = "", string orderField = "Id", string orderType = "desc", int pageIndex = 1, int pageSize = 10, bool delete = false)
        {
            int count = 0;
            Data.Models.User page = new Data.Models.User()
            {
                CompanyCode = companyCode,
                UserName = filter,
                IsDelete = delete,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            object replacement = new { OrderField = orderField, OrderType = orderType };
            var result = user.GetPageList<Data.Models.User>(ref count, page, replacement);
            return new ResponseModel<IEnumerable<Data.Models.User>>(ErrorCode.success, result, count);
        }
        [JwtAuthorize("RemoveUser")]
        public ActionResult Remove(IEnumerable<string> userIds)
        {
            if (userIds == null || userIds.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            return new ResponseModel<int>(ErrorCode.success, user.RemoveUser(userIds));
        }
        [JwtAuthorize("DeleteUser")]
        public ActionResult Delete(IEnumerable<string> userIds)
        {
            if (userIds == null || userIds.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            var count = user.DeleteUser(userIds);
            if (count == -1) return new ResponseModel<string>(ErrorCode.record_has_been_used, "");
            if (count > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        [JwtAuthorize("RestoreUser")]
        public ActionResult Restore(IEnumerable<string> userIds)
        {
            if (userIds == null || userIds.Count() == 0) return new ResponseModel<int>(ErrorCode.success, 0);
            return new ResponseModel<int>(ErrorCode.success, user.RestoreUser(userIds));
        }
        [JwtAuthorize("GetUser")]
        public ActionResult GetUser()
        {
            Model.UserData userData = user.GetUserUpdate(User.Identity.Name);
            if (userData == null)
            {
                var userName = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "StaffName").Select(s => s.Value).FirstOrDefault();
                userData = new Model.UserData()
                {
                    UserId = User.Identity.Name,
                    UserName = userName,
                    Role = new List<string>() { admin[2] }
                };
            }
            return new ResponseModel<Model.UserData>(ErrorCode.success, userData);
        }
        [JwtAuthorize("GetUser")]
        public ActionResult GetByUserId(string userId)
        {
            return new ResponseModel<Model.UserData>(ErrorCode.success, user.GetUserUpdate(userId));
        }
    }
}