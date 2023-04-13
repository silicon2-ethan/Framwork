using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using SL.Framework.Extension;
using SL.Framework.Utility;

namespace SL.Framework.Utility
{
    /// <summary>
    /// FTP 도우미
    /// </summary>
    public sealed class FtpHelper
    {
        #region [Propertities]
        readonly string _ftpUserName;
        readonly string _ftpPassword;
        readonly string _ftpRootPath;
        #endregion
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="server"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public FtpHelper(string server, string user, string password)
        {
            _ftpRootPath = server.TrimEnd('/') + "/";
            _ftpUserName = user;
            _ftpPassword = password;
        }

        /// <summary>
        /// UPLOAD FILE 
        /// </summary>
        /// <param name="sourceFileFullPathName"></param>
        /// <param name="destFilePathName"></param>
        /// <returns></returns>
        public bool UploadFile(string sourceFileFullPathName, string destFilePathName)
        {
            CreateFtpDirectory(destFilePathName);

            var fi = new FileInfo(sourceFileFullPathName);
            var fs = fi.OpenRead();
            var length = fs.Length;

            var req = (FtpWebRequest)WebRequest.Create(_ftpRootPath + destFilePathName);
            req.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.ContentLength = length;
            req.Timeout = 10 * 1000;
            try
            {
                var stream = req.GetRequestStream();
                const int bufferLength = 4096;
                var b = new byte[bufferLength];
                int i;
                while ((i = fs.Read(b, 0, bufferLength)) > 0)
                {
                    stream.Write(b, 0, i);
                }
                stream.Close();
                stream.Dispose();
            }
            catch
            {
                return false;
            }
            finally
            {
                fs.Close();
                req.Abort();
            }
            req.Abort();
            return true;
        }

        /// <summary>
        /// DOWNLOAD FILE 
        /// </summary>
        /// <param name="ftpFilePathName"></param>
        /// <returns></returns>
        public byte[] DownloadFile(string ftpFilePathName)
        {
            var req = (FtpWebRequest)WebRequest.Create(_ftpRootPath + ftpFilePathName);
            req.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;
            try
            {
                var response = (FtpWebResponse)req.GetResponse();
                using (var ftpStream = response.GetResponseStream())
                using (var memoryStream = new MemoryStream())
                {
                    if (ftpStream != null) ftpStream.CopyTo(memoryStream);
                    response.Close();
                    req.Abort();
                    return memoryStream.ToArray();
                }
            }
            catch
            {
                req.Abort();
                return null;
            }
        }

        /// <summary>
        /// 실제 디렉토리에 존재하는 파일 다운로드
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="localFile"></param>
        /// <example>
        /// DownloadFile("QFSAdmin/Language/language.en.xml", @"D:\Upload\Test\Language\language.en.xml");
        /// </example>
        public void DownloadFile(string remoteFile, string localFile)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(localFile);
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_ftpRootPath + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    var ftpStream = ftpResponse.GetResponseStream();
                    FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                    byte[] byteBuffer = new byte[2048];
                    int bytesRead = ftpStream.Read(byteBuffer, 0, 2048);
                    try
                    {
                        while (bytesRead > 0)
                        {
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = ftpStream.Read(byteBuffer, 0, 2048);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        localFileStream.Close();
                        ftpStream.Close();
                        ftpResponse.Close();
                        ftpRequest = null;
                    }
                } //using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
            }
            catch (Exception ex)
            {
                new ExceptionExtension(ex);
            }
        }

        /// <summary>
        /// folder 단위로 다운로드 할 경우 사용한다. (윈도우 시스템에서만 사용한다.. 리눅스와 디렉토리 정보가 다름..)
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="localPath"></param>
        /// <example>
        /// DownloadFtpDirectoryOnWindowsSystem("QFSAdmin", @"D:\Upload\Test");
        /// </example>
        public void DownloadFtpDirectoryOnWindowsSystem(string folderName, string localPath)
        {
            try
            {
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(_ftpRootPath + folderName);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

                List<string> lines = new List<string>();

                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                using (Stream listStream = listResponse.GetResponseStream())
                using (StreamReader listReader = new StreamReader(listStream))
                {
                    while (!listReader.EndOfStream)
                    {
                        lines.Add(listReader.ReadLine());
                    }
                }

                foreach (string line in lines)
                {
                    if (line.vIsEmpty()) continue;
                    string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    int length = tokens.Length - 1;
                    string name = tokens[length];
                    string permissions = tokens[length - 1];

                    string localFilePath = Path.Combine(localPath, name);
                    string fileUrl = folderName + "/" + name;

                    if (permissions.vDefaultValue().ToString().ToLower().Contains("dir"))
                    {
                        if (!Directory.Exists(localFilePath))
                            Directory.CreateDirectory(localFilePath);
                        DownloadFtpDirectoryOnWindowsSystem(fileUrl + "/", localFilePath);
                    }
                    else
                        this.DownloadFile(fileUrl, localFilePath);
                } //foreach (string line in lines)
            }
            catch (Exception ex)
            {
                new ExceptionExtension(ex);
            }
        }

        /// <summary>
        /// DELETE FILE 
        /// </summary>
        /// <param name="ftpFilePathName"></param>
        /// <returns></returns>
        public bool DeleteFile(string ftpFilePathName)
        {
            var req = (FtpWebRequest)WebRequest.Create(_ftpRootPath + ftpFilePathName);
            req.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            req.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                var response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }

        /// <summary>
        /// CREATE DIRECTORY  
        /// </summary>
        /// <param name="filePathName"></param>
        private void CreateFtpDirectory(string filePathName)
        {
            var fullDir = FtpParseDirectory(filePathName);
            var dirs = fullDir.Split('/');
            var curDir = "/";
            foreach (var dir in dirs.Where(dir => !string.IsNullOrEmpty(dir)))
            {
                try
                {
                    curDir += dir + "/";
                    if (!CheckDirectoryExists(curDir))
                        MakeDirectory(curDir);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// PARSE DIRECTORY  
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        private static string FtpParseDirectory(string filePathName)
        {
            return filePathName.Substring(0, filePathName.LastIndexOf("/", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// CHECK DIRECTORY
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public bool CheckDirectoryExists(string directoryPath)
        {
            var req = (FtpWebRequest)WebRequest.Create(_ftpRootPath + directoryPath);
            req.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                var response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }

        /// <summary>
        /// CREATE DIRECTORY
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        private bool MakeDirectory(string directoryPath)
        {
            var req = (FtpWebRequest)WebRequest.Create(_ftpRootPath + directoryPath);
            req.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                var response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }
    }
}