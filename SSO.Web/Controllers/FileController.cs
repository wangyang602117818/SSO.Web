using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class FileController : Controller
    {
        public static string fileServiceUrl = AppSettings.GetValue("fileServiceUrl");
        public ActionResult Upload(HttpPostedFileBase file)
        {
            FileClientService fileClientService = new FileClientService(fileServiceUrl, JwtManager.GetAuthorization(Request));
            var result = fileClientService.Upload(file.FileName, file.ContentType, file.InputStream);

            return View();
        }
        public ActionResult DownloadFile(string id, string filename)
        {

            return null;
        }
        public ActionResult DownloadFileIcon(string id, string filename)
        {
            return null;
        }
    }
}