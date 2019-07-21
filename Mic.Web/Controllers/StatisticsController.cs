using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class StatisticsController : BaseController
    {
        private StoreStatisticsRepository storeStatisticsRepository;
        private SingerStatisticsRepository singerStatisticsRepository;
        public StatisticsController()
        {
            storeStatisticsRepository = ClassInstance<StoreStatisticsRepository>.Instance;
            singerStatisticsRepository = ClassInstance<SingerStatisticsRepository>.Instance;
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
            ViewBag.singerId = GetStrValFromReq("singerId");
            ViewBag.singerName = GetStrValFromReq("singerName");
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
        #region 商家统计
        public ActionResult GetStoreStatistics()
        {
            var result = storeStatisticsRepository.GetStoreStatistics();
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStoreStatisticsList()
        {
            var page = GetIntValFromReq("page");//页码
            var limit = GetIntValFromReq("limit");//每页数据
            DateTime begin = GetDateTimeValFromReq("beginDate").Value;
            DateTime end = GetDateTimeValFromReq("endDate").Value.AddDays(1).AddSeconds(-1);
            string field = GetStrValFromReq("field");
            StorePlaySongPageParam param = new StorePlaySongPageParam
            {
                PageIndex = page,
                PageSize = limit,
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
            var page = GetIntValFromReq("page");//页码
            var limit = GetIntValFromReq("limit");//每页数据

            DateTime begin = GetDateTimeValFromReq("beginDate").Value;
            DateTime end = GetDateTimeValFromReq("endDate").Value.AddDays(1).AddSeconds(-1);
            string field = GetStrValFromReq("field");
            int playUserId = GetIntValFromReq("playUserId");
            StorePlaySongPageParam param = new StorePlaySongPageParam
            {
                PageIndex = page,
                PageSize = limit,
                OrderField = field,
                BeginDate = begin,
                EndDate = end,
                PlayUserId = playUserId
            };
            var result = storeStatisticsRepository.GetStorePlaySongList(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 音乐人统计
        public ActionResult GetSingerStatistics()
        {
            var result = singerStatisticsRepository.GetSingerStatistics();
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSingerStatisticsList()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            DateTime begin = GetDateTimeValFromReq("beginDate").Value;
            DateTime end = GetDateTimeValFromReq("endDate").Value.AddDays(1).AddSeconds(-1);
            string field = GetStrValFromReq("field");

            StorePlaySongPageParam param = new StorePlaySongPageParam
            {
                PageIndex = page,
                PageSize = limit,
                OrderField = field,
                BeginDate = begin,
                EndDate = end
            };
            var result = singerStatisticsRepository.GetSingerStatisticsList(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUploadSongListBySingerId()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            int singerId = GetIntValFromReq("singerId");
            SingerSongPageParam param = new SingerSongPageParam
            {
                PageIndex = page,
                PageSize = limit,
                SingerId = singerId
            };
            var result = singerStatisticsRepository.GetUploadSongListBySingerId(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPublishSongListBySingerId()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            int singerId = GetIntValFromReq("singerId");
            var order = GetStrValFromReq("order");
            string field = GetStrValFromReq("field");
            SingerSongPageParam param = new SingerSongPageParam
            {
                PageIndex = page,
                PageSize = limit,
                SingerId = singerId,
                OrderType = order,
                OrderField = field
            };
            var result = singerStatisticsRepository.GetPublishSongListBySingerId(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSongRecordBySingerId()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            int singerId = GetIntValFromReq("singerId");
            SingerSongPageParam param = new SingerSongPageParam
            {
                PageIndex = page,
                PageSize = limit,
                SingerId = singerId,
            };
            var result = singerStatisticsRepository.GetSongRecordBySingerId(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);

        }
        #endregion

    }
}