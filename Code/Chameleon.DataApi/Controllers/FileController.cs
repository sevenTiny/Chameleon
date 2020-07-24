using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.DataApi.Models;
using Chameleon.Domain;
using Chameleon.Infrastructure.Consts;
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
        [RequestSizeLimit(CommonConst.MaxFileUploadSizeLimit)]//限制上传文件大小不得超过100M=100*1024*1024(B)
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromForm]IFormCollection formData)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                IFormFileCollection files = formData.Files;

                if (files == null || !files.Any())
                    return Result.Info("file not found").ToJsonResult();

                var successList = new List<string>(files.Count);

                foreach (var item in files)
                {
                    //校验文件大小
                    if (item.Length > _queryContext.InterfaceSetting.FileSizeLimit)
                        return Result.Error($"file [{item.FileName}] cannot large than {_queryContext.InterfaceSetting.FileSizeLimit} byte").ToJsonResult();

                    //校验文件后缀
                    var extensionsLimit = _queryContext.InterfaceSetting.FileExtensionLimit?.Split('|') ?? new string[0];

                    if (extensionsLimit.Any() && !extensionsLimit.Contains(Path.GetExtension(item.FileName)))
                        return Result.Error($"The [{item.FileName}] file type does not conform to the interface definition").ToJsonResult();
                }

                foreach (var item in files)
                {
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
        public IActionResult Download([FromQuery]QueryArgs queryArgs)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                if (string.IsNullOrEmpty(queryArgs._fileId))
                    return Result.Error("Parameter invalid: fileId is null").ToJsonResult();

                var downloadPayload = _fileApp.Download(CurrentUserId, CurrentUserRole, CurrentOrganization, queryArgs._fileId);

                Response.ContentType = downloadPayload.ContentType;

                return File(downloadPayload.ReadStream, downloadPayload.ContentType, downloadPayload.FileName);
            });
        }
    }
}