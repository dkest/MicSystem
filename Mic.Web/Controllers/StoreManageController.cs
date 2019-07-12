using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class StoreManageController : BaseController
    {
        public ActionResult StoreManageList()
        {
            return View();
        }

        public ActionResult GetStoreTypeList()
        {
            return Json(new { },JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateStoreType()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteStoreType()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
        

    }
}