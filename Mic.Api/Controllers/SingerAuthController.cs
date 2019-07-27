using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mic.Api.Controllers
{

    /// <summary>
    /// 音乐人认证
    /// </summary>
    [RoutePrefix("singerAuth")]
    public class SingerAuthController : ApiController
    {
        private SingerAuthRepository singerAuthRepository;
        public SingerAuthController()
        {
            singerAuthRepository = ClassInstance<SingerAuthRepository>.Instance;
        }


        /// <summary>
        /// 获取认证条款[AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getAttestationClause")]
        [AccessTokenAuthorize]
        public ResponseResultDto<string> GetAuthNote()
        {
            var result = singerAuthRepository.GetAuthNote();
            return new ResponseResultDto<string>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result//认证条款内容
            };
        }

        /// <summary>
        /// 提交音乐人认证资料[AUTH]
        /// </summary>
        /// <param name="singer"></param>
        /// <returns></returns>
        [HttpPost, Route("addAuthinfo")]
        [AccessTokenAuthorize]
        public ResponseResultDto<bool> AddAuthInfo(SingerAuthParam singer)
        {
            var result = singerAuthRepository.AddAuthInfo(singer);

            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 获取音乐人认证信息[AUTH]
        /// </summary>
        /// <param name="singerId"></param>
        /// <returns></returns>
        [HttpPost, Route("getAuthInfo")]
        [AccessTokenAuthorize]
        public ResponseResultDto<SingerAuthParam> GetSingerAuthInfo(int singerId)
        {
            var result = singerAuthRepository.GetSingerAuthInfo(singerId);

            return new ResponseResultDto<SingerAuthParam>
            {
                IsSuccess = result != null ? true : false,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 获取音乐人流派列表[AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getSingerTypeList")]
        [AccessTokenAuthorize]
        public ResponseResultDto<List<SingerTypeEntity>> GetAllSingerTypeList()
        {
            return new ResponseResultDto<List<SingerTypeEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = singerAuthRepository.GetAllSingerTypeList()
            };
        }
    }
}
