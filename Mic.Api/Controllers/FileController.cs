using Mic.Api.Common;
using Mic.Api.Filter;
using Mic.Api.Models;
using Mic.Entity.Api.Param;
using Mic.Repository.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    public class FileController : ApiController
    {
        /// <summary>
        /// 上传音频文件,自动识别音频时长 [FileUpload][AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("uploadAudioFile")]
        [AccessTokenAuthorize]
        public async Task<ResponseResultDto<UploadFileInfo>> UploadAudioFile()
        {
            UploadFileInfo info = new UploadFileInfo();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HostingEnvironment.MapPath("/UploadFile/Audio/");

            if (!Directory.Exists(root)) Directory.CreateDirectory(root);

            var provider = new MultipartFormDataMemoryStreamProvider();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                
                var item = provider.Contents[0];
                //foreach (var item in provider.Contents)
                //{
                string fileNameStr = item.Headers.ContentDisposition.FileName.Trim('"');
                var arr = fileNameStr.Split('.');
                string extensionName = arr[arr.Length - 1]; //得到扩展名
                string fileName = Guid.NewGuid().ToString();
                string fullPath = Path.Combine(root, fileName + "." + extensionName);
                var ms = item.ReadAsStreamAsync().Result;
                using (var br = new BinaryReader(ms))
                {
                    var data = br.ReadBytes((int)ms.Length);
                    File.WriteAllBytes(fullPath, data);
                }
                //}
                Duration duration = new ByShell32();
                var result = duration.GetDuration(fullPath);
                FileInfo fileInfo = new FileInfo(fullPath);
                info = new UploadFileInfo
                {
                    FilePath = "/UploadFile/Audio/" + fileName + "." + extensionName,
                    Duration = result.Item1,
                    FileSize = fileInfo.Length
                };


            }
            catch (Exception ex)
            {
                Logger.FileLoggerHelper.WriteErrorLog(DateTime.Now + "：上传音频文件错误-" + ex.Message);
            }
            return new ResponseResultDto<UploadFileInfo>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = info
            };
        }


        /// <summary>
        /// 上传一般文件[FileUpload][AUTH]
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("uploadOrdinaryFile")]
        [AccessTokenAuthorize]
        public async Task<ResponseResultDto<UploadFileInfo>> UploadOrdinaryFile()
        {
            UploadFileInfo info = new UploadFileInfo();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HostingEnvironment.MapPath("/UploadFile/Other/");

            if (!Directory.Exists(root)) Directory.CreateDirectory(root);

            var provider = new MultipartFormDataMemoryStreamProvider();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                var item = provider.Contents[0];
                //foreach (var item in provider.Contents)
                //{
                string fileNameStr = item.Headers.ContentDisposition.FileName.Trim('"');
                var arr = fileNameStr.Split('.');
                string extensionName = arr[arr.Length - 1]; //得到扩展名
                string fileName = Guid.NewGuid().ToString();
                string fullPath = Path.Combine(root, fileName + "." + extensionName);
                var ms = item.ReadAsStreamAsync().Result;
                using (var br = new BinaryReader(ms))
                {
                    var data = br.ReadBytes((int)ms.Length);
                    File.WriteAllBytes(fullPath, data);
                }
                info = new UploadFileInfo
                {
                    FilePath = "/UploadFile/Other/" + fileName + "." + extensionName
                };


            }
            catch (Exception ex)
            {
                Logger.FileLoggerHelper.WriteErrorLog(DateTime.Now + "：上传一般文件错误-" + ex.Message);
            }
            return new ResponseResultDto<UploadFileInfo>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = info
            };
        }
    }
}
