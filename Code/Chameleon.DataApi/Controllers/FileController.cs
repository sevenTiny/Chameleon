using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ApiControllerCommonBase
    {
        IFileApp _fileApp;
        public FileController(IFileApp fileApp)
        {
            _fileApp = fileApp;
        }

        [HttpPost]
        public IActionResult Post([FromForm]IFormCollection formData)
        {
            return SafeExecute(() =>
            {
                IFormFileCollection files = formData.Files;

                if (files == null || !files.Any())
                    return Result.Info("未找到文件").ToJsonResult();

                var successList = new List<string>(files.Count);

                foreach (var item in files)
                {
                    var fileUploadPayload = new FileUploadPayload
                    {
                        FileName = item.FileName,
                        ContentType = item.ContentType,
                        UserId = CurrentUserId,
                        Organization = CurrentOrganization,
                        UploadTime = DateTime.Now
                    };

                    fileUploadPayload.UploadFileStream = item.OpenReadStream();

                    successList.Add(_fileApp.Upload(fileUploadPayload));
                }

                return Result<List<string>>.Success($"Success 共成功上传[{successList.Count}]个文件", successList).ToJsonResult();
            });
        }

        [HttpPost]
        public IActionResult Delete(string fileId)
        {
            return SafeExecute(() =>
            {
                _fileApp.Delete(CurrentUserId, CurrentOrganization, fileId);

                return JsonResultSuccess();
            });
        }

        [Route("api/file/view")]
        public IActionResult GetFileView(string fileId)
        {
            return SafeExecute(() =>
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var downloadPayload = _fileApp.Download(CurrentUserId, CurrentOrganization, fileId, stream);

                    var bytes = new byte[stream.Length];

                    stream.Read(bytes, 0, bytes.Length);

                    return File(bytes, downloadPayload.ContentType);
                }
            });
        }

        [Route("api/file/download")]
        public IActionResult Download(string fileId)
        {
            return SafeExecute(() =>
            {
                var stream = Response.Body;

                var downloadPayload = _fileApp.Download(CurrentUserId, CurrentOrganization, fileId, stream);

                return new FileStreamResult(stream, downloadPayload.ContentType);
            });
        }
    }
}