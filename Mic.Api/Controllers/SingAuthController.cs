using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
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
    public class SingAuthController : ApiController
    {
        public SingAuthController()
        {

        }


        /// <summary>
        /// 获取认证条款[AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getAttestationClause")]
        [AccessTokenAuthorize]
        public ResponseResultDto<string> GetPublishSongList()
        {
          

            return new ResponseResultDto<string>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = ""//认证条款内容
            };
        }

        /// <summary>
        /// 提交音乐人认证资料[AUTH]
        /// </summary>
        /// <param name="singer"></param>
        /// <returns></returns>
        [HttpPost, Route("addAuthinfo")]
        [AccessTokenAuthorize]
        public ResponseResultDto<string> AddAuthinfo(SingerDetailInfoEntity singer)
        {
            //参数校验


            return new ResponseResultDto<string>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = ""//认证条款内容
            };
        }

        /// <summary>
        /// 获取音乐人认证信息[AUTH]
        /// </summary>
        /// <param name="singerId"></param>
        /// <returns></returns>
        [HttpPost, Route("getAuthInfo")]
        [AccessTokenAuthorize]
        public ResponseResultDto<SingerDetailInfoEntity> AddAuthinfo(int singerId)
        {
            //参数校验


            return new ResponseResultDto<SingerDetailInfoEntity>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = new SingerDetailInfoEntity()//音乐人详细信息
            };
        }
    }
}
