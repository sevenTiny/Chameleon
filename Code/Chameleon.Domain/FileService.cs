using Chameleon.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;
using System.IO;
using MongoDB.Driver;
using SevenTiny.Bantina.Validation;

namespace Chameleon.Domain
{
    /// <summary>
    /// 文件管理
    /// </summary>
    public interface IFileService
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
        /// <param name="fileId"></param>
        void Delete(string fileId);
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="downloadStream"></param>
        void Download(string fileId, Stream downloadStream);
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        bool ExistsById(string fileId);
        /// <summary>
        /// 获取文件元数据
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        FileMetaData GetFileMetaDataByFileId(string fileId);
    }

    public class FileService : IFileService
    {
        ChameleonDataDbContext _chameleonDataDbContext;
        public FileService(ChameleonDataDbContext chameleonDataDbContext)
        {
            _chameleonDataDbContext = chameleonDataDbContext;
        }

        private MongoGridFS GetMongoFs()
        {
            MongoGridFSSettings fsSetting = new MongoGridFSSettings() { Root = "Chameleon.FileSystem" };
            MongoGridFS fs = new MongoGridFS("", fsSetting);
            return fs;
        }

        public string Upload(FileUploadPayload fileUploadPayload)
        {
            Ensure.ArgumentNotNullOrEmpty(fileUploadPayload, nameof(fileUploadPayload));
            Ensure.ArgumentNotNullOrEmpty(fileUploadPayload.FileName, nameof(fileUploadPayload.FileName));
            Ensure.ArgumentNotNullOrEmpty(fileUploadPayload.ContentType, nameof(fileUploadPayload.ContentType));

            MongoGridFS fs = GetMongoFs();

            MongoGridFSCreateOptions option = new MongoGridFSCreateOptions();

            BsonDocument doc = new BsonDocument();

            doc.Add("FileName", fileUploadPayload.FileName);
            doc.Add("ContentType", fileUploadPayload.ContentType);
            doc.Add("UploadTime", fileUploadPayload.UploadTime);
            doc.Add("UserId", fileUploadPayload.UserId);
            doc.Add("Organization", fileUploadPayload.Organization);

            option.Metadata = doc; // 添加metadata数据

            var result = fs.Upload(fileUploadPayload.UploadFileStream, fileUploadPayload.FileName, option);

            return result.Id.ToString();
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public bool ExistsById(string fileId)
        {
            Ensure.ArgumentNotNullOrEmpty(fileId, nameof(fileId));

            MongoGridFS fs = GetMongoFs();

            return fs.ExistsById(fileId);
        }

        /// <summary>
        /// 获取文件元数据
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public FileMetaData GetFileMetaDataByFileId(string fileId)
        {
            Ensure.ArgumentNotNullOrEmpty(fileId, nameof(fileId));

            MongoGridFS fs = GetMongoFs();

            var meta = fs.FindOneById(fileId).Metadata;

            return new FileMetaData
            {
                FileName = meta["FileName"].AsString,
                ContentType = meta["ContentType"].AsString,
                UploadTime = meta["UploadTime"].ToUniversalTime(),
                UserId = meta["UserId"].AsInt64,
                Organization = meta["Organization"].AsGuid,
            };
        }

        public void Delete(string fileId)
        {
            Ensure.ArgumentNotNullOrEmpty(fileId, nameof(fileId));

            MongoGridFS fs = GetMongoFs();

            fs.DeleteById(fileId);
        }

        public void Download(string fileId, Stream downloadStream)
        {
            Ensure.ArgumentNotNullOrEmpty(fileId, nameof(fileId));

            MongoGridFS fs = GetMongoFs();

            if (!fs.ExistsById(fileId))
                return;

            var meta = fs.FindOneById(fileId);

            fs.Download(downloadStream, meta);
        }
    }

    /// <summary>
    /// 上传载荷
    /// </summary>
    public class FileUploadPayload : FileOperationPayload
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream UploadFileStream { get; set; }
    }

    /// <summary>
    /// 文件下载载荷
    /// </summary>
    public class FileDownloadPayload : FileOperationPayload
    {
        public FileDownloadPayload() { }
        public FileDownloadPayload(FileMetaData fileMetaData)
        {
            this.FileName = fileMetaData.FileName;
            this.ContentType = fileMetaData.ContentType;
            this.UserId = fileMetaData.UserId;
            this.Organization = fileMetaData.Organization;
            this.UploadTime = fileMetaData.UploadTime;
        }
    }

    /// <summary>
    /// 文件元数据信息
    /// </summary>
    public class FileMetaData : FileOperationPayload
    {

    }

    public abstract class FileOperationPayload
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 上传userid
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public Guid Organization { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }
    }
}
