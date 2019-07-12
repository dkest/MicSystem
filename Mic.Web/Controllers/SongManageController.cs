using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class SongManageController : BaseController
    {
        private SongBookRepository songBookRepository;
        private SongMarkRepository songMarkRepository;
        private SongAuditRepository songAuditRepository;
        public SongManageController()
        {
            songBookRepository = ClassInstance<SongBookRepository>.Instance;
            songMarkRepository = ClassInstance<SongMarkRepository>.Instance;
            songAuditRepository = ClassInstance<SongAuditRepository>.Instance;
        }
        public ActionResult SongBookList()
        {
            return View();
        }

        #region 曲库管理
        /// <summary>
        /// 获取已审核的歌曲列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetApprovedSongList()
        {
            var page = GetIntValFromReq("page");//页码
            var limit = GetIntValFromReq("limit");//每页数据
            var keyword = GetStrValFromReq("keyword");
            var field = GetStrValFromReq("field");
            var order = GetStrValFromReq("order");

            var param = new PageParam
            {
                PageIndex = page,
                PageSize = limit,
                Keyword = keyword,
                OrderField = field,
                OrderType = order
            };

            var result = songBookRepository.GetApprovedSongList(param);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAuditSongList()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            var keyword = GetStrValFromReq("keyword");
            var auditStatus = GetIntValFromReq("auditStatus");
            var param = new AuditSongPageParam
            {
                PageIndex = page,
                PageSize = limit,
                Keyword = keyword,
                AuditStatus = auditStatus
            };
            var result = songBookRepository.GetAuditSongList(param);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSongById(int id)
        {
            var result = songBookRepository.DeleteSongById(id);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateSong(SongBookEntity songBookEntity)
        {
            var result = songBookRepository.AddOrUpdateSong(songBookEntity);
            return Json(new { status = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuditSong(SongBookEntity song)
        {
            var result = songAuditRepository.AuditSong(song);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SongMark 歌曲标签
        public ActionResult AddOrUpdateSongMark(SongMarkEntity songMarkEntity)
        {
            var result = songMarkRepository.AddOrUpdateSongMark(songMarkEntity);
            return Json(new { status = result.Item1, data = result.Item2 });
        }

        public ActionResult DeleteSongMark(int id)
        {
            var result = songMarkRepository.DeleteSongMark(id);
            return Json(new { status = result.Item1, msg=result.Item2 });
        }

        public ActionResult GetSongMakList()
        {
            var result = songMarkRepository.GetSongMakList();
            return Json(new { code = 0, msg = "", count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 歌曲审核
        /// <summary>
        /// 获取歌曲审核明细
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        public ActionResult GetAuditDetail(int songId)
        {
            var result = songAuditRepository.GetAuditDetail(songId);
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}