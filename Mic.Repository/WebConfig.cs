using System.Configuration;

namespace Mic.Repository
{
    public static class WebConfig
    {
        //public static string SqlConnection = @"server=(localdb)\MSSQLLocalDB;user id=sa;password=123456;persistsecurityinfo=True;database=MicSystem;";


        public static string SqlConnection = ConfigurationManager.ConnectionStrings["strCon"].ToString();
        //@"server=152.136.227.143;user id=sa;password=Admin123;persistsecurityinfo=True;database=MicSystem;";

       //string connStr = ConfigurationManager.AppSettings["ConnectionString"];
    }
}
