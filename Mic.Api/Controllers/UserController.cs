using Mic.Entity;
using Mic.Logger;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [RoutePrefix("tokens")]
    public class UserController : ApiController
    {
        private TokenRepository tokeRepository;
        public UserController()
        {
            tokeRepository = ClassInstance<TokenRepository>.Instance;
        }
        /// <summary>
        /// 根据登录信息，对用户进行验证，通过则创建并返回一个访问令牌
        /// </summary>
        [HttpPost, Route("login")]
        public AccessToken CreateAccessToken(User login)
        {
            // 验证账号密码
            //object user = null;
            //if (login.UserType == UserType.Admin)
            //    user = Admin.GetAdmin(login.UserId, login.Password);
            //else if (login.UserType == UserType.Customer)
            //    user = Customer.GetCustomer(login.UserId, login.Password);

            //if (user == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "用户名或密码错误"));
            LoggerProvider.Logger.Error("ceshi 222");
            AccessToken accessToken = new AccessToken
            {
                TokenId = Guid.NewGuid(),
                RoleId = login.RoleId,
                UserId = login.Id,
                CreateTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddDays(14)
            };
            tokeRepository.AddAccessToken(accessToken);
            return accessToken;
        }

        /// <summary>
        /// 根据令牌Id，对令牌进行验证，通过则返回完整的访问令牌信息
        /// </summary>
        [HttpGet, Route("{tokenId:guid}")]
        public AccessToken GetAccessToken(Guid tokenId)
        {
            AccessToken accessToken = null;
            string key = tokenId.ToString();
            object value = HttpRuntime.Cache.Get(key);
            if (value != null) accessToken = value as AccessToken;
            if (accessToken == null)
            {
                accessToken = tokeRepository.GetAccessToken(tokenId);
                if (accessToken != null)
                    HttpRuntime.Cache.Add(key, accessToken, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0), CacheItemPriority.Normal,
                        (k, v, r) => tokeRepository.RemoveExpired());
            }

            if (accessToken == null)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "访问令牌不合法"));

            return accessToken;
        }


    }
}