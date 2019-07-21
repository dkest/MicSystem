using System;
using System.IO;
using System.Web;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        public string PostFiles()
        {
            string result = "";
            HttpFileCollection filelist = HttpContext.Current.Request.Files;
            if (filelist != null && filelist.Count > 0)
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    HttpPostedFile file = filelist[i];
                    String Tpath = "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
                    string filename = file.FileName;
                    string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    //存放文件夹
                    string FilePath = Path.Combine(@"E:\bak\" + DateTime.Now.ToString("yyyyMMdd") + @"\");
                    DirectoryInfo di = new DirectoryInfo(FilePath);

                    //判断文件夹是否存在
                    if (!di.Exists) { di.Create(); }
                    try
                    {
                        file.SaveAs(FilePath + filename);

                        result = (Tpath + FileName).Replace("\\", "/");

                    }
                    catch (Exception ex)
                    {
                        result = "上传文件写入失败：" + ex.Message;
                    }
                }
            }
            else
            {
                result = "上传的文件信息不存在！";
            }

            return result;
        }
    }
}
