using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System.Collections.Generic;
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
        private StoreRepository storeRepository;
        public StoreController()
        {
            storeTypeRepository = ClassInstance<StoreTypeRepository>.Instance;
            storeRepository = ClassInstance<StoreRepository>.Instance;
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
        /// 根据商家Id，获取商家企业资料[AUTH]
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        [HttpGet, Route("getStoreInfo/{storeId:int}")]
        public ResponseResultDto<StoreDetailInfoEntity> GetStoreInfoById(int storeId)
        {
            var result = storeRepository.GetStoreDetailById(storeId);
            return new ResponseResultDto<StoreDetailInfoEntity>
            {
                IsSuccess = result.Item1,
                ErrorMessage = string.Empty,
                Result = result.Item2
            };
        }

    }
}
