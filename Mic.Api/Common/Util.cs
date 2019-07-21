using System.Text.RegularExpressions;

namespace Mic.Api.Common
{
    public static class Util
    {
        /// <summary>
        /// 校验手机号码是否符合标准。
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool ValidateMobilePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return false;
            return Regex.IsMatch(phone, @"^(13|14|15|16|18|19|17)\d{9}$");
        }
    }
}