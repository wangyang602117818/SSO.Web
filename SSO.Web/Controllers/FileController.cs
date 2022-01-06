using SSO.Util.Client;
using SSO.Web.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    [NoneLogRecord]
    public class FileController : BaseController
    {
        public static string fileServiceUrl = AppSettings.GetValue("fileServiceUrl");
        Business.User user = new Business.User();
        public ActionResult Upload(HttpPostedFileBase file)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var result = fileClientService.Upload(file.FileName, file.ContentType, file.InputStream);
            if (result.code != 0) return Json(result, JsonRequestBehavior.AllowGet);
            if (user.UpdateFileId(User.Identity.Name, result.result.FileId, result.result.FileName) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, result.result.FileId);
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult Uploads(UploadFileModel uploadFileModel)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            List<UploadFileItem> files = new List<UploadFileItem>();
            foreach (var item in uploadFileModel.Files)
            {
                files.Add(new UploadFileItem()
                {
                    FileName = item.FileName,
                    FileStream = item.InputStream,
                    ContentType = item.ContentType
                });
            }
            Dictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("roles", uploadFileModel.Roles);
            paras.Add("users", uploadFileModel.Users);
            paras.Add("usersDisplay", uploadFileModel.UsersDisplay);
            var result = fileClientService.Uploads(files, paras);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult DownloadPic(string id, string filename)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var fileItem = fileClientService.DownloadFileIcon(id, filename);
            return File(fileItem.FileStream, fileItem.ContentType);
        }
        public ActionResult FileState(string id)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var fileItem = fileClientService.FileState(id);
            return Json(fileItem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFileList(int pageIndex = 1, int pageSize = 10, string from = "", string filter = "", string fileType = "", DateTime? startTime = null, DateTime? endTime = null, Dictionary<string, string> sorts = null, bool delete = false)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var filelist = fileClientService.GetFileList(pageIndex, pageSize, from, filter, fileType, startTime, endTime, sorts, delete);
            return Content(JsonSerializerHelper.Serialize(filelist));
        }
        public ActionResult GetFileInfos(IEnumerable<string> ids)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var fileinfo = fileClientService.GetFileInfos(ids);
            return Content(JsonSerializerHelper.Serialize(fileinfo));
        }
        [AllowAnonymous]
        public ActionResult DownloadFile(string id, string filename)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var fileItem = fileClientService.DownloadFile(id, filename);
            return File(fileItem.FileStream, fileItem.ContentType);
        }
        public ActionResult GetFromList()
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var froms = fileClientService.GetFromList();
            return Content(JsonSerializerHelper.Serialize(froms));
        }
        public ActionResult GetFileTypeMapping()
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var mappings = fileClientService.GetExtensionMap();
            return Content(JsonSerializerHelper.Serialize(mappings));
        }
        public ActionResult Remove(string id)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization());
            var result = fileClientService.RemoveFile(id);
            return Content(JsonSerializerHelper.Serialize(result));
        }
    }
}