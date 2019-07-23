using Mic.Api.Common;
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

        #region 分店管理
        /// <summary>
        /// 添加分店[AUTH]
        /// </summary>
        /// <param name="sonStore">分店信息</param>
        /// <returns></returns>
        [HttpPost, Route("addSonStore")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> AddSonStore(SonStoreInfoParam sonStore)
        {
            if (!Util.ValidateMobilePhone(sonStore.Phone))
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "手机号格式不正确",
                    Result = false
                };
            }
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeApiRepository.AddSonStore(token, sonStore);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item1
            };
        }

        /// <summary>
        /// 更新分店信息[AUTH]
        /// </summary>
        /// <param name="sonStore">分店信息</param>
        /// <returns></returns>
        [HttpPost, Route("updateSonStore")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateSonStore(SonStoreInfoParam sonStore)
        {
            if (!Util.ValidateMobilePhone(sonStore.Phone))
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "手机号格式不正确",
                    Result = false
                };
            }
            var result = storeApiRepository.UpdateSonStore(sonStore);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item1
            };
        }


        /// <summary>
        /// 更新分店启用禁用状态[AUTH]
        /// </summary>
        /// <param name="id">分店Id</param>
        /// <param name="enable">可用状态</param>
        /// <returns></returns>
        [HttpPost, Route("updateSonStoreStatus")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateSonStoreStatus(int id, bool enable)
        {
            var result = storeApiRepository.UpdateSonStoreStatus(id, enable);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 分页获取分店列表信息[AUTH]
        /// </summary>
        /// <param name="pageParam">分页参数</param>
        [HttpPost, Route("getSonStoreList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<SonStoreInfoParam>> GetStaffList(PageParam pageParam)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeApiRepository.GetSonStoreList(token, pageParam);
            return new ResponseResultDto<PagedResult<SonStoreInfoParam>>
            {
                IsSuccess = true,
                ErrorMessage = result.Item2,
                Result = new PagedResult<SonStoreInfoParam>
                {
                    Results = result.Item4,
                    Page = pageParam.PageIndex,
                    PageSize = pageParam.PageSize,
                    Total = result.Item3
                }
            };
        }
        /// <summary>
        /// 获取商家分店总数和一起用分店数量[AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getSonStoreStatistics")]
        [AccessTokenAuthorize]
        public ResponseResultDto<StoreStatistic> GetStoreStatistics()
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeApiRepository.GetSonStoreStatistic(token);
            return new ResponseResultDto<StoreStatistic>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = new StoreStatistic()
                {
                    MaxCount = result.Item3,
                    ValidCount = result.Item4
                }
            };
        }
        #endregion

    }
}
