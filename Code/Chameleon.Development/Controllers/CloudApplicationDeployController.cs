using Chameleon.Application;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;

namespace Chameleon.Development.Controllers
{
    public class CloudApplicationDeployController : WebControllerBase
    {
        ICloudApplicationDeployService _cloudApplicationDeployService;
        ICloudApplicationRepository _cloudApplicationRepository;
        ICloudApplicationApp _cloudApplicationApp;
        public CloudApplicationDeployController(ICloudApplicationApp cloudApplicationApp, ICloudApplicationRepository cloudApplicationRepository, ICloudApplicationDeployService cloudApplicationDeployService)
        {
            _cloudApplicationApp = cloudApplicationApp;
            _cloudApplicationRepository = cloudApplicationRepository;
            _cloudApplicationDeployService = cloudApplicationDeployService;
        }

        public IActionResult Export()
        {
            ViewData["CloudApplications"] = _cloudApplicationApp.GetUserPermissionApplications(CurrentUserId);
            return View(ResponseModel.Success());
        }

        /// <summary>
        /// 整个应用导出
        /// </summary>
        /// <param name="cloudApplicationId"></param>
        /// <returns></returns>
        public IActionResult AllCloudApplicationExport(Guid cloudApplicationId)
        {
            var application = _cloudApplicationRepository.GetById(cloudApplicationId);

            if (application == null)
            {
                ViewData["CloudApplications"] = _cloudApplicationApp.GetUserPermissionApplications(CurrentUserId);
                return View("Export", ResponseModel.Error("应用不存在"));
            }

            var deployDto = _cloudApplicationDeployService.AllCloudApplicationExport(cloudApplicationId);

            var json = JsonConvert.SerializeObject(deployDto);

            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", $"{application.Name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.json");
        }

        public IActionResult Import()
        {
            return View(ResponseModel.Success());
        }

        public IActionResult AllCloudApplicationImport([FromForm]IFormCollection formData)
        {
            IFormFileCollection files = formData.Files;

            if (files == null || !files.Any())
                return View("Import", ResponseModel.Error("未找到文件"));

            var successList = new List<List<string>>(files.Count);

            foreach (var item in files)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    item.CopyTo(stream);
                    var bytes = stream.ToArray();
                    var content = Encoding.UTF8.GetString(bytes);
                    var dto = JsonConvert.DeserializeObject<CloudApplicationDeployDto>(content);
                    var result = _cloudApplicationDeployService.AllCloudApplicationImport(dto);

                    if (result.IsSuccess)
                        successList.Add(result.MoreMessage);
                }
            }

            return Content(JsonConvert.SerializeObject(successList));
        }
    }
}