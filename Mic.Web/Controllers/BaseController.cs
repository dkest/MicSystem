using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }
        public double GetDoubleValFromReq(string paramName)
        {
            double retVal = 0;
            string tmp = Request[paramName];
            if (tmp != null && tmp != "undefined" && tmp.Length > 0)
            {
                retVal = Convert.ToDouble(tmp);//显示行数
            }
            return retVal;
        }
        public int GetIntValFromReq(string paramName)
        {
            int retVal = 0;
            string tmp = Request[paramName];
            if (tmp != null && tmp != "undefined" && tmp.Length > 0)
            {
                retVal = Convert.ToInt32(tmp);//显示行数
            }
            return retVal;
        }
        public string GetStrValFromReq(string paramName)
        {
            string retVal = string.Empty;
            string tmp = Request[paramName];
            if (tmp != null)
            {
                retVal = tmp;//显示行数
            }
            return retVal;
        }
        public DateTime? GetDateTimeValFromReq(string paramName)
        {
            string retVal = string.Empty;
            string tmp = Request[paramName];
            if (tmp != null)
            {
                retVal = tmp;//显示行数
            }
            return string.IsNullOrEmpty(retVal) ? null : (DateTime?)DateTime.Parse(retVal.Trim());
        }
        public List<int> GetIntListValFromReq(string paramName)
        {

            string multiValues = GetStrValFromReq(paramName);
            multiValues = multiValues.Replace("[", "").Replace("]", "").Replace("\"", "");
            string[] multiArray = multiValues.Split(',');
            List<int> multiList = new List<int>();
            foreach (string w in multiArray)
            {
                if (w != null && w != "")
                {
                    multiList.Add(Convert.ToInt32(w));
                }
            }
            return multiList;

        }
        public string[] GetStrArrayValFromReq(string paramName)
        {
            string multiValues = GetStrValFromReq(paramName);
            multiValues = multiValues.Replace("[", "").Replace("]", "").Replace("\"", "");
            string[] multiArray = multiValues.Split(',');
            return multiArray;
        }

    }
}
