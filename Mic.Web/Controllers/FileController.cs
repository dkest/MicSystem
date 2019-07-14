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
                var file = Request.Files[0];
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
                return Json(new { status = true, fileName = "/UploadFile/Audio/"+fileName + extensionName, duration = result.Item1 });
            }
            catch (Exception ex)
            {
                Logger.FileLoggerHelper.WriteErrorLog("upload Audio file error-"+ex.Message);
                throw;
            }

        }

        [HttpPost]
        public ActionResult UploadOrdinaryFile()
        {
            try
            {
                var file = Request.Files[0];
                var strServerFilePath = Server.MapPath("/UploadFile/Other/");

                if (!Directory.Exists(strServerFilePath))
                    Directory.CreateDirectory(strServerFilePath);
                int fileSize = file.ContentLength; //得到文件大小
                string extensionName = Path.GetExtension(file.FileName); //得到扩展名
                string fileName = Guid.NewGuid().ToString();
                string fullPath = Path.Combine(strServerFilePath, fileName + extensionName);
                file.SaveAs(fullPath);

                return Json(new { status = true, fileName = "/UploadFile/Other/" + fileName + extensionName });
            }
            catch (Exception ex)
            {
                Logger.FileLoggerHelper.WriteErrorLog("upload Audio file error-" + ex.Message);
                throw;
            }

        }
    }
}