using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// ftp辅助类
    /// </summary>
    public class FtpEx
    {
        /// <summary>
        /// init ftp url
        /// </summary>
        private readonly string _ftpUrl;

        /// <summary>
        /// ftp根地址
        /// </summary>
        private readonly string _ftpRootUrl;

        /// <summary>
        /// 根目录层级数组
        /// </summary>
        private readonly string[] _rootDirs;

        /// <summary>
        /// 用户名
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// 密码
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// 代理
        /// </summary>
        private readonly IWebProxy _proxy;

        /// <summary>
        /// 路径拆分字符数组
        /// </summary>
        private readonly char[] _splitDirChs = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        /// <summary>
        /// FTP目录信息匹配正则表达式
        /// </summary>
        private readonly string _ftpDirRegStr = @"(?<pre>^(?<year>\d{2}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}[A,P,a,p]{1}[M,m]{1})\s+<[D,d]{1}[I,i]{1}[R,r]{1}>\s+)";

        /// <summary>
        /// FTP文件信息匹配正则表达式
        /// </summary>
        private readonly string _ftpFileRegStr = @"(?<pre>^(?<year>\d{2}-\d{2}-\d{2})\s+(?<time>\d{2}:\d{2}[A,P,a,p]{1}[M,m]{1})\s+(?<length>\d+)\s+)";

        /// <summary>
        /// 拆分文件或目录列表字符数组
        /// </summary>
        private readonly char[] _splitFileInfoChs = { ' ' };

        /// <summary>
        /// 年前缀
        /// </summary>
        private readonly string _yearPre;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ftpUrl">ftpUrl</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="proxy">代理</param>
        public FtpEx(string ftpUrl, string userName = null, string password = null, IWebProxy proxy = null)
        {
            if (string.IsNullOrWhiteSpace(ftpUrl))
            {
                throw new ArgumentNullException("ftpUrl");
            }

            ftpUrl = ftpUrl.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            Match match = Regex.Match(ftpUrl, RegexConstant.FtpUrl);
            if (!match.Success)
            {
                throw new ArgumentException(string.Format("无效的Ftp地址:{0}", ftpUrl), "ftpUrl");
            }

            string ip = match.Groups["ip"].Value;
            string port = match.Groups["port"].Value;
            string dir = match.Groups["dir"].Value;
            if (string.IsNullOrWhiteSpace(port))
            {
                port = "21";//ftp默认端口号为21
            }

            this._ftpUrl = ftpUrl;
            this._ftpRootUrl = string.Format("ftp://{0}:{1}", ip, port);
            this._rootDirs = dir.Split(this._splitDirChs, StringSplitOptions.RemoveEmptyEntries);
            this._userName = userName;
            this._password = password;
            this._proxy = proxy;
            this._yearPre = (DateTime.Now.Year / 100).ToString();
        }

        /// <summary>
        /// 创建FtpWebRequest
        /// </summary>
        /// <param name="ftpUrl">ftp url</param>
        /// <param name="method">请求方法</param>
        /// <returns>FtpWebRequest</returns>
        private FtpWebRequest CreateRequest(string ftpUrl, string method)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            ftpUrl = ftpUrl.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Credentials = new NetworkCredential(this._userName, this._password);
            request.KeepAlive = false;
            request.UsePassive = false;
            request.UseBinary = true;
            request.Proxy = this._proxy;
            request.Method = method;
            return request;
        }

        /// <summary>
        /// 获取全目录层级名称列表
        /// </summary>
        /// <param name="relativeDir">相对目录</param>
        /// <returns>全目录层级名称列表</returns>
        private List<string> GetFullDirFolderNames(string relativeDir)
        {
            List<string> dirs = new List<string>(this._rootDirs);
            if (!string.IsNullOrWhiteSpace(relativeDir))
            {
                dirs.AddRange(relativeDir.Split(this._splitDirChs, StringSplitOptions.RemoveEmptyEntries));
            }

            return dirs;
        }

        /// <summary>
        /// 获取全路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>全路径</returns>
        private string GetFullPath(string relativePath)
        {
            char ch;
            int index = 0;

            //截取头
            for (int i = 0; i < relativePath.Length; i++)
            {
                ch = relativePath[i];
                if (ch != Path.DirectorySeparatorChar && ch != Path.AltDirectorySeparatorChar)
                {
                    index = i;
                    break;
                }
            }

            if (index > 0)
            {
                relativePath = relativePath.Substring(index);
            }

            //截取尾
            int length = 0;
            for (int i = relativePath.Length - 1; i > -1; i--)
            {
                ch = relativePath[i];
                if (ch != Path.DirectorySeparatorChar && ch != Path.AltDirectorySeparatorChar)
                {
                    length = relativePath.Length - 1 - i;
                    break;
                }
            }

            if (length > 0)
            {
                relativePath = relativePath.Substring(0, length);
            }

            return Path.Combine(this._ftpUrl, relativePath);
        }

        /// <summary>
        /// 检查FTP上指定目录是否存在
        /// </summary>
        /// <param name="remoteDir">FTP服务器的相对目录</param>
        /// <returns></returns>
        public bool DirectoryExists(string remoteDir)
        {
            var dirs = this.GetFullDirFolderNames(remoteDir);
            string ftpUrl = this._ftpRootUrl;
            string cuurentDir;
            for (int i = 0; i < dirs.Count; i++)
            {
                cuurentDir = dirs[i];
                if (this.CheckDirectoryExists(ftpUrl, cuurentDir))//检查目录不存在则创建
                {
                    ftpUrl = Path.Combine(ftpUrl, cuurentDir);
                }
                else
                {
                    //某一级目录不存在
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检测FTP服务器URL指定的目录下是否存在指定目录[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="ftpUrl">ftp目录</param>
        /// <param name="dir">目录名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        private bool CheckDirectoryExists(string ftpUrl, string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                return false;
            }

            Match match;
            string dirNanme;
            FtpWebRequest ftp = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.ListDirectoryDetails);
            using (Stream stream = ftp.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                string line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    match = Regex.Match(line, _ftpDirRegStr);
                    if (match.Success)
                    {
                        dirNanme = line.Substring(match.Groups["pre"].Value.Length).Trim();
                        if (string.Equals(dirNanme, dir, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    line = sr.ReadLine();
                }
            }

            return false;
        }

        /// <summary>
        /// 创建目录[可多级]
        /// </summary>
        /// <param name="remoteDir">FTP服务器的多级相对目录</param>
        /// <returns>目录有创建并返回true,无创建返回false</returns>
        public bool CreateDirectory(string remoteDir)
        {
            if (string.IsNullOrWhiteSpace(remoteDir))
            {
                throw new ArgumentNullException("目录不能为空", "remoteDir");
            }

            bool createResult = false;
            var dirs = this.GetFullDirFolderNames(remoteDir);
            string ftpUrl = this._ftpRootUrl;
            foreach (var dir in dirs)
            {
                if (this.CheckDirectoryExists(ftpUrl, dir))
                {
                    ftpUrl = Path.Combine(ftpUrl, dir);
                }
                else
                {
                    //检查目录不存在则创建
                    ftpUrl = Path.Combine(ftpUrl, dir);
                    FtpWebRequest ftp = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.MakeDirectory);
                    using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
                    {
                        createResult = true;
                    }
                }
            }

            return createResult;
        }

        /// <summary>
        /// 检测FTP服务是是否存在指定url的文件[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="remoteFilePath">FTP上文件相对路径</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public bool FileExists(string remoteFilePath)
        {
            try
            {
                string ftpUrl = this.GetFullPath(remoteFilePath);
                FtpWebRequest ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.GetDateTimestamp);
                using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                {
                    //return ftpWebResponse.ContentLength > 0;
                    return true;
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.ProtocolError)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        /// <param name="remoteFilePath">FTP服务器的相对路径</param>
        /// <param name="buffLength">单次传输数据大小,默认2048</param>
        /// <param name="isAppend">是否追加文件[true:断点续传;false:存在覆盖,不存在则新建;默认为false]</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前已传输大小)</param>
        public void UploadFile(string localFilePath, string remoteFilePath, int buffLength = 2048, bool isAppend = false, Action<long, long> updateProgress = null)
        {
            if (!File.Exists(localFilePath))
            {
                throw new FileNotFoundException(string.Empty, localFilePath);
            }

            //创建目录
            string remoteDir = Path.GetDirectoryName(remoteFilePath);
            bool createResult = this.CreateDirectory(remoteDir);
            string ftpUrl = this.GetFullPath(remoteFilePath);
            long sendedLength;
            FtpWebRequest ftpWebRequest;
            if (isAppend && !createResult)
            {
                //检查文件是否存在
                if (this.FileExists(remoteFilePath))
                {
                    //查询已上传大小
                    ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.GetFileSize);
                    using (FtpWebResponse re = (FtpWebResponse)ftpWebRequest.GetResponse())
                    {
                        sendedLength = re.ContentLength;
                    }

                    ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.AppendFile);
                }
                else
                {
                    sendedLength = 0;
                    ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.UploadFile);
                }
            }
            else
            {
                ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.UploadFile);
                sendedLength = 0;
            }

            //上传
            byte[] buff = new byte[buffLength];
            int contentLen;
            try
            {
                var handler = updateProgress;
                using (FileStream localFileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (!isAppend)
                    {
                        localFileStream.Seek(sendedLength, SeekOrigin.Begin);
                    }

                    ftpWebRequest.ContentLength = localFileStream.Length - sendedLength;
                    using (Stream requestStream = ftpWebRequest.GetRequestStream())
                    {
                        contentLen = localFileStream.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            requestStream.Write(buff, 0, contentLen);
                            sendedLength += contentLen;
                            if (handler != null)
                            {
                                handler(localFileStream.Length, sendedLength);
                            }

                            contentLen = localFileStream.Read(buff, 0, buffLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TransferFileException(sendedLength, "上传文件失败", ex);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="remoteFilePath">FTP服务器的相对路径</param>
        /// <param name="contentLength">要上传的数据大小</param>
        /// <param name="getData">获取数据回调[返回null长度为为的数组为上传结束]</param>
        /// <param name="isAppend">是否追加文件[true:断点续传;false:存在覆盖,不存在则新建;默认为false]</param>
        public void UploadFile(string remoteFilePath, long contentLength, Func<byte[]> getData, bool isAppend = false)
        {
            if (getData == null)
            {
                throw new ArgumentNullException("getData");
            }

            //创建目录
            string remoteDir = Path.GetDirectoryName(remoteFilePath);
            bool createResult = this.CreateDirectory(remoteDir);

            string ftpUrl = this.GetFullPath(remoteFilePath);
            long sendedLength;
            FtpWebRequest ftpWebRequest;
            if (isAppend && !createResult)
            {
                //检查文件是否存在
                if (this.FileExists(remoteFilePath))
                {
                    //查询已上传大小
                    ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.GetFileSize);
                    using (FtpWebResponse re = (FtpWebResponse)ftpWebRequest.GetResponse())
                    {
                        sendedLength = re.ContentLength;
                    }

                    ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.AppendFile);
                }
                else
                {
                    sendedLength = 0;
                    ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.UploadFile);
                }
            }
            else
            {
                ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.UploadFile);
                sendedLength = 0;
            }

            //上传
            byte[] buff;
            try
            {
                buff = getData();
                if (buff == null || buff.Length == 0)
                {
                    return;
                }

                ftpWebRequest.ContentLength = contentLength;
                using (Stream requestStream = ftpWebRequest.GetRequestStream())
                {
                    while (buff != null && buff.Length > 0)
                    {
                        requestStream.Write(buff, 0, buff.Length);
                        sendedLength += buff.Length;
                        buff = getData();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TransferFileException(sendedLength, "上传文件失败", ex);
            }
        }

        /// <summary>
        /// 获取指定文件大小
        /// </summary>
        /// <param name="remoteFilePath">FTP上文件相对路径</param>
        /// <returns>指定文件大小</returns>
        public long GetFileLength(string remoteFilePath)
        {
            if (!this.FileExists(remoteFilePath))
            {
                throw new FileNotFoundException("Ftp上文件不存在", remoteFilePath);
            }

            string ftpUrl = this.GetFullPath(remoteFilePath);
            FtpWebRequest ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.GetFileSize);
            using (FtpWebResponse re = (FtpWebResponse)ftpWebRequest.GetResponse())
            {
                return re.ContentLength;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        /// <param name="remoteFilePath">FTP服务器的相对路径</param>
        /// <param name="buffLength">单次传输数据大小,默认2048</param>
        /// <param name="isAppend">是否断点续传[true:断点续传;false:存在覆盖,不存在则新建;默认为false]</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前已传输大小)</param>
        public void DownloadFile(string localFilePath, string remoteFilePath, int buffLength = 2048, bool isAppend = false, Action<long, long> updateProgress = null)
        {
            if (!this.FileExists(remoteFilePath))
            {
                throw new FileNotFoundException("Ftp上文件不存在", remoteFilePath);
            }

            string dir = Path.GetDirectoryName(localFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string ftpUrl = this.GetFullPath(remoteFilePath);
            long totallLength;
            FtpWebRequest ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.GetFileSize);
            using (FtpWebResponse re = (FtpWebResponse)ftpWebRequest.GetResponse())
            {
                totallLength = re.ContentLength;
            }

            ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.DownloadFile);
            long transferLength = 0;

            try
            {
                var handler = updateProgress;
                using (FileStream outputStream = new FileStream(localFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    if (isAppend && outputStream.Length > 0)
                    {
                        ftpWebRequest.ContentOffset = outputStream.Length;
                    }

                    using (FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse())
                    {
                        using (Stream ftpResponseStream = ftpWebResponse.GetResponseStream())
                        {
                            byte[] buffer = new byte[buffLength];
                            int readCount = 0;

                            readCount = ftpResponseStream.Read(buffer, 0, buffLength);
                            while (readCount > 0)
                            {
                                outputStream.Write(buffer, 0, readCount);
                                transferLength += readCount;
                                if (handler != null)
                                {
                                    handler(totallLength, transferLength);
                                }

                                readCount = ftpResponseStream.Read(buffer, 0, buffLength);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TransferFileException(transferLength, "下载文件失败", ex);
            }
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldFileName">当前文件名</param>
        /// <param name="newFileName">新文件名</param>
        public void Rename(string oldFileName, string newFileName)
        {
            if (!this.FileExists(oldFileName))
            {
                throw new FileNotFoundException("Ftp上文件不存在", oldFileName);
            }

            string ftpUrl = this.GetFullPath(oldFileName);
            FtpWebRequest request = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.Rename);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.RenameTo = newFileName;
            using (var response = request.GetResponse())
            {
            }
        }

        /// <summary>
        /// 删除FTP上的文件
        /// </summary>
        /// <param name="remoteFilePath">文件相对路径</param>
        public void DeleteFile(string remoteFilePath)
        {
            if (!this.FileExists(remoteFilePath))
            {
                return;
            }

            string ftpUrl = this.GetFullPath(remoteFilePath);
            FtpWebRequest ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.DeleteFile);
            using (var response = ftpWebRequest.GetResponse())
            {

            }
        }

        /// <summary>
        /// 转换FTP创建时间
        /// </summary>
        /// <param name="year">年月日</param>
        /// <param name="time">时间</param>
        /// <returns>FTP创建时间</returns>
        private DateTime ConvertFTPCreateTime(string year, string time)
        {
            /************************
             * 月-日-年 时-分
             * 10-22-17 10:53AM
             * 02-03-18 03:03PM 
             ************************/
            char[] splitYearChs = { '-' };
            string[] yearStrs = year.Split(splitYearChs, StringSplitOptions.RemoveEmptyEntries);
            var dateTimeStr = string.Format("{0}{1}-{2}-{3} {4}", this._yearPre, yearStrs[2], yearStrs[0], yearStrs[1], time);
            var createTime = DateTime.Parse(dateTimeStr);
            return createTime;
        }

        /// <summary>
        /// 获取指定目录下的目录列表
        /// </summary>
        /// <param name="remoteDir">FTP上的相对目标目录</param>
        /// <returns>目录列表</returns>
        public List<FTPDirectoryInfo> GetDirectoryList(string remoteDir)
        {
            if (!this.DirectoryExists(remoteDir))
            {
                return new List<FTPDirectoryInfo>();
            }

            string ftpUrl;
            if (string.IsNullOrWhiteSpace(remoteDir))
            {
                ftpUrl = this._ftpUrl;
            }
            else
            {
                ftpUrl = this.GetFullPath(remoteDir);
            }

            var dirList = new List<FTPDirectoryInfo>();
            DateTime createTime;
            string name;
            FtpWebRequest ftp = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.ListDirectoryDetails);

            Match match;
            using (Stream stream = ftp.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                string line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    match = Regex.Match(line, _ftpDirRegStr);
                    if (match.Success)
                    {
                        createTime = this.ConvertFTPCreateTime(match.Groups["year"].Value, match.Groups["time"].Value);
                        name = line.Substring(match.Groups["pre"].Value.Length).Trim();
                        dirList.Add(new FTPDirectoryInfo(createTime, name));
                    }

                    line = sr.ReadLine();
                }
            }

            return dirList;
        }

        /// <summary>
        /// 获取指定目录下的文件列表
        /// </summary>
        /// <param name="remoteDir">FTP上的相对目标目录</param>
        /// <returns>文件列表</returns>
        public List<FTPFileInfo> GetFileList(string remoteDir)
        {
            if (!this.DirectoryExists(remoteDir))
            {
                return new List<FTPFileInfo>();
            }

            string ftpUrl = this.GetFullPath(remoteDir);
            var fileList = new List<FTPFileInfo>();
            DateTime createTime;
            long length;
            FtpWebRequest ftp = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.ListDirectoryDetails);
            Match match;
            using (Stream stream = ftp.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                string line = sr.ReadLine();
                string name;
                while (!string.IsNullOrEmpty(line))
                {
                    match = Regex.Match(line, this._ftpFileRegStr);
                    if (match.Success)
                    {
                        createTime = this.ConvertFTPCreateTime(match.Groups["year"].Value, match.Groups["time"].Value);
                        length = long.Parse(match.Groups["length"].Value);
                        name = line.Substring(match.Groups["pre"].Value.Length).Trim();
                        fileList.Add(new FTPFileInfo(createTime, name, length));
                    }

                    line = sr.ReadLine();
                }
            }

            return fileList;
        }

        /// <summary>
        /// 上传本地目录及其子目录内的所有文件到FTP,并保持结构一致
        /// </summary>
        /// <param name="localDir">本地要上传的目录</param>
        /// <param name="remoteDir">FTP上的相对目标目录</param>
        public void UploadDirectory(string localDir, string remoteDir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(localDir);
            int localDirLength = directoryInfo.FullName.Length + 1;
            string remoteFilePath;
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

            //空目录无视,有文件的目录在上传文件时被创建
            foreach (var fileInfo in fileInfos)
            {
                remoteFilePath = Path.Combine(remoteDir, fileInfo.FullName.Substring(localDirLength, fileInfo.FullName.Length - localDirLength));
                this.UploadFile(fileInfo.FullName, remoteFilePath);
            }
        }

        /// <summary>
        /// 下载FTP服务器上指定目录及其子目录内的所有文件,并按原结构存放本地
        /// </summary>
        /// <param name="localDir">本地目录</param>
        /// <param name="remoteDir">要下载的相对目标目录</param>
        public void DownloadDirectory(string localDir, string remoteDir)
        {
            if (string.IsNullOrWhiteSpace(remoteDir))
            {
                return;
            }

            if (!this.DirectoryExists(remoteDir))
            {
                return;
            }

            string localFilePath, remoteFilePath;
            List<FTPFileInfo> fileList = this.GetFileList(remoteDir);
            foreach (var ftpFile in fileList)
            {
                //下载文件
                localFilePath = Path.Combine(localDir, ftpFile.Name);
                remoteFilePath = Path.Combine(remoteDir, ftpFile.Name);
                this.DownloadFile(localFilePath, remoteFilePath);
            }

            //下载子目录
            List<FTPDirectoryInfo> subDirLis = this.GetDirectoryList(remoteDir);
            foreach (var subDir in subDirLis)
            {
                this.DownloadDirectory(Path.Combine(localDir, subDir.Name), Path.Combine(remoteDir, subDir.Name));
            }
        }

        /// <summary>
        /// 删除FTP服务器上的目录
        /// </summary>
        /// <param name="remoteDir">FTP服务器上的相对目录</param>
        public void DeleteDirectory(string remoteDir)
        {
            if (!this.DirectoryExists(remoteDir))
            {
                return;
            }

            //删除当前子目录
            string ftpUrl = this.GetFullPath(remoteDir);
            FtpWebRequest ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.RemoveDirectory);
            using (var response = ftpWebRequest.GetResponse())
            {

            }
        }

        /// <summary>
        /// 删除FTP服务器上的目录
        /// </summary>
        /// <param name="remoteDir">FTP服务器上的相对目录</param>
        /// <param name="recursive">若要移除 path 中的目录、子目录和文件，则为 true；否则为 false</param>
        public void DeleteDirectory(string remoteDir, bool recursive)
        {
            if (recursive)
            {
                this.DeleteSubDirectoryAndFile(remoteDir);
            }
            else
            {
                this.DeleteDirectory(remoteDir);
            }
        }

        private void DeleteSubDirectoryAndFile(string remoteDir)
        {
            var ftpFileInfos = this.GetFileList(remoteDir);
            foreach (var ftpFileInfo in ftpFileInfos)
            {
                //删除当前目录中的文件
                this.DeleteFile(Path.Combine(remoteDir, ftpFileInfo.Name));
            }

            var ftpDirInfos = this.GetDirectoryList(remoteDir);
            foreach (var ftpDirInfo in ftpDirInfos)
            {
                string remoteDirSub = Path.Combine(remoteDir, ftpDirInfo.Name);

                //删除子目录中的下级子目录及文件
                this.DeleteSubDirectoryAndFile(remoteDirSub);

                //删除子目录
                this.DeleteDirectory(remoteDirSub);
            }

            this.DeleteDirectory(remoteDir);
        }
    }

    /// <summary>
    /// 传输文件异常
    /// </summary>
    [Serializable]
    public class TransferFileException : Exception
    {
        /// <summary>
        /// 已传输文件大小
        /// </summary>
        public long TransferLength { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transferLength">已传输文件大小</param>
        /// <param name="message">信息</param>
        /// <param name="ex">内部异常</param>
        public TransferFileException(long transferLength, string message, Exception ex) : base(message, ex)
        {
            this.TransferLength = transferLength;
        }
    }

    /// <summary>
    /// FTP服务器的文件信息
    /// </summary>
    [Serializable]
    public class FTPFileInfo : FTPFileSystem
    {
        /// <summary>
        /// 文件长度
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="createTime">创建时间</param>
        /// <param name="name">名称</param>
        /// <param name="length">文件长度</param>
        public FTPFileInfo(DateTime createTime, string name, long length) : base(createTime, name)
        {
            this.Length = length;
        }
    }

    /// <summary>
    /// FTP服务器的目录信息
    /// </summary>
    [Serializable]
    public class FTPDirectoryInfo : FTPFileSystem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="createTime">创建时间</param>
        /// <param name="name">名称</param>
        public FTPDirectoryInfo(DateTime createTime, string name) : base(createTime, name)
        {

        }
    }

    /// <summary>
    /// ftp服务器文件信息
    /// </summary>
    [Serializable]
    public abstract class FTPFileSystem
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="createTime">创建时间</param>
        /// <param name="name">名称</param>
        public FTPFileSystem(DateTime createTime, string name)
        {
            this.CreateTime = createTime;
            this.Name = name;
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }

    /// <summary>
    /// Ftp错误码定义类
    /// </summary>
    public class FtpErrorCode
    {
        /// <summary>
        /// 同时连接该ftp的人数过多，一般ftp网站都有同时登陆人数的上限，超过该上限就会出现421错误
        /// 解决办法：在ftp软件中，把重试次数改为999，重试间隔改为60秒，一般几分钟到半小时就会连上，要注意的是，有些网站有连接时间的设定，连上后，超过一定时间不下载，就会自动断开，所以要经常去看看有没有连上
        /// </summary>
        public const int ConnectionMore = 401;

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        public const int AccountInfoError = 530;

        /// <summary>
        /// Url错误或没权限
        /// </summary>
        public const int UrlErrorOrNoAuthority = 550;
        /*
        1xx – 肯定的初步答复
这些状态代码指示一项* 作已经成功开始，但客户端希望在继续* 作新命令前得到另一个答复。 
● 110 重新启动标记答复。 
● 120 服务已就绪，在 nnn 分钟后开始。 
● 125 数据连接已打开，正在开始传输。 
● 150 文件状态正常，准备打开数据连接。


2xx – 肯定的完成答复
一项* 作已经成功完成。客户端可以执行新命令。 
● 200 命令确定。 
● 202 未执行命令，站点上的命令过多。 
● 211 系统状态，或系统帮助答复。 
● 212 目录状态。 
● 213 文件状态。 
● 214 帮助消息。 
● 215 NAME 系统类型，其中，NAME 是 Assigned Numbers 文档中所列的正式系统名称。 
● 220 服务就绪，可以执行新用户的请求。 
● 221 服务关闭控制连接。如果适当，请注销。 
● 225 数据连接打开，没有进行中的传输。 
● 226 关闭数据连接。请求的文件* 作已成功（例如，传输文件或放弃文件）。 
● 227 进入被动模式(h1, h2, h3, h4, p1, p2)。 
● 230 用户已登录，继续进行。 
● 250 请求的文件* 作正确，已完成。 
● 257 已创建“PATHNAME”。

3xx – 肯定的中间答复
该命令已成功，但服务器需要更多来自客户端的信息以完成对请求的处理。 
● 331 用户名正确，需要密码。 
● 332 需要登录帐户。 
● 350 请求的文件* 作正在等待进一步的信息。

4xx – 瞬态否定的完成答复
该命令不成功，但错误是暂时的。如果客户端重试命令，可能会执行成功。 
● 421 服务不可用，正在关闭控制连接。如果服务确定它必须关闭，将向任何命令发送这一应答。 
● 425 无法打开数据连接。 
● 426 Connection closed; transfer aborted.
● 450 未执行请求的文件* 作。文件不可用（例如，文件繁忙）。 
● 451 请求的* 作异常终止：正在处理本地错误。 
● 452 未执行请求的* 作。系统存储空间不够。

5xx – 永久性否定的完成答复
该命令不成功，错误是永久性的。如果客户端重试命令，将再次出现同样的错误。 
● 500 语法错误，命令无法识别。这可能包括诸如命令行太长之类的错误。 
● 501 在参数中有语法错误。 
● 502 未执行命令。 
● 503 错误的命令序列。 
● 504 未执行该参数的命令。 
● 530 未登录。 
● 532 存储文件需要帐户。 
● 550 未执行请求的* 作。文件不可用（例如，未找到文件，没有访问权限）。 
● 551 请求的* 作异常终止：未知的页面类型。 
● 552 请求的文件* 作异常终止：超出存储分配（对于当前目录或数据集）。 
● 553 未执行请求的* 作。不允许的文件名。

常见的 FTP 状态代码及其原因
● 150 – FTP 使用两个端口：21 用于发送命令，20 用于发送数据。状态代码 150 表示服务器准备在端口 20 上打开新连接，发送一些数据。 
● 226 – 命令在端口 20 上打开数据连接以执行* 作，如传输文件。该* 作成功完成，数据连接已关闭。 
● 230 – 客户端发送正确的密码后，显示该状态代码。它表示用户已成功登录。 
● 331 – 客户端发送用户名后，显示该状态代码。无论所提供的用户名是否为系统中的有效帐户，都将显示该状态代码。 
● 426 – 命令打开数据连接以执行* 作，但该* 作已被取消，数据连接已关闭。 
● 530 – 该状态代码表示用户无法登录，因为用户名和密码组合无效。如果使用某个用户帐户登录，可能键入错误的用户名或密码，也可能选择只允许匿名访问。如果使用匿名帐户登录，IIS 的配置可能拒绝匿名访问。 
● 550 – 命令未被执行，因为指定的文件不可用。例如，要 GET 的文件并不存在，或试图将文件 PUT 到您没有写入权限的目录。
          */
    }
}
