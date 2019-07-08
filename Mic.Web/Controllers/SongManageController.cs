using Mic.Repository;
using Mic.Repository.Repositories;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class SongManageController : BaseController
    {
        private SongBookRepository songBookRepository;
        public SongManageController()
        {
            songBookRepository = ClassInstance<SongBookRepository>.Instance;
        }
        public ActionResult SongBookList()
        {
            return View();
        }

        /// <summary>
        /// 获取已审核的歌曲列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetApprovedSongList()
        {
            var page = GetIntValFromReq("page");//页码
            var limit = GetIntValFromReq("limit");//每页数据
            var keyword = GetStrValFromReq("keyword");
            var param = new PageParam
            {
                PageIndex = page,
                PageSize = limit,
                Keyword = keyword
            };
            var result = songBookRepository.GetApprovedSongList(param);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAuditSongList()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            var keyword = GetStrValFromReq("keyword");
            var param = new PageParam
            {
                PageIndex = page,
                PageSize = limit,
                Keyword = keyword
            };
            var result = songBookRepository.GetAuditSongList(param);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

    }
}