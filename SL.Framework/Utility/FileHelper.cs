using System;
using System.IO;
using System.IO.Compression;

namespace SL.Framework.Utility
{
    /// <summary>
    /// FILE 도우미
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 파일명 구하기
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static string GetFileName(string filePathName)
        {
            return Path.GetFileName(filePathName);
        }

        /// <summary>
        /// UNIQUE 파일명 구하기
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static string GetUniqueFileName(string filePathName)
        {
            return string.Format("{0}_{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), KeyHelper.RngUniqueKey(),
                Path.GetExtension(filePathName));
        }

        /// <summary>
        /// FILE 삭제
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static bool FileDelete(string filePathName)
        {
            if (File.Exists(filePathName))
                File.Delete(filePathName);

            return true;
        }

        /// <summary>
        /// FILE 이동
        /// </summary>
        /// <param name="sourceFilePathName"></param>
        /// <param name="destFilePathName"></param>
        /// <returns></returns>
        public static bool FileMove(string sourceFilePathName, string destFilePathName)
        {
            if (File.Exists(sourceFilePathName))
                File.Move(sourceFilePathName, destFilePathName);

            return true;
        }

        /// <summary>
        /// 폴더 복사
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}