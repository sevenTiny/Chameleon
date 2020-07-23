using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chameleon.Application
{
    public interface IFileApp
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileUploadPayload"></param>
        /// <returns></returns>
        string Upload(FileUploadPayload fileUploadPayload);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="organization">所属组织</param>
        /// <param name="fileId">文件id</param>
        void Delete(long userId, int userRole, Guid organization, string fileId);
        /// <summary>
        /// 下载文件并返回文件媒体类型
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="organization">所属组织</param>
        /// <param name="fileId">文件id</param>
        /// <param name="downloadStream">下载流</param>
        FileDownloadPayload Download(long userId, int userRole, Guid organization, string fileId, Stream downloadStream);
    }

    public class FileApp : IFileApp
    {
        IFileService _fileService;
        IOrganizationService _organizationService;
        public FileApp(IOrganizationService organizationService, IFileService fileService)
        {
            _organizationService = organizationService;
            _fileService = fileService;
        }

        public void Delete(long userId, int userRole, Guid organization, string fileId)
        {
            if (!_fileService.ExistsById(fileId))
                throw new FileNotFoundException($"File not found by fileId:[{fileId}]");

            var meta = _fileService.GetFileMetaDataByFileId(fileId);

            if (meta.IsSystemFile == 1)
            {
                //系统文件只有开发人员才可以删除
                if (userRole != (int)RoleEnum.Developer)
                    throw new InvalidOperationException("No Permission");
            }
            else
            {
                //获取员工有权限的所有组织
                var permissionOrgs = _organizationService.GetPermissionOrganizations(organization);

                if (!permissionOrgs.Contains(meta.Organization.ToString()))
                    throw new InvalidOperationException("No Permission");
            }

            _fileService.Delete(fileId);
        }

        public FileDownloadPayload Download(long userId, int userRole, Guid organization, string fileId, Stream downloadStream)
        {
            if (!_fileService.ExistsById(fileId))
                throw new FileNotFoundException($"File not found by fileId:[{fileId}]");

            var meta = _fileService.GetFileMetaDataByFileId(fileId);

            //如果是系统文件，则不走权限过滤，所有人都可以下载
            if (meta.IsSystemFile != 1)
            {
                //获取员工有权限的所有组织
                var permissionOrgs = _organizationService.GetPermissionOrganizations(organization);

                if (!permissionOrgs.Contains(meta.Organization.ToString()))
                    throw new InvalidOperationException("No Permission");
            }

            _fileService.Download(fileId, downloadStream);

            return new FileDownloadPayload(meta);
        }

        public string Upload(FileUploadPayload fileUploadPayload)
        {
            return _fileService.Upload(fileUploadPayload);
        }
    }
}
