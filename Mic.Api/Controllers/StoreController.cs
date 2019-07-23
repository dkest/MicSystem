using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 商家接口
    /// </summary>
    [RoutePrefix("store")]
    public class StoreController : ApiController
    {
        private StoreTypeRepository storeTypeRepository;
        private StoreApiRepository storeApiRepository;
        public StoreController()
        {
            storeTypeRepository = ClassInstance<StoreTypeRepository>.Instance;
            storeApiRepository = ClassInstance<StoreApiRepository>.Instance;
        }

        /// <summary>
        /// 获取商家类型信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getStoreTypeList")]
        public ResponseResultDto<List<StoreTypeEntity>> GetStoreTypeList()
        {
            var result = storeTypeRepository.GetStoreTypeList();
            if (result == null || result.Count <= 0)
            {
                return new ResponseResultDto<List<StoreTypeEntity>>
                {
                    IsSuccess = false,
                    ErrorMessage = "当前系统没有商家类型信息",
                    Result = result
                };
            }
            return new ResponseResultDto<List<StoreTypeEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 根据商家编码，获取商家企业资料[AUTH]
        /// </summary>
        /// <param name="storeCode">商家编码</param>
        /// <returns></returns>
        [HttpGet, Route("getStoreInfo/{storeCode:guid}")]
        [AccessTokenAuthorize]
        public ResponseResultDto<StoreInfoParam> GetStoreDetailByStoreCode(string storeCode)
        {
            var result = storeApiRepository.GetStoreDetailByStoreCode(storeCode);
            return new ResponseResultDto<StoreInfoParam>
            {
                IsSuccess = result.Item1,
                ErrorMessage = string.Empty,
                Result = result.Item2
            };
        }
        /// <summary>
        /// 更新商家信息[AUTH]
        /// </summary>
        /// <param name="storeInfo">商家信息</param>
        /// <returns></returns>
        [HttpPost, Route("updateStoreInfo")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateStoreInfo(StoreInfoParam storeInfo)
        {
            if (storeInfo == null || storeInfo.Id < 0)
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = false
                };
            }
            var result = storeApiRepository.UpdateStoreInfo(storeInfo);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = string.Empty,
                Result = result.Item1
            };
        }

    }
}
