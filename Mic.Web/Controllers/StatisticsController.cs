using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        public ActionResult StatisticsView()
        {
            return View();
        }

        public ActionResult SingerStatistics()
        {
            return View();
        }
        public ActionResult SingerStatisticsDetail()
        {
            return View();
        }

        public ActionResult StoreStatistics()
        {
            return View();
        }
        public ActionResult StoreStatisticsDetail()
        {
            ViewBag.id = GetStrValFromReq("id");
            return View();
        }
    }
}