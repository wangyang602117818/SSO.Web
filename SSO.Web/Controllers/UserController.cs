﻿using SSO.Model;
using SSO.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class UserController : BaseController
    {
        Business.UserBasic user = new Business.UserBasic();
        public ActionResult Add(AddUserModel addUserModel)
        {
            if (user.GetUser(addUserModel.UserId) != null) return new ResponseModel<string>(ErrorCode.record_exist, "");
            InfoLog("0", "AddUser", addUserModel.UserName);
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
            InfoLog(updateUserModel.Id.ToString(), "UpdateUser", updateUserModel.UserName);
            if (updateUserModel.Departments == null) updateUserModel.Departments = new List<string>();
            if (updateUserModel.Roles == null) updateUserModel.Roles = new List<string>();
            int count = user.Update(updateUserModel.Id, updateUserModel.UserId, updateUserModel.UserName, updateUserModel.Password, updateUserModel.Mobile, updateUserModel.Email, updateUserModel.CompanyCode, updateUserModel.IdCard, updateUserModel.Sex, updateUserModel.Departments, updateUserModel.Roles);
            if (count == 0) return new ResponseModel<string>(ErrorCode.record_exist, "");
            return new ResponseModel<string>(ErrorCode.success, "");
        }
        public ActionResult UpdateBasicSetting(UpdateUserModel updateUserModel)
        {
            InfoLog(updateUserModel.Id.ToString(), "UpdateBasicSetting", updateUserModel.UserName);
            if (updateUserModel.Departments == null) updateUserModel.Departments = new List<string>();
            int count = user.Update(updateUserModel.Id, User.Identity.Name, updateUserModel.UserName, updateUserModel.Password, updateUserModel.Mobile, updateUserModel.Email, updateUserModel.CompanyCode, updateUserModel.IdCard, updateUserModel.Sex, updateUserModel.Departments, null);
            if (count == 0) return new ResponseModel<string>(ErrorCode.record_exist, "");
            return new ResponseModel<string>(ErrorCode.success, "");
        }
        public ActionResult UpdatePassword(UpdatePasswordModel updatePasswordModel)
        {
            InfoLog("0", "UpdatePassword");
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
        public ActionResult GetBasic(string filter, int pageIndex = 1, int pageSize = 10, bool delete = false)
        {
            int count = 0;
            var result = user.GetBasic(ref count, filter, delete, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.UserBasic>>(ErrorCode.success, result, count);
        }
        public ActionResult Remove(IEnumerable<string> userIds)
        {
            InfoLog(userIds, "RemoveUser");
            return new ResponseModel<int>(ErrorCode.success, user.RemoveUser(userIds));
        }
        public ActionResult Delete(IEnumerable<string> userIds)
        {
            InfoLog(userIds, "DeleteUser");
            return new ResponseModel<int>(ErrorCode.success, user.DeleteUser(userIds));
        }
        public ActionResult Restore(IEnumerable<string> userIds)
        {
            InfoLog(userIds, "RestoreUser");
            return new ResponseModel<int>(ErrorCode.success, user.RestoreUser(userIds));
        }
        public ActionResult GetUser()
        {
            UserBasicData userBasicData = user.GetUserUpdate(User.Identity.Name);
            if (userBasicData == null)
            {
                var roles = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == ClaimTypes.Role).Select(s => s.Value);
                var userName = ((ClaimsPrincipal)User).Claims.Where(w => w.Type == "StaffName").Select(s => s.Value).FirstOrDefault();
                userBasicData = new UserBasicData()
                {
                    UserId = User.Identity.Name,
                    UserName = userName,
                    Role = roles.ToList()
                };
            }
            return new ResponseModel<UserBasicData>(ErrorCode.success, userBasicData);
        }
        public ActionResult GetByUserId(string userId)
        {
            return new ResponseModel<UserBasicData>(ErrorCode.success, user.GetUserUpdate(userId));
        }

    }
}