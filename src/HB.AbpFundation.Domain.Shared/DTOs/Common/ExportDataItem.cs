using System;
using HB.AbpFundation.Helpers;

namespace HB.AbpFundation.DTOs.Common
{
    public class ExportDataItem
    {
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Done { get; set; }
        /// <summary>
        /// 导出文件
        /// </summary>
        public string FilePath { get; set; } 

        /// <summary>
        /// 进度
        /// </summary>
        public decimal Progress { get; set; }

        public ExportDataItem(string title,string ext=".xls")
        {
            FilePath = $"ExportFile/{title}/{title}{DateTime.Now.ToString("yyyy-MM-dd")}{ext}";
            FileHelper.CreateDirectoryIfNotExist(FilePath);
        }
    }
}
