using SSO.Util.Client;
using SSO.Web.Filters;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var controllers = assembly.GetTypes().Where(w => w.FullName.Contains("SSO.Web.Controllers"));
            var actions = JwtAuthorizeAttribute.GetPermissionDescription(controllers);
            SSOClientService sSOClientService = new SSOClientService("http://www.ssoapi.com:8030/", JwtManager.GetAuthorization(Request));
            sSOClientService.ReplacePermissions(AppSettings.GetApplicationUrlTrimHttpPrefix(Request), actions);
            return Json(actions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ticket()
        {
            return Content(jwtManager.GenerateTicket("CN445379"));
        }
        public ActionResult Decode(string ticket)
        {
            return Content(jwtManager.DecodeTicket(ticket));
        }
        public ActionResult DecodeToken(string token)
        {
            var result = JwtManager.ParseAuthorization(token, ssoSecretKey);
            return Content(result.Identity.Name);
        }
        [AllowAnonymous]
        public string SecretKey()
        {
            return SymmetricEncryptHelper.GenerateAESKey;
        }
    }
}