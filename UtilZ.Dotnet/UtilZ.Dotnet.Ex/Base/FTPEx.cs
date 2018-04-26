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
    public class FTPEx
    {
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
        public FTPEx(string ftpUrl, string userName = null, string password = null, IWebProxy proxy = null)
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

        private List<string> GetFullDir(string relativeDir)
        {
            List<string> dirs = new List<string>(this._rootDirs);
            if (!string.IsNullOrWhiteSpace(relativeDir))
            {
                dirs.AddRange(relativeDir.Split(this._splitDirChs, StringSplitOptions.RemoveEmptyEntries));
            }

            return dirs;
        }

        /// <summary>
        /// 检查FTP上指定目录是否存在
        /// </summary>
        /// <param name="remoteDir">FTP服务器的相对目录</param>
        /// <returns></returns>
        public bool DirectoryExists(string remoteDir)
        {
            var dirs = this.GetFullDir(remoteDir);
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
        public bool CheckDirectoryExists(string ftpUrl, string dir)
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
            var dirs = this.GetFullDir(remoteDir);
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
                string ftpUrl = Path.Combine(this._ftpRootUrl, remoteFilePath);
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

            string ftpUrl = Path.Combine(this._ftpRootUrl, remoteFilePath);
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

            string ftpUrl = Path.Combine(this._ftpRootUrl, remoteFilePath);
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

            string ftpUrl = Path.Combine(this._ftpRootUrl, remoteFilePath);
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

            string ftpUrl = Path.Combine(this._ftpRootUrl, remoteFilePath);
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

            string ftpUrl = Path.Combine(this._ftpRootUrl, oldFileName);
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

            string ftpUrl = Path.Combine(this._ftpRootUrl, remoteFilePath);
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
                throw new DirectoryNotFoundException(string.Format("Ftp上目录:{0}不存在", remoteDir));
            }

            string ftpUrl;
            if (string.IsNullOrWhiteSpace(remoteDir))
            {
                ftpUrl = this._ftpRootUrl;
            }
            else
            {
                ftpUrl = Path.Combine(this._ftpRootUrl, remoteDir);
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
                throw new DirectoryNotFoundException(string.Format("Ftp上目录:{0}不存在", remoteDir));
            }

            string ftpUrl;
            if (string.IsNullOrWhiteSpace(remoteDir))
            {
                ftpUrl = this._ftpRootUrl;
            }
            else
            {
                ftpUrl = Path.Combine(this._ftpRootUrl, remoteDir);
            }

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
                throw new DirectoryNotFoundException(string.Format("Ftp上目录:{0}不存在", remoteDir));
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
                throw new DirectoryNotFoundException(string.Format("Ftp上目录:{0}不存在", remoteDir));
            }

            string ftpUrl = Path.Combine(this._ftpRootUrl, remoteDir);
            FtpWebRequest ftpWebRequest = this.CreateRequest(ftpUrl, WebRequestMethods.Ftp.RemoveDirectory);
            using (var response = ftpWebRequest.GetResponse())
            {

            }
        }
    }

    /// <summary>
    /// 传输文件异常
    /// </summary>
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
}
