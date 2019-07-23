using Mic.Api.Filter;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 员工管理
    /// </summary>
    [RoutePrefix("staff")]
    public class StoreSatffController : ApiController
    {
        private StoreStaffApiRepository storeStaffApiRepository;

        public StoreSatffController()
        {
            storeStaffApiRepository = ClassInstance<StoreStaffApiRepository>.Instance;
        }

        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="storeStaffParam"></param>
        [HttpGet, Route("add")]
        [AccessTokenAuthorize]
        public void AddStaff(StoreStaffParam storeStaffParam)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();

            storeStaffApiRepository.AddStoreStaff(token, storeStaffParam);
        }
        
    }
}
