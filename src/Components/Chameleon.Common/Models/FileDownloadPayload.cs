using System.IO;

namespace Chameleon.Common.Models
{
    /// <summary>
    /// 文件下载载荷
    /// </summary>
    public class FileDownloadPayload : FileOperationPayload
    {
        public Stream ReadStream { get; set; }
    }
}
