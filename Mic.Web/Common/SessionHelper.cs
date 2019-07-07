using Mic.Entity;
using Mic.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mic.Web.Common
{
    public class SessionHelper
    {
        public static Admin GetUser(HttpSessionStateBase session)
        {
            try
            {
                object user = session[CommonConst.UserSession];
                if (null != user)
                {
                    return (user as Admin);
                }
            }
            catch (Exception ex)
            {
                FileLoggerHelper.WriteErrorLog(string.Format("SessionHelper::GetUser出现异常:{0}", ex.Message));
            }
            return null;
        }

        /// <summary>
        /// 设置User的session
        /// </summary>
        public static Tuple<bool, string> SetUser(HttpSessionStateBase session, Admin admin)
        {
            bool isSuccess = true;
            string retMsg = string.Empty;
            try
            {
                session[CommonConst.UserSession] = admin;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                retMsg = ex.Message;
                FileLoggerHelper.WriteErrorLog(string.Format("SessionHelper::SetUser出现异常:{0}", ex.Message));
            }
            return Tuple.Create(isSuccess, retMsg);
        }
    }
}