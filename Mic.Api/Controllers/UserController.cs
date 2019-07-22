using Mic.Api.Common;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Logger;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
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
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        private TokenRepository tokeRepository;
        private UserRepository userRepository;
        public UserController()
        {
            tokeRepository = ClassInstance<TokenRepository>.Instance;
            userRepository = ClassInstance<UserRepository>.Instance;
        }

        /// <summary>
        /// 注册账号时，获取手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpGet, Route("registerSms")]
        public ResponseResultDto<string> GetRegisterSmsCode(string phone)
        {
            bool isSussess = false;
            string errorMessage = string.Empty;
            string smsCode = string.Empty;
            if (Util.ValidateMobilePhone(phone))//手机号格式是否正确
            {
                if (userRepository.PhoneIsExist(phone))
                {
                    isSussess = false;
                    errorMessage = "手机号已经存在，无法注册";
                }
                else
                {
                    //是正确的手机号格式，获取验证码，同时将手机号等注册信息存储到数据库
                    smsCode = "2123";
                    isSussess = true;
                    SmsRecord sms = new SmsRecord
                    {
                        UserId = -1,
                        Phone = phone,
                        Code = smsCode,
                    };
                    userRepository.SaveSmsCode(sms);
                }

            }
            else
            {
                errorMessage = "手机号格式不正确";
            }
            return new ResponseResultDto<string>
            {
                IsSuccess = isSussess,
                Result = smsCode,
                ErrorMessage = errorMessage
            };
        }
        /// <summary>
        /// 注册校验 验证码（只需要传入手机号和验证码）
        /// </summary>
        /// <param name="registerParam"></param>
        /// <returns></returns>
        [HttpPost, Route("validateSms")]
        public ResponseResultDto<bool> ValidateSmsCode([FromBody]RegisterParam registerParam)
        {
            bool isSussess = true;
            string errorMessage = string.Empty;
            bool result = false;

            var res = userRepository.ValidateSmsCode(registerParam);
            if (!res.Item1)
            {
                isSussess = false;
                errorMessage = "验证码不正确";
            }
            return new ResponseResultDto<bool>
            {
                IsSuccess = isSussess,
                Result = result,
                ErrorMessage = errorMessage
            };

        }
        /// <summary>
        /// 账号注册
        /// </summary>
        /// <param name="registerParam">注册参数</param>
        /// <returns></returns>
        [HttpPost, Route("register")]
        public ResponseResultDto<bool> Register([FromBody]RegisterParam registerParam)
        {
            bool isSussess = false;
            string errorMessage = string.Empty;
            bool result = false;

            if (userRepository.Register(registerParam))
            {
                isSussess = true;
                result = true;
            }
            
            return new ResponseResultDto<bool>
            {
                IsSuccess = isSussess,
                Result = result,
                ErrorMessage = errorMessage
            };
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