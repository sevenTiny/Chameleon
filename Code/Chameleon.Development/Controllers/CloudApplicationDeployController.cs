using Chameleon.Domain;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chameleon.Development.Controllers
{
    public class CloudApplicationDeployController : WebControllerBase
    {
        ICloudApplicationDeployService _cloudApplicationDeployService;
        ICloudApplicationRepository _cloudApplicationRepository;
        IMetaObjectRepository _metaObjectRepository;
        public CloudApplicationDeployController(IMetaObjectRepository metaObjectRepository, ICloudApplicationRepository cloudApplicationRepository, ICloudApplicationDeployService cloudApplicationDeployService)
        {
            _metaObjectRepository = metaObjectRepository;
            _cloudApplicationRepository = cloudApplicationRepository;
            _cloudApplicationDeployService = cloudApplicationDeployService;
        }

        //元数据文件后缀（更改时记得把前端导入accept也一起调整）
        const string MetaDataFileExtension = ".chameleonmeta";

        private void QuerySelectDatas()
        {
            //应用下对象
            ViewData["MetaObjects"] = _metaObjectRepository.GetMetaObjectListUnDeletedByApplicationId(CurrentApplicationId);
        }

        public IActionResult Export()
        {
            QuerySelectDatas();
            return View(ResponseModel.Success());
        }

        /// <summary>
        /// 整个应用导出
        /// </summary>
        /// <returns></returns>
        public IActionResult AllCloudApplicationExport()
        {
            var application = _cloudApplicationRepository.GetById(CurrentApplicationId);

            if (application == null)
            {
                QuerySelectDatas();
                return View("Export", ResponseModel.Error("应用不存在"));
            }

            var deployDto = _cloudApplicationDeployService.CloudApplicationExport(CurrentApplicationId);

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deployDto));

            return File(bytes, "application/json", $"{application.Name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{MetaDataFileExtension}");
        }

        /// <summary>
        /// 单个对象导出
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        public IActionResult MetaObjectDataExport(Guid metaObjectId)
        {
            var metaObject = _metaObjectRepository.GetById(metaObjectId);

            if (metaObject == null)
            {
                QuerySelectDatas();
                return View("Export", ResponseModel.Error("对象不存在"));
            }

            var deployDto = _cloudApplicationDeployService.MetaObjectExport(metaObjectId);

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deployDto));

            return File(bytes, "application/json", $"{metaObject.Name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{MetaDataFileExtension}");
        }

        /// <summary>
        /// 应用接口导出
        /// </summary>
        /// <returns></returns>
        public IActionResult DataSourceExport()
        {
            var application = _cloudApplicationRepository.GetById(CurrentApplicationId);

            if (application == null)
            {
                QuerySelectDatas();
                return View("Export", ResponseModel.Error("应用不存在"));
            }

            var deployDto = _cloudApplicationDeployService.DataSourceExport(CurrentApplicationId);

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deployDto));

            return File(bytes, "application/json", $"{application.Name}_ApplicationInterface_{DateTime.Now.ToString("yyyyMMddHHmmss")}{MetaDataFileExtension}");
        }

        /// <summary>
        /// 身份菜单功能
        /// </summary>
        /// <returns></returns>
        public IActionResult ProfileMenuFunc()
        {
            var deployDto = _cloudApplicationDeployService.ProfileMenuFunc();

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deployDto));

            return File(bytes, "application/json", $"ProfileMenuFunction_{DateTime.Now.ToString("yyyyMMddHHmmss")}{MetaDataFileExtension}");
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

            var successList = new List<Tuple<string, List<string>>>(files.Count);

            foreach (var item in files)
            {
                if (!".chameleonmeta".Equals(Path.GetExtension(item.FileName)))
                {
                    successList.Add(Tuple.Create($"导入元数据文件：{item.FileName} 失败，元数据文件类型不匹配", new List<string>(0)));
                    continue;
                }

                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        item.CopyTo(stream);
                        var bytes = stream.ToArray();
                        var content = Encoding.UTF8.GetString(bytes);
                        var dto = JsonConvert.DeserializeObject<CloudApplicationDeployDto>(content);
                        var result = _cloudApplicationDeployService.CloudApplicationImport(dto);

                        if (result.IsSuccess)
                            successList.Add(Tuple.Create($"导入元数据文件文件：{item.FileName}", result.MoreMessage));
                        else
                            successList.Add(Tuple.Create($"导入元数据文件文件：{item.FileName} 失败", result.MoreMessage));
                    }
                }
                catch (Exception ex)
                {
                    successList.Add(Tuple.Create($"导入元数据文件文件：{item.FileName} 失败，元数据文件内容不正确，ex:{ex.ToString()}", new List<string>(0)));
                    continue;
                }
            }

            return Content(JsonHelper.FormatJsonString(JsonConvert.SerializeObject(successList)));
        }
    }
}