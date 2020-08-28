using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class FileController : BaseController
    {
        public static string fileServiceUrl = AppSettings.GetValue("fileServiceUrl");
        Business.UserBasic user = new Business.UserBasic();
        public ActionResult Upload(HttpPostedFileBase file)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization(Request));
            var result = fileClientService.Upload(file.FileName, file.ContentType, file.InputStream);
            if (result.code != 0) return new ResponseModel<string>(ErrorCode.server_exception, result.message);
            if (user.UpdateFileId(User.Identity.Name, result.result.FileId, result.result.FileName) > 0)

            {
                return new ResponseModel<string>(ErrorCode.success, result.result.FileId);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult DownloadPic(string id, string filename)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization(Request));
            var fileItem = fileClientService.DownloadFileIcon(id, filename);
            return File(fileItem.FileStream, fileItem.ContentType);
        }
        public ActionResult FileState(string id)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization(Request));
            var fileItem = fileClientService.FileState(id);
            return Json(fileItem, JsonRequestBehavior.AllowGet);
        }
    }
}