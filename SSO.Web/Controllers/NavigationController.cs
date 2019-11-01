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
            if (navigation.Insert(navigationModel.Title, navigationModel.BaseUrl, navigationModel.IconUrl) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.server_exception, "");
            }
        }
        public ActionResult Update(UpdateNavigationModel updateNavigationModel)
        {
            if (navigation.Update(updateNavigationModel.Id, updateNavigationModel.Title, updateNavigationModel.BaseUrl,updateNavigationModel.IconUrl) > 0)
            {
                return new ResponseModel<string>(ErrorCode.success, "");
            }
            else
            {
                return new ResponseModel<string>(ErrorCode.record_exist, "");
            }

        }
        public ActionResult GetById(int id)
        {
            return new ResponseModel<Data.Models.Navigation>(ErrorCode.success, navigation.GetById(id));
        }
        public ActionResult GetList(string filter = "", int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            var result = navigation.GetList(ref count, filter, pageIndex, pageSize);
            return new ResponseModel<IEnumerable<Data.Models.Navigation>>(ErrorCode.success, result, count);
        }
        public ActionResult Delete(IEnumerable<int> ids)
        {
            return new ResponseModel<int>(ErrorCode.success, navigation.Delete(ids));
        }

    }
}