using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {

        /// <summary>
        /// 检查该用户时候答过题
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, Route("{phone}")]
        public bool ExistPhone(string phone)
        {
            return false;
        }
        /// <summary>
        /// 保存答题结果
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost, Route("saveresult")]
        public bool SaveResult()
        {
            return true;
        }

        /// <summary>
        /// adadas
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, Route("ranking")]
        public int GetRanking(string phone)
        {
            return 3;
        }




    }
}