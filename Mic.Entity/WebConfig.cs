using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Mic.Entity
{
    public static class WebConfig
    {
        public static  string RootUrl { get; set; }
        static WebConfig()
        {
            RootUrl = WebConfigurationManager.AppSettings["rootUrl"];
        }
    }
}
