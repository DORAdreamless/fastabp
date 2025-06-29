using System;
using System.IO;

namespace HB.AbpFundation.Helpers
{
    public enum FileType
    {
        Image,
        Video,
        Audio,
        Document,
        Other
    }
    /// <summary>
    /// 文件操作处理类
    /// </summary>
    public class FileHelper
    {

        /// <summary>
        /// 生成保存文件的目录
        /// </summary>
        /// <param name="pathPrefix">路径前缀</param>
        /// <param name="type"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string CreateFilePath(string pathPrefix, FileType type, string ext)
        {
            string typeName = type.ToString().ToLower() + "s";
            string fileName = Guid.NewGuid().ToString("N") + ext;
            string path = $"{pathPrefix}/{typeName}/{DateTime.Today.Year}/{DateTime.Today.ToString("MM")}";
            var filePath = Path.Combine(AppContext.BaseDirectory, path);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return $"{path}/{fileName}";
        }



        /// <summary>
        /// 根据文件后缀获取资源类型
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static FileType CreateResourceType(string ext)
        {
            switch (ext)
            {
                case ".svg":
                case ".jpg":
                case ".jpeg":
                case ".webp":
                case ".png":
                case ".gif":
                case ".tif":
                case ".bmp":
                    return FileType.Image;
                case ".mp4":
                case ".ogg":
                case ".webm":
                case ".avi":
                case ".wmv":
                case ".mpg":
                case ".mov":
                case ".rm":
                case ".rmvb":
                    return FileType.Video;
                case ".mp2":
                case ".mp3":
                case ".wav":
                case ".wma":
                    return FileType.Audio;
                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".ppt":
                case ".pptx":
                    return FileType.Document;
                default:
                    return FileType.Other;
            }
        }


        /// <summary>
        /// 获取音频或视频的长度
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetResourceDurationLength(string filePath)
        {
            return 0;
        }
        /// <summary>
        /// 创建文件的文件夹
        /// </summary>
        /// <param name="zipPath"></param>
        public static void CreateDirectoryIfNotExist(string zipPath)
        {
            string directoryName = Path.GetDirectoryName(zipPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
    }
}
