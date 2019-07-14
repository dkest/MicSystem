using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class StoreManageController : BaseController
    {
        private StoreTypeRepository storeTypeRepository;
        private StoreRepository storeRepository;
        public StoreManageController()
        {
            storeTypeRepository = ClassInstance<StoreTypeRepository>.Instance;
            storeRepository = ClassInstance<StoreRepository>.Instance;
        }
        public ActionResult StoreManageList()
        {
            return View();
        }

        #region 商家类型
        public ActionResult GetStoreTypeList()
        {
            var result = storeTypeRepository.GetStoreTypeList();
            return Json(new { code = 0, msg = "", count = result.Count, data = result },JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdateStoreType(StoreTypeEntity storeTypeEntity)
        {
            var result = storeTypeRepository.AddOrUpdateStoreType(storeTypeEntity);
            return Json(new { status = result.Item1, data = result.Item2 });
        }
        public ActionResult DeleteStoreType(int id)
        {
            var result = storeTypeRepository.DeleteStoreType(id);
            return Json(new { status = result.Item1, msg = result.Item2, JsonRequestBehavior.AllowGet });

        }
        #endregion

        #region 商家详细信息
        public ActionResult GetStoreList()
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

            var result = storeRepository.GetStoreList(param);
            return Json(new { code = 0, msg = "", count = result.Item1, data = result.Item2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStoreStatus(int id, bool status)
        {
            var result = storeRepository.UpdateStoreStatus(id,status);
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddOrUpdateStoreInfo(StoreDetailInfoEntity entity)
        {
            var result = storeRepository.AddOrUpdateStoreInfo(entity);
            return Json(new { status=result.Item1, data=result.Item2},JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}