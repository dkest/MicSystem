using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class SingerManageController : BaseController
    {
        private ParamRepository paramRepository;
        private SingerRepository singerRepository;
        private SingerTypeRepository singerTypeRepository;
        public SingerManageController()
        {
            paramRepository = ClassInstance<ParamRepository>.Instance;
            singerRepository = ClassInstance<SingerRepository>.Instance;
            singerTypeRepository = ClassInstance<SingerTypeRepository>.Instance;
        }

        public ActionResult SingerList()
        {
            return View();
        }
        
        /// <summary>
        /// 更新音乐人入驻条款
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult UpdateParam(ParamEntity param)
        {
            var result = paramRepository.UpdateParam(param);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取音乐人入驻条款
        /// </summary>
        /// <returns></returns>
        public ActionResult GetParamByType()
        {
            var result = paramRepository.GetParamByTypeId(1);
            return Json(new { status = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        #region 音乐人流派
        public ActionResult GetSingerTypeList()
        {
            var result = singerTypeRepository.GetSingerTypeList();
            return Json(new { code = 0, msg = "", count = result.Count, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateSingerType(SingerTypeEntity singerTypeEntity)
        {
            var result = singerTypeRepository.AddOrUpdateSingerType(singerTypeEntity);
            return Json(new { status = result.Item1, data = result.Item2 });
        }
        public ActionResult DeleteSingerType(int id)
        {
            var result = singerTypeRepository.DeleteSingerType(id);
            return Json(new { status = result.Item1, msg = result.Item2, JsonRequestBehavior.AllowGet });

        }
        #endregion

        /// <summary>
        /// 分页获取音乐人列表
        /// </summary>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public ActionResult GetSingerList(PageParam pageParam)
        {
            var result = singerRepository.GetSingerList(pageParam);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }
    }
}