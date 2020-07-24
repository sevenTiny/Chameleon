using System;

namespace Chameleon.Common
{
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
