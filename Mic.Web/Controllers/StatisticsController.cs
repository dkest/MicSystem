using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        private StoreStatisticsRepository storeStatisticsRepository;
        public StatisticsController()
        {
            storeStatisticsRepository = ClassInstance<StoreStatisticsRepository>.Instance;
        }
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
            ViewBag.name = GetStrValFromReq("storeName");
            return View();
        }

        public ActionResult GetStoreStatistics()
        {
            var result = storeStatisticsRepository.GetStoreStatistics();
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStoreStatisticsList()
        {
            DateTime begin = GetDateTimeValFromReq("beginDate").Value;
            DateTime end = GetDateTimeValFromReq("endDate").Value.AddDays(1).AddSeconds(-1);
            string field = GetStrValFromReq("field");
            StorePlaySongPageParam param = new StorePlaySongPageParam
            {
                OrderField = field,
                BeginDate = begin,
                EndDate = end
            };
            var result = storeStatisticsRepository.GetStorePlaySongInfo(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStoreStaticInfo(int storeId, DateTime beginDate, DateTime endDate)
        {
            var result = storeStatisticsRepository.GetStoreStatisticsInfo(storeId, beginDate, endDate);
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStorePlaySongList()
        {
            DateTime begin = GetDateTimeValFromReq("beginDate").Value;
            DateTime end = GetDateTimeValFromReq("endDate").Value.AddDays(1).AddSeconds(-1);
            string field = GetStrValFromReq("field");
            StorePlaySongPageParam param = new StorePlaySongPageParam
            {
                OrderField = field,
                BeginDate = begin,
                EndDate = end
            };
            var result = storeStatisticsRepository.GetStorePlaySongList(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}