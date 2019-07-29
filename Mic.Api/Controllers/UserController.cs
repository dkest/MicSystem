using Mic.Api.Common;
using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Net;
using System.Net.Http;
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
        [HttpGet, Route("registerSms/{phone}")]
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
                    var temp = SmsSingleSenderHelper.SendSms(phone);
                    if (temp.Item3 != 0)
                    {
                        return new ResponseResultDto<string>
                        {
                            IsSuccess = false,
                            ErrorMessage = temp.Item2,
                            Result = string.Empty
                        };
                    }
                    smsCode = temp.Item1;
                    //errorMessage = temp.Item2;
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
                ErrorMessage = errorMessage,
                Result = smsCode
            };
        }

        /// <summary>
        /// 忘记密码时，获取手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpGet, Route("forgetSms/{phone}")]
        public ResponseResultDto<string> GetForgetSmsCode(string phone)
        {
            bool isSussess = false;
            string errorMessage = string.Empty;
            string smsCode = string.Empty;
            if (Util.ValidateMobilePhone(phone))//手机号格式是否正确
            {
                if (!userRepository.PhoneIsExist(phone))
                {
                    isSussess = false;
                    errorMessage = "该手机号还没有注册账号";
                }
                else
                {
                    //是正确的手机号格式，获取验证码，同时将手机号等注册信息存储到数据库
                    var temp = SmsSingleSenderHelper.SendSms(phone);
                    smsCode = temp.Item1;
                    errorMessage = temp.Item2;

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
                ErrorMessage = errorMessage,
                Result = smsCode
            };
        }

        ///// <summary>
        /////  验证短信验证码
        ///// </summary>
        ///// <param name="phone">手机号</param>
        ///// <param name="smsCode">短信验证码</param>
        ///// <returns></returns>
        //[HttpGet, Route("validateSms/{phone}/{smsCode}")]
        //public ResponseResultDto<bool> ValidateSmsCode(string phone, string smsCode)
        //{
        //    var res = userRepository.ValidateSmsCode(phone, smsCode);
        //    if (!res.Item1)
        //    {
        //        return new ResponseResultDto<bool>
        //        {
        //            IsSuccess = false,
        //            ErrorMessage = "验证码不正确",
        //            Result = false
        //        };
        //    }
        //    userRepository.DeleteSmsCode(phone, smsCode);
        //    return new ResponseResultDto<bool>
        //    {
        //        IsSuccess = true,
        //        ErrorMessage = string.Empty,
        //        Result = true
        //    };

        //}
        /// <summary>
        /// 用户账号注册
        /// </summary>
        /// <param name="userInfo">用户注册参数</param>
        /// <returns></returns>
        [HttpPost, Route("register")]
        public ResponseResultDto<bool> Register(RegisterParam userInfo)
        {

            var res = userRepository.ValidateSmsCode(userInfo.Phone, userInfo.SmsCode);
            if (!res.Item1)
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "验证码不正确",
                    Result = false
                };
            }
            userRepository.DeleteSmsCode(userInfo.Phone, userInfo.SmsCode);

            bool isSussess = false;
            string errorMessage = string.Empty;
            bool result = false;

            if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.Phone) ||
                string.IsNullOrWhiteSpace(userInfo.Password) || userInfo.UserType < 1)
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = result
                };
            }


            if (userRepository.Register(userInfo))
            {
                isSussess = true;
                result = true;
            }

            return new ResponseResultDto<bool>
            {
                IsSuccess = isSussess,
                ErrorMessage = errorMessage,
                Result = result
            };
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        [HttpPost, Route("updatePassword")]
        public ResponseResultDto<bool> UpdateUserPassword(UserParam userParam)
        {

            var res = userRepository.ValidateSmsCode(userParam.Phone, userParam.SmsCode);
            if (!res.Item1)
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "验证码不正确",
                    Result = false
                };
            }
            userRepository.DeleteSmsCode(userParam.Phone, userParam.SmsCode);

            if (userParam == null || string.IsNullOrWhiteSpace(userParam.Phone) || string.IsNullOrWhiteSpace(userParam.Password))
            {
                return new ResponseResultDto<bool>
                {
                    IsSuccess = false,
                    ErrorMessage = "参数异常",
                    Result = false
                };
            }
            var result = userRepository.UpdateUserPassword(userParam);
            return new ResponseResultDto<bool>
            {
                IsSuccess = result,
                ErrorMessage = string.Empty,
                Result = result
            };
        }

        /// <summary>
        /// 根据登录信息，对用户进行验证，通过则创建并返回一个访问令牌
        /// </summary>
        [HttpPost, Route("login")]
        public ResponseResultDto<UserEntity> CreateAccessToken(UserParam user)
        {
            // 验证账号密码
            bool isSuccess = true;
            string message = string.Empty;
            var resultUser = userRepository.VerifyLogin(user);

            if (resultUser == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "用户名或密码错误"));

            if (resultUser.Item1)
            {
                AccessToken accessToken = new AccessToken
                {
                    TokenId = Guid.NewGuid().ToString(),
                    UserId = resultUser.Item3.Id,
                    CreateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddDays(14)
                };
                tokeRepository.UpdateAccessToken(accessToken);
                resultUser.Item3.AccessToken = accessToken.TokenId;
            }
            else
            {
                isSuccess = false;
                message = resultUser.Item2;
            }
            return new ResponseResultDto<UserEntity>
            {
                IsSuccess = isSuccess,
                ErrorMessage = message,
                Result = resultUser.Item3
            };
        }

        ///// <summary>
        ///// 根据令牌Id，对令牌进行验证，通过则返回完整的访问令牌信息
        ///// </summary>
        //[HttpGet, Route("accesstoken/{tokenId:guid}")]
        //public AccessToken GetAccessToken(Guid tokenId)
        //{
        //    AccessToken accessToken = null;
        //    string key = tokenId.ToString();
        //    object value = HttpRuntime.Cache.Get(key);
        //    if (value != null) accessToken = value as AccessToken;
        //    if (accessToken == null)
        //    {
        //        accessToken = tokeRepository.GetAccessToken(tokenId);
        //        if (accessToken != null)
        //            HttpRuntime.Cache.Add(key, accessToken, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 20, 0), CacheItemPriority.Normal,
        //                (k, v, r) => tokeRepository.RemoveExpired());
        //    }

        //    if (accessToken == null)
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "访问令牌不合法"));

        //    return accessToken;
        //}


    }
}