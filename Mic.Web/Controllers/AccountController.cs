using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using Mic.Web.Common;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class AccountController : BaseController
    {
        private AdminRepository adminRepository;
        public AccountController()
        {
            adminRepository = ClassInstance<AdminRepository>.Instance;
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

            var result = adminRepository.VerifyLogin(admin.UserName, admin.Password);
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

        public ActionResult Logout()
        {
            Session.Remove(CommonConst.UserSession);
            return Json(new { status = true });
        }
        public ActionResult GetAdminList()
        {
            var page = GetIntValFromReq("page");
            var limit = GetIntValFromReq("limit");
            var param = new PageParam
            {
                PageIndex = page,
                PageSize = limit
            };

            var result = adminRepository.GetAdminList(param);
            return Json(new { code = 0, msg = string.Empty, count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAdminStatus(int id, bool status)
        {
            var result = adminRepository.UpdateAdminStatus(id, status);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrUpdateAdmin(Admin admin)
        {
            var result = adminRepository.AddOrUpdateAdmin(admin);
            return Json(new { status = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

    }
}