using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// ftp辅助类
    /// </summary>
    public class NFTP
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFilePath">本地文件绝对路径</param>
        /// <param name="ftpUrl">上传到ftp的路径</param>
        /// <param name="userName">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static void Upload(string localFilePath, string ftpUrl, string userName, string password)
        {
            if (!File.Exists(localFilePath))
            {
                throw new FileNotFoundException(string.Empty, localFilePath);
            }

            FileInfo localFileInfo = new FileInfo(localFilePath);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUrl));
            ftpWebRequest.Credentials = new NetworkCredential(userName, password);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpWebRequest.ContentLength = localFileInfo.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen = 0;

            using (FileStream localFileStream = localFileInfo.OpenRead())
            {
                using (Stream requestStream = ftpWebRequest.GetRequestStream())
                {
                    contentLen = localFileStream.Read(buff, 0, buffLength);
                    while (contentLen != 0)
                    {
                        requestStream.Write(buff, 0, contentLen);
                        contentLen = localFileStream.Read(buff, 0, buffLength);
                    }
                }
            }
        }

        /// <summary>
        /// 下载文件[成功true,失败false]
        /// </summary>
        /// <param name="localFilePath">本地文件全路径</param>
        /// <param name="ftpUrl">下载的ftp的路径</param>
        /// <param name="userName">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        /// <returns>成功true,失败false</returns>
        public static void Download(string localFilePath, string ftpUrl, string userName, string password)
        {
            FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUrl));
            ftpWebRequest.Credentials = new NetworkCredential(userName, password);
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
            {
                using (Stream ftpResponseStream = ftpWebResponse.GetResponseStream())
                {
                    long contentLength = ftpWebResponse.ContentLength;
                    int bufferSize = 2048;
                    byte[] buffer = new byte[bufferSize];
                    int readCount = 0;

                    using (FileStream outputStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
                        while (readCount > 0)
                        {
                            outputStream.Write(buffer, 0, readCount);
                            readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 重命名ftp文件
        /// </summary>
        /// <param name="ftpDir">ftp目录</param>
        /// <param name="currentFileName">当前文件名</param>
        /// <param name="newFileName">新文件名</param>
        /// <param name="userName">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static void Rename(string ftpDir, string currentFileName, string newFileName, string userName, string password)
        {
            string ftpUrl = Path.Combine(ftpDir, currentFileName);
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
            request.Credentials = new NetworkCredential(userName, password);
            request.UseBinary = true;
            request.Method = WebRequestMethods.Ftp.Rename;
            request.RenameTo = newFileName;

            using (var response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    //StreamReader reader = new StreamReader(stream);
                    //string retStr = reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 删除ftp服务上指定url的文件
        /// </summary>
        /// <param name="ftpUrl">ftp文件url</param>
        /// <param name="userName">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static void Delete(string ftpUrl, string userName, string password)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
            request.Credentials = new NetworkCredential(userName, password);
            request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            using (var response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    //StreamReader reader = new StreamReader(stream);
                    //string retStr = reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 检测FTP服务是是否存在指定url的文件[存在返回true,不存在返回false],此种方法当FTP的服务器中文件很多时就不可行
        /// </summary>
        /// <param name="ftpUrl">ftp文件url</param>
        /// <param name="userName">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        /// <returns>存在返回true,不存在返回false</returns>
        private static bool Exist_bk(string ftpUrl, string userName, string password)
        {
            string fileName = Path.GetFileName(ftpUrl).ToUpper();
            string ftpUrlDir = ftpUrl.Substring(0, ftpUrl.Length - fileName.Length);
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(ftpUrlDir);
            request.Credentials = new NetworkCredential(userName, password);
            request.KeepAlive = false;
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            using (var response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    string ftpFileName = reader.ReadLine();
                    while (!string.IsNullOrEmpty(ftpFileName))
                    {
                        if (fileName.Equals(ftpFileName.ToUpper()))
                        {
                            return true;
                        }

                        ftpFileName = reader.ReadLine();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 检测FTP服务是是否存在指定url的文件[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="ftpUrl">ftp文件url</param>
        /// <param name="userName">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public static bool Exist(string ftpUrl, string userName, string password)
        {
            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUrl));
                ftpWebRequest.Credentials = new NetworkCredential(userName, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                {
                    using (Stream ftpResponseStream = ftpWebResponse.GetResponseStream())
                    {
                        int bufferSize = 1;
                        byte[] buffer = new byte[bufferSize];
                        int readCount = 0;
                        string dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
                        string localFilePath = Path.Combine(dir, Path.GetFileName(ftpUrl));
                        using (FileStream outputStream = new FileStream(localFilePath, FileMode.Create))
                        {
                            readCount = ftpResponseStream.Read(buffer, 0, bufferSize);
                            if (readCount > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.Success)
                {
                    return false;
                }

                throw;
            }
        }
    }
}
