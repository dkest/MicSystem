using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class SongManageController : Controller
    {
        public ActionResult SongBookList()
        {
            return View();
        }
    }
}