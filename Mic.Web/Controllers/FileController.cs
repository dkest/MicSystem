using Mic.Repository.Utils;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class FileController : Controller
    {
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            var strServerFilePath = Server.MapPath("/UploadFile/");

            if (!Directory.Exists(strServerFilePath))
                Directory.CreateDirectory(strServerFilePath);
            int fileSize = file.ContentLength; //得到文件大小
            string extensionName = Path.GetExtension(file.FileName); //得到扩展名
            string fileName = new Guid().ToString();
            string fullPath = Path.Combine(strServerFilePath, fileName + extensionName);
            file.SaveAs(fullPath);

            Duration duration = new ByShell32();
            var result = duration.GetDuration(fullPath);

            return Json(new { status = true, fileName= fileName + extensionName, duration= result.Item1 });
        }
    }
}