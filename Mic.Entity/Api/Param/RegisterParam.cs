using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mic.Entity
{

    /// <summary>
    /// 注册的参数
    /// </summary>
    public class RegisterParam
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string SmsCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 用户类型 1-音乐人；2-商家
        /// </summary>
        public int UserType { get; set; }
    }
}