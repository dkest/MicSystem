using Mic.Repository;
using Mic.Repository.Repositories;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class SongManageController : Controller
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

        public ActionResult GetAllSongBookList()
        {
            var result = songBookRepository.GetAll();
            return Json(new { code=0,msg="", count=result.Count,data= result }, JsonRequestBehavior.AllowGet);
        }
    }
}