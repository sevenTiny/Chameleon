using System.IO;

namespace Chameleon.Common.Models
{
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
}
