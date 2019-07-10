using Mic.Repository.Utils;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Mic.Web.Controllers
{
    public class FileController : BaseController
    {
        [HttpPost]
        public ActionResult UploadAudioFile()
        {
            try
            {
                //HttpPostedFileBase
                //HttpFileCollection filelist = HttpContext.Current.Request.Files;
                ///获取上传的文件
                var file = Request.Files[0];
                //HttpFileCollectionBase files = Request.Files;
                //HttpPostedFileBase file = files[0];

                var strServerFilePath = Server.MapPath("/UploadFile/Audio/");

                if (!Directory.Exists(strServerFilePath))
                    Directory.CreateDirectory(strServerFilePath);
                int fileSize = file.ContentLength; //得到文件大小
                string extensionName = Path.GetExtension(file.FileName); //得到扩展名
                string fileName = Guid.NewGuid().ToString();
                string fullPath = Path.Combine(strServerFilePath, fileName + extensionName);
                file.SaveAs(fullPath);

                Duration duration = new ByShell32();
                var result = duration.GetDuration(fullPath);
                return Json(new { status = true, fileName = fileName + extensionName, duration = result.Item1 });
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}