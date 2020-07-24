using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Chameleon.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : InterfaceApiControllerBase
    {
        IFileApp _fileApp;
        public FileController(IFileApp fileApp, ITriggerScriptRepository triggerScriptRepository, IInterfaceSettingRepository interfaceSettingRepository)
            : base(
                 triggerScriptRepository,
                 interfaceSettingRepository
                 )
        {
            _fileApp = fileApp;
        }

        [HttpPost]
        [RequestSizeLimit(52428800)]//限制上传文件大小不得超过50M=50*1024*1024(B)
        public IActionResult Post([FromForm]IFormCollection formData)
        {
            return SafeExecute(() =>
            {
                IFormFileCollection files = formData.Files;

                if (files == null || !files.Any())
                    return Result.Info("file not found").ToJsonResult();

                var successList = new List<string>(files.Count);

                foreach (var item in files)
                {
                    if (item.Length > 52428800)
                        return Result.Error("file can not large than 50MB.").ToJsonResult();

                    var fileUploadPayload = new FileUploadPayload
                    {
                        FileName = item.FileName,
                        ContentType = item.ContentType,
                        UserId = CurrentUserId,
                        Organization = CurrentOrganization,
                        UploadTime = DateTime.Now,
                        IsSystemFile = 0//业务线上传的文件未非系统文件
                    };

                    fileUploadPayload.UploadFileStream = item.OpenReadStream();

                    successList.Add(_fileApp.Upload(fileUploadPayload));
                }

                return Result<List<string>>.Success($"Success 共成功上传[{successList.Count}]个文件", successList).ToJsonResult();
            });
        }

        [HttpDelete]
        public IActionResult Delete(string fileId)
        {
            return SafeExecute(() =>
            {
                if (string.IsNullOrEmpty(fileId))
                    return Result.Error("Parameter invalid: fileId is null").ToJsonResult();

                _fileApp.Delete(CurrentUserId, CurrentUserRole, CurrentOrganization, fileId);

                return JsonResultSuccess();
            });
        }

        [Route("Download")]
        [HttpGet]
        public IActionResult Download(string fileId)
        {
            return SafeExecute(() =>
            {
                if (string.IsNullOrEmpty(fileId))
                    return Result.Error("Parameter invalid: fileId is null").ToJsonResult();

                var downloadPayload = _fileApp.Download(CurrentUserId, CurrentUserRole, CurrentOrganization, fileId);

                Response.ContentType = downloadPayload.ContentType;

                return File(downloadPayload.ReadStream, downloadPayload.ContentType, downloadPayload.FileName);
            });
        }
    }
}