using SSO.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SSO.Web.Controllers
{
    public class NavigationController : Controller
    {
        Business.Navigation navigation = new Business.Navigation();
        public ActionResult Add(NavigationModel navigationModel)
        {
            if (navigation.Insert(navigationModel.Title, navigationModel.Url, navigationModel.IconUrl) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var result = navigation.GetList(ref count, filter, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.Navigation>>(ErrorCode.success, result, count);
        }
    }
}