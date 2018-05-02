using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilZ.Dotnet.Ex.Base;

namespace TestE.Common
{
    public partial class FTestFTP : Form
    {
        private readonly FTPEx _ftp;
        public FTestFTP()
        {
            InitializeComponent();

            string ftpUrl = @"ftp://192.168.0.103/";
            string ftpUserName = string.Empty;
            string ftpPassword = string.Empty;
            this._ftp = new FTPEx(ftpUrl, ftpUserName, ftpPassword);
        }

        private void FTestFTP_Load(object sender, EventArgs e)
        {

        }

        private void btnDirExists_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = @"q1";
                bool ret = this._ftp.DirectoryExists(dir);

                dir = @"/q1";
                bool ret2 = this._ftp.DirectoryExists(dir);

                dir = @"q1/";
                bool ret3 = this._ftp.DirectoryExists(dir);

                dir = @"\q1";
                bool ret4 = this._ftp.DirectoryExists(dir);

                dir = @"\q1\";
                bool ret5 = this._ftp.DirectoryExists(dir);

                dir = @"\q1/";
                bool ret6 = this._ftp.DirectoryExists(dir);

                dir = @"/q1\";
                bool ret7 = this._ftp.DirectoryExists(dir);

                dir = @"\q1/a";
                bool ret8 = this._ftp.DirectoryExists(dir);

                dir = @"\q1/a/";
                bool ret9 = this._ftp.DirectoryExists(dir);

                dir = @"\q1/a\";
                bool ret10 = this._ftp.DirectoryExists(dir);

                dir = @"q1/a/";
                bool ret11 = this._ftp.DirectoryExists(dir);

                dir = @"q1/a\";
                bool ret12 = this._ftp.DirectoryExists(dir);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnCreateDir_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = @"b1";
                bool ret1 = this._ftp.CreateDirectory(dir);

                dir = @"\b2";
                bool ret2 = this._ftp.CreateDirectory(dir);

                dir = @"b3\";
                bool ret3 = this._ftp.CreateDirectory(dir);

                dir = @"\b4\";
                bool ret4 = this._ftp.CreateDirectory(dir);

                dir = @"/b5";
                bool ret5 = this._ftp.CreateDirectory(dir);

                dir = @"b6/";
                bool ret6 = this._ftp.CreateDirectory(dir);

                dir = @"/b7";
                bool ret7 = this._ftp.CreateDirectory(dir);



                dir = @"b8\c";
                bool ret8 = this._ftp.CreateDirectory(dir);

                dir = @"b9/c";
                bool ret9 = this._ftp.CreateDirectory(dir);


                dir = @"\b10\c";
                bool ret10 = this._ftp.CreateDirectory(dir);

                dir = @"/b11\c";
                bool ret11 = this._ftp.CreateDirectory(dir);



                dir = @"\b12\c/";
                bool ret12 = this._ftp.CreateDirectory(dir);

                dir = @"/b13\c\";
                bool ret13 = this._ftp.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnFileExist_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteFilePath = @"PowerMode.vsix";
                bool ret = this._ftp.FileExists(remoteFilePath);

                remoteFilePath = @"abc.msi";
                bool ret2 = this._ftp.FileExists(remoteFilePath);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnFileLength_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteFilePath = @"PowerMode.vsix";
                long length = this._ftp.GetFileLength(remoteFilePath);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            try
            {
                string localFilePath, remoteFilePath;

                /*localFilePath = @"F:\Soft\KKSetup_2000.exe";
                remoteFilePath = @"KKSetup_2000.exe";
                this._extendFTP.UploadFile(localFilePath, remoteFilePath, 2048, true);

                localFilePath = @"F:\Soft\dotNetFx40_Full_x86_x64.exe";
                remoteFilePath = @"Soft\dotNetFx40_Full_x86_x64.exe";
                this._extendFTP.UploadFile(localFilePath, remoteFilePath, 2048, true);



                localFilePath = @"F:\Soft\feiq.rar";
                remoteFilePath = @"feiq.rar";
                this._extendFTP.UploadFile(localFilePath, remoteFilePath, 2048, true);*/

               
                remoteFilePath = @"12/刀剑心.mp3";
                localFilePath = @"F:\刀剑心.mp3";
                using (var fs = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int bufferSize = 1024;
                    int offset = 0;
                    byte[] buffer = new byte[bufferSize];
                    this._ftp.UploadFile(remoteFilePath, fs.Length, () =>
                    {
                        if (offset == fs.Length)
                        {
                            return null;
                        }

                        if (offset + bufferSize > fs.Length)
                        {
                            bufferSize = (int)(fs.Length - offset);
                            buffer = new byte[bufferSize];
                        }

                        fs.Read(buffer, 0, bufferSize);
                        offset += bufferSize;
                        return buffer;
                    }, true);
                }
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            try
            {
                string localFilePath = @"G:\Tmp\test\dotNetFx40_Full_x86_x64.exe";
                string remoteFilePath = @"Soft\dotNetFx40_Full_x86_x64.exe";
                this._ftp.DownloadFile(localFilePath, remoteFilePath, 2048, true);

                localFilePath = @"G:\Tmp\test\feiq.rar";
                remoteFilePath = @"feiq.rar";
                this._ftp.DownloadFile(localFilePath, remoteFilePath, 2048, false);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            string oldFileName = "流程图.vsd";
            string newFileName = "FlowImg.vsd";
            try
            {
                this._ftp.Rename(oldFileName, newFileName);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteFilePath = @"feiq.rar";
                this._ftp.DeleteFile(remoteFilePath);

                remoteFilePath = @"Soft/dotNetFx40_Full_x86_x64.exe";
                this._ftp.DeleteFile(remoteFilePath);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnDirList_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteDir = null;
                var dirList = this._ftp.GetDirectoryList(remoteDir);

                remoteDir = @"a";
                var dirList2 = this._ftp.GetDirectoryList(remoteDir);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnFileList_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteDir = null;
                var dirList = this._ftp.GetFileList(remoteDir);

                remoteDir = @"a";
                var dirList2 = this._ftp.GetFileList(remoteDir);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }

        private void btnUploadDir_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                string localDir = @"G:\Tmp_Study\2014-11-4";
                try
                {
                    string remoteDir = @"2014-11-4";
                    this._ftp.UploadDirectory(localDir, remoteDir);
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        rtxtMsg.AppendText(ex.Message);
                        rtxtMsg.AppendText(Environment.NewLine);
                    }));
                }
            });
        }

        private void btnDownloadDir_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                string localDir = @"G:\Tmp\test\2014-11-4";
                try
                {
                    string remoteDir = @"a\2014-11-4";
                    this._ftp.DownloadDirectory(localDir, remoteDir);
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        rtxtMsg.AppendText(ex.Message);
                        rtxtMsg.AppendText(Environment.NewLine);
                    }));
                }
            });
        }

        private void btnDeleteDir_Click(object sender, EventArgs e)
        {
            try
            {
                string remoteDir = @"Soft";
                this._ftp.DeleteDirectory(remoteDir);
            }
            catch (Exception ex)
            {
                rtxtMsg.AppendText(ex.Message);
                rtxtMsg.AppendText(Environment.NewLine);
            }
        }
    }
}
