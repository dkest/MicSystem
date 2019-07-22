using Mic.Logger;
using qcloudsms_csharp;
using System;
using System.Configuration;

namespace Mic.Api.Common
{
    public class SmsSingleSenderHelper
    {
        public static string SendSms(string phone)
        {
            Random rad = new Random();
            int value = rad.Next(1000, 10000);
            string code = value.ToString();

            int appId = Convert.ToInt32(ConfigurationManager.AppSettings["appId"]);
            string appKey = ConfigurationManager.AppSettings["appKey"].ToString();
            int templateId = Convert.ToInt32(ConfigurationManager.AppSettings["templateId"].ToString());
            string smsTimeOut = ConfigurationManager.AppSettings["smsTimeOut"].ToString();
            SmsSingleSender ssender = new SmsSingleSender(appId, appKey);
            var result = ssender.sendWithParam("86", phone, templateId, new[] { code, smsTimeOut }, "", "", "");
            FileLoggerHelper.WriteInfoLog(DateTime.Now + ": 发送短信验证码-" + result.result + "---" + result.errMsg);
            return code;
        }
    }
}