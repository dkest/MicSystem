using Mic.Api.Common;
using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Collections.Generic;
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
        /// 添加员工[AUTH]
        /// </summary>
        /// <param name="storeStaffParam">员工信息</param>
        /// <returns></returns>
        [HttpPost, Route("add")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> AddStaff(StoreStaffParam storeStaffParam)
        {
            if (!Util.ValidateMobilePhone(storeStaffParam.Phone))
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
            var result = storeStaffApiRepository.AddStoreStaff(token, storeStaffParam);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item1
            };
        }

        /// <summary>
        /// 更新员工信息[AUTH]
        /// </summary>
        /// <param name="storeStaffParam">员工信息</param>
        /// <returns></returns>
        [HttpPost, Route("update")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateStaff(StoreStaffParam storeStaffParam)
        {
            if (!Util.ValidateMobilePhone(storeStaffParam.Phone))
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "手机号格式不正确",
                    Result = false
                };
            }
            var result = storeStaffApiRepository.UpdateStoreStaff(storeStaffParam);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result.Item1,
                ErrorMessage = result.Item2,
                Result = result.Item1
            };
        }


        /// <summary>
        /// 更新员工启用禁用状态[AUTH]
        /// </summary>
        /// <param name="id">员工Id</param>
        /// <param name="enable">可用状态</param>
        /// <returns></returns>
        [HttpPost, Route("updateStatus")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> UpdateStaffStatus(int id, bool enable)
        {
            var result = storeStaffApiRepository.UpdateStaffStatus(id, enable);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 分页获取员工信息[AUTH]
        /// </summary>
        /// <param name="pageParam">分页参数</param>
        [HttpPost, Route("getList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<PagedResult<StoreStaffParam>> GetStaffList(PageParam pageParam)
        {
            HttpRequest request = HttpContext.Current.Request;
            string token = request.Headers.GetValues("Access-Token").FirstOrDefault();
            var result = storeStaffApiRepository.GetStaffList(token, pageParam);
            return new ResponseResultDto<PagedResult<StoreStaffParam>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new PagedResult<StoreStaffParam>
                {
                    Results = result.Item2,
                    Page = pageParam.PageIndex,
                    PageSize = pageParam.PageSize,
                    Total = result.Item1
                }
            };
        }

    }
}
