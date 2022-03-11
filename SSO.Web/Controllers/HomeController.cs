using SSO.Business;
using SSO.Util.Client;
using SSO.Web.Filters;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class HomeController : BaseController
    {
        Permission permission = new Permission();
        public ActionResult InitPermission()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(w => w.FullName.Contains("SSO.Web.Controllers"));
            var actions = JwtAuthorizeAttribute.GetPermissionDescription(controllers);
            //SSOClientService sSOClientService = new SSOClientService("http://www.ssoapi.com", JwtManager.GetAuthorization(Request));
            //sSOClientService.ReplacePermissions("ssoapi", actions);
            permission.DeleteAndInsertMany("ssoapi", actions);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ticket()
        {
            return Content(jwtManager.GenerateTicket("CN445379"));
        }
        [AllowAnonymous]
        public ActionResult Decode(string ticket)
        {
            return Content(jwtManager.DecodeTicket(ticket));
        }
        [AllowAnonymous]
        public ActionResult DecodeToken(string token)
        {
            var result = JwtManager.ParseAuthorization(token, ssoSecretKey);
            return Content(result.Identity.Name);
        }
        [AllowAnonymous]
        public string SecretKey()
        {
            return AesEncryptHelper.GenerateAESKey();
        }
    }

}