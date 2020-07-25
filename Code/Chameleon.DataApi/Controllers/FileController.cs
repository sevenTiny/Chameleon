using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.Common;
using Chameleon.DataApi.Models;
using Chameleon.Domain;
using Chameleon.Entity;
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
        ITriggerScriptService _triggerScriptService;
        public FileController(ITriggerScriptService triggerScriptService, IFileApp fileApp, ITriggerScriptRepository triggerScriptRepository, IInterfaceSettingRepository interfaceSettingRepository)
            : base(
                 triggerScriptRepository,
                 interfaceSettingRepository
                 )
        {
            _triggerScriptService = triggerScriptService;
            _fileApp = fileApp;
        }

        /// <summary>
        /// 尝试执行服务对应的动态脚本，如果没有，则采用默认值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="interfaeServiceType"></param>
        /// <param name="parameters"></param>
        /// <param name="ifNoScriptReturnDefaultValue">默认值</param>
        /// <returns></returns>
        private TResult TryExecuteTriggerByServiceType<TResult>(InterfaceServiceTypeEnum interfaeServiceType, object[] parameters, TResult ifNoScriptReturnDefaultValue)
        {
            //拿到当前服务类型对应的脚本
            var triggerScript = _queryContext.TriggerScripts?.FirstOrDefault(t => t.InterfaceServiceType == (int)interfaeServiceType);

            if (triggerScript == null)
                return ifNoScriptReturnDefaultValue;

            //执行脚本
            var executeResult = _triggerScriptService.ExecuteTriggerScript<TResult>(triggerScript, parameters);

            if (!executeResult.IsSuccess)
                throw new InvalidOperationException(executeResult.Message);

            return executeResult.Data;
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

                //校验接口类型是否匹配
                if (_queryContext.InterfaceSetting.GetInterfaceType() != InterfaceTypeEnum.FileUpload)
                    return Result.Error("该接口不适用于该接口编码对应的接口类型").ToJsonResult();

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

                    //before
                    fileUploadPayload = TryExecuteTriggerByServiceType(InterfaceServiceTypeEnum.Application_UploadFile_Before, new object[] { _queryContext.TriggerContext, fileUploadPayload }, fileUploadPayload);

                    fileUploadPayload.UploadFileStream = item.OpenReadStream();

                    successList.Add(_fileApp.Upload(fileUploadPayload));

                    //after
                    TryExecuteTriggerByServiceType(InterfaceServiceTypeEnum.Application_UploadFile_After, new object[] { _queryContext.TriggerContext, fileUploadPayload }, fileUploadPayload);
                }

                return Result<List<string>>.Success($"Success 共成功上传[{successList.Count}]个文件", successList).ToJsonResult();
            });
        }

        [HttpGet]
        public IActionResult Get([FromQuery]QueryArgs queryArgs)
        {
            return SafeExecute(() =>
            {
                InitQueryContext(queryArgs);

                if (string.IsNullOrEmpty(queryArgs._fileId))
                    return Result.Error("Parameter invalid: _fileId is null").ToJsonResult();

                //校验接口类型是否匹配
                if (_queryContext.InterfaceSetting.GetInterfaceType() != InterfaceTypeEnum.FileDownload)
                    return Result.Error("该接口不适用于该接口编码对应的接口类型").ToJsonResult();

                //before
                TryExecuteTriggerByServiceType<object>(InterfaceServiceTypeEnum.Application_DownloadFile_Before, new object[] { _queryContext.TriggerContext }, null);

                var downloadPayload = _fileApp.Download(CurrentUserId, CurrentUserRole, CurrentOrganization, queryArgs._fileId);

                //after
                downloadPayload = TryExecuteTriggerByServiceType(InterfaceServiceTypeEnum.Application_DownloadFile_After, new object[] { _queryContext.TriggerContext, downloadPayload }, downloadPayload);

                Response.ContentType = downloadPayload.ContentType;

                return File(downloadPayload.ReadStream, downloadPayload.ContentType, downloadPayload.FileName);
            });
        }

        [HttpDelete]
        public IActionResult Delete(string _fileId)
        {
            return SafeExecute(() =>
            {
                if (string.IsNullOrEmpty(_fileId))
                    return Result.Error("Parameter invalid: fileId is null").ToJsonResult();

                _fileApp.Delete(CurrentUserId, CurrentUserRole, CurrentOrganization, _fileId);

                return JsonResultSuccess();
            });
        }
    }
}