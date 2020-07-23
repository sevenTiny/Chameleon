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
        /// <summary>
        /// 获得读文件流
        /// </summary>
        /// <param name="fileOperationPayload"></param>
        Stream OpenRead(FileOperationPayload fileOperationPayload);
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
            MongoGridFS fs = new MongoGridFS(_chameleonDataDbContext.GetMongoServer(), "ChameleonDataDbContext", fsSetting);
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

            var innerFileName = Guid.NewGuid().ToString();

            doc.Add("InnerFileName", innerFileName);//对内使用的文件名，用于文件下载等场景
            doc.Add("FileName", fileUploadPayload.FileName);
            doc.Add("ContentType", fileUploadPayload.ContentType);
            doc.Add("UploadTime", fileUploadPayload.UploadTime);
            doc.Add("UserId", fileUploadPayload.UserId);
            doc.Add("Organization", fileUploadPayload.Organization);
            doc.Add("IsSystemFile", fileUploadPayload.IsSystemFile);

            option.Metadata = doc; // 添加metadata数据

            var result = fs.Upload(fileUploadPayload.UploadFileStream, innerFileName, option);

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

            return fs.ExistsById(ObjectId.Parse(fileId));
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

            var fileMeta = fs.FindOneById(ObjectId.Parse(fileId));

            var meta = fileMeta.Metadata;

            return new FileMetaData
            {
                FileName = meta["FileName"].AsString,
                InnerFileName = meta["InnerFileName"].AsString,
                ContentType = meta["ContentType"].AsString,
                UploadTime = meta["UploadTime"].ToUniversalTime(),
                UserId = meta["UserId"].AsInt64,
                Organization = meta["Organization"].AsGuid,
                IsSystemFile = meta["IsSystemFile"].AsInt32,
                MongoGridFSFileInfo = fileMeta
            };
        }

        public void Delete(string fileId)
        {
            Ensure.ArgumentNotNullOrEmpty(fileId, nameof(fileId));

            MongoGridFS fs = GetMongoFs();

            fs.DeleteById(fileId);
        }

        public Stream OpenRead(FileOperationPayload fileOperationPayload)
        {
            Ensure.ArgumentNotNullOrEmpty(fileOperationPayload, nameof(fileOperationPayload));
            Ensure.ArgumentNotNullOrEmpty(fileOperationPayload.InnerFileName, nameof(fileOperationPayload.InnerFileName));

            MongoGridFS fs = GetMongoFs();

            return fs.OpenRead(fileOperationPayload.InnerFileName);
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
        public Stream ReadStream { get; set; }
    }

    /// <summary>
    /// 文件元数据信息
    /// </summary>
    public class FileMetaData : FileOperationPayload
    {
        //文件元数据
        public MongoGridFSFileInfo MongoGridFSFileInfo { get; set; }
    }

    /// <summary>
    /// 文件操作载荷
    /// </summary>
    public abstract class FileOperationPayload
    {
        /// <summary>
        /// 对内使用的文件名，上传时使用Guid生成，用于下载等场景确定唯一文件
        /// </summary>
        public string InnerFileName { get; set; }
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
        /// <summary>
        /// 是否系统文件（系统文件全局可以访问，但是只有开发人员可以删除）
        /// </summary>
        public int IsSystemFile { get; set; }
    }
}
