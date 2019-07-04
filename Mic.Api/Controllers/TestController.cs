using Mic.Api.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("test")]
    [AccessTokenAuthorize(Roles = "Admin")]
    public class TestController : ApiController
    {

        /// <summary>
        /// 测试权限[AUTH]
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost, Route("login")]
        public string Test(int login)
        {
            return "hello" + login;
        }
    }
}
