using HB.AbpFundation.Helpers;
using HB.AbpFundation.Persistences;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.AbpFundation.AggregateRoot.ObjectStorage
{
    /// <summary>
    /// 文件存储
    /// </summary>
    [Table("storage_file_storage")]
    public class FileStorage: PersistenceObjectBase
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [StringLength(100)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [StringLength(100)]
        public string FilePath { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 文件存储提供者（阿里云对象存储，Minio对象存储，七牛云对象存储）
        /// </summary>
        [StringLength(50)]
        public string FileProvider { get; set; }
    }

    
}
