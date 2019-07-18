using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using Mic.Web.Common;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class AccountController : BaseController
    {
        private AdminRepository adminrRepository;
        public AccountController()
        {
            adminrRepository = ClassInstance<AdminRepository>.Instance;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult AccountManageView()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyLogin(Admin admin)
        {
            if (string.IsNullOrWhiteSpace(admin.UserName))
            {
                return Json(new { status = false, msg = "用户名不能为空" });
            }
            if (string.IsNullOrWhiteSpace(admin.Password))
            {
                return Json(new { status = false, msg = "密码不能为空" });
            }

            var result = adminrRepository.VerifyLogin(admin.UserName, admin.Password);
            if (result.Item1)
            {

                //写入Session
                SessionHelper.SetUser(Session, result.Item3);
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = result.Item1, msg = result.Item2 });
            }

        }
    }
}