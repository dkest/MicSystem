using Mic.Repository;
using Mic.Repository.Repositories;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class CityController : Controller
    {
        private CityRepository cityRepository;
        public CityController()
        {
            cityRepository = ClassInstance<CityRepository>.Instance;
        }
        public ActionResult GetProvinceList()
        {
            var result = cityRepository.GetProvinceList();
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCityList(int provinceId)
        {
            var result = cityRepository.GetCityList(provinceId);
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountyList(int cityId)
        {
            var result = cityRepository.GetCountyList(cityId);
            return Json(new { status = true, data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDeatilCity(int province, int city, int county)
        {
            var result = cityRepository.GetDeatilCity(province, city, county);
            return Json(new { status = true, province = result.Item1,city=result.Item2,county=result.Item3 }, JsonRequestBehavior.AllowGet);
        }
    }
}