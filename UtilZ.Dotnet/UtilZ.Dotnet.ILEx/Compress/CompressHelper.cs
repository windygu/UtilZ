using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.ILEx.Compress
{
    /// <summary>
    /// 压缩和解压文件辅助类
    /// </summary>
    public class CompressHelper : CompressEx
    {
        /// <summary>
        /// 压缩单个文件到zip文件
        /// </summary>
        /// <param name="filePath">待压缩的文件</param>
        /// <param name="compressFilePath">压缩文件保存路径</param>
        /// <param name="compressionLevel">压缩程度，范围0-9，数值越大，压缩程序越高[默认为5]</param>
        /// <param name="blockSize">分块大小[默认为1024]</param>
        public static void CompressFileToZip(string filePath, string compressFilePath, int compressionLevel = 5, int blockSize = 10204)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!System.IO.File.Exists(filePath))//如果文件没有找到，则报错
            {
                throw new FileNotFoundException("目标文件不存在", filePath);
            }

            if (string.IsNullOrWhiteSpace(compressFilePath))
            {
                throw new ArgumentNullException(nameof(compressFilePath));
            }

            if (compressionLevel < 0 || compressionLevel > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(compressionLevel));
            }

            if (blockSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(blockSize));
            }

            using (FileStream streamToZip = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fs = File.Create(compressFilePath))
                {
                    ZipOutputStream zipStream = new ZipOutputStream(fs);
                    ZipEntry zipEntry = new ZipEntry(filePath);
                    zipStream.PutNextEntry(zipEntry);
                    zipStream.SetLevel(compressionLevel);
                    byte[] buffer = new byte[blockSize];
                    int size = streamToZip.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, size);
                    try
                    {
                        while (size < streamToZip.Length)
                        {
                            int sizeRead = streamToZip.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sizeRead);
                            size += sizeRead;
                        }
                    }
                    catch (Exception)
                    {
                        GC.Collect();
                        throw;
                    }
                    zipStream.Finish();
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// 压缩目录到zip文件（包括子目录及所有文件）
        /// </summary>
        /// <param name="directory">待压缩的目录</param>
        /// <param name="compressFilePath">压缩文件保存路径</param>
        /// <param name="compressLevel">压缩程度，范围0-9，数值越大，压缩程序越高[默认为5]</param>
        public static void CompressDirectoryToZip(string directory, string compressFilePath, int compressLevel = 5)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                throw new FileNotFoundException("目标目录不存在", directory);
            }

            if (string.IsNullOrWhiteSpace(compressFilePath))
            {
                throw new ArgumentNullException(nameof(compressFilePath));
            }

            if (compressionLevel < 0 || compressionLevel > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(compressionLevel));
            }

            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            string rootMark = directory + "\\";//得到当前路径的位置，以备压缩时将所压缩内容转变成相对路径。
            Crc32 crc = new Crc32();

            using (ZipOutputStream outPutStream = new ZipOutputStream(File.Create(compressFilePath)))
            {
                outPutStream.SetLevel(compressLevel); // 0 - store only to 9 - means best compression
                foreach (string file in files)
                {
                    //打开压缩文件
                    using (FileStream fileStream = File.OpenRead(file))
                    {
                        byte[] buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, buffer.Length);
                        ZipEntry entry = new ZipEntry(file.Replace(rootMark, string.Empty));
                        entry.DateTime = DateTime.Now;
                        // set Size and the crc, because the information
                        // about the size and crc should be stored in the header
                        // if it is not set it is automatically written in the footer.
                        // (in this case size == crc == -1 in the header)
                        // Some ZIP programs have problems with zip files that don't store
                        // the size and crc in the header.
                        entry.Size = fileStream.Length;

                        crc.Reset();
                        crc.Update(buffer);
                        entry.Crc = crc.Value;
                        outPutStream.PutNextEntry(entry);
                        outPutStream.Write(buffer, 0, buffer.Length);
                    }
                }

                outPutStream.Finish();
            }
            GC.Collect();
        }

        /// <summary>
        /// 解zip压缩文件(压缩文件中含有子目录)
        /// </summary>
        /// <param name="compressFilePath">待解压缩的文件路径</param>
        /// <param name="deCompressDirectory">解压缩目录</param>
        /// <returns>解压后的文件列表</returns>
        public static void DeCompressZip(string compressFilePath, string deCompressDirectory)
        {
            if (string.IsNullOrWhiteSpace(compressFilePath))
            {
                throw new ArgumentNullException(nameof(compressFilePath));
            }

            if (!File.Exists(compressFilePath))
            {
                throw new FileNotFoundException("目标压缩文件不存在", compressFilePath);
            }

            if (string.IsNullOrWhiteSpace(deCompressDirectory))
            {
                throw new ArgumentNullException(nameof(deCompressDirectory));
            }

            //检查输出目录是否以“\\”结尾
            if (deCompressDirectory.EndsWith("\\") == false || deCompressDirectory.EndsWith(":\\") == false)
            {
                deCompressDirectory += "\\";
            }

            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(compressFilePath)))
            {
                ZipEntry theEntry = null;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(deCompressDirectory);
                    string fileName = Path.GetFileName(theEntry.Name);

                    //生成解压目录【用户解压到硬盘根目录时，不需要创建】
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入,但是实际上是有需要的
                    //if (theEntry.CompressedSize == 0)
                    //{
                    //    break;
                    //}

                    //解压文件到指定的目录
                    directoryName = Path.GetDirectoryName(deCompressDirectory + theEntry.Name);
                    //建立下面的目录和子目录
                    Directory.CreateDirectory(directoryName);

                    using (FileStream streamWriter = File.Create(deCompressDirectory + theEntry.Name))
                    {
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// 解zip压缩文件中的指定文件
        /// </summary>
        /// <param name="compressFilePath">待解压缩的文件路径</param>
        /// <param name="filePaths">压缩包中要解压的相对文件路径路径</param>
        /// <param name="deCompressDirectory">解压缩目录</param>
        /// <returns>解压后的文件列表</returns>
        public static void DeCompressZip(string compressFilePath, IEnumerable<string> filePaths, string deCompressDirectory)
        {
            if (string.IsNullOrWhiteSpace(compressFilePath))
            {
                throw new ArgumentNullException(nameof(compressFilePath));
            }

            if (!File.Exists(compressFilePath))
            {
                throw new FileNotFoundException("目标压缩文件不存在", compressFilePath);
            }

            if (filePaths == null || filePaths.Count() == 0)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            if (string.IsNullOrWhiteSpace(deCompressDirectory))
            {
                throw new ArgumentNullException(nameof(deCompressDirectory));
            }

            //检查输出目录是否以“\\”结尾
            if (deCompressDirectory.EndsWith("\\") == false || deCompressDirectory.EndsWith(":\\") == false)
            {
                deCompressDirectory += "\\";
            }
            throw new NotImplementedException();
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(compressFilePath)))
            {
                ZipEntry theEntry = null;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(deCompressDirectory);
                    string fileName = Path.GetFileName(theEntry.Name);

                    //生成解压目录【用户解压到硬盘根目录时，不需要创建】
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入,但是实际上是有需要的
                    //if (theEntry.CompressedSize == 0)
                    //{
                    //    break;
                    //}

                    //解压文件到指定的目录
                    directoryName = Path.GetDirectoryName(deCompressDirectory + theEntry.Name);
                    //建立下面的目录和子目录
                    Directory.CreateDirectory(directoryName);
                    string filePath = deCompressDirectory + theEntry.Name;
                    using (FileStream streamWriter = File.Create(filePath))
                    {
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            GC.Collect();
        }

        #region DecompressRar
        /// <summary>
        /// 解压rar压缩文件
        /// </summary>
        /// <param name="rarFile">rar压缩文件路径</param>
        /// <param name="decompressDir">解压目录</param>
        /// <param name="isCreateDir">是否创建压缩文件中的目录,true:按照压缩文件中的文件目录结构解压,false:压缩文件中的所有文件全部解压到解压目录中[默认为true]</param>
        public static void DecompressRar(string rarFile, string decompressDir, bool isCreateDir = true)
        {
            if (!File.Exists(rarFile))
            {
                throw new FileNotFoundException("不能找到需要解压的文件", rarFile);
            }

            using (Unrar unrar = new Unrar(rarFile))
            {
                unrar.Open(Unrar.OpenMode.Extract);
                unrar.DestinationPath = decompressDir;

                while (unrar.ReadHeader())
                {
                    if (unrar.CurrentFile.IsDirectory)
                    {
                        unrar.Skip();
                    }
                    else
                    {
                        if (isCreateDir)
                        {
                            unrar.Extract();
                        }
                        else
                        {
                            unrar.Extract(Path.Combine(decompressDir, Path.GetFileName(unrar.CurrentFile.FileName)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解压rar压缩文件
        /// </summary>
        /// <param name="rarFile">rar压缩文件路径</param>
        /// <param name="files">目标文件名集合,不包含路径</param>
        /// <param name="decompressDir">解压目录</param>
        /// <param name="isCreateDir">是否创建压缩文件中的目录,true:按照压缩文件中的文件目录结构解压,false:压缩文件中的所有文件全部解压到解压目录中[默认为true]</param>
        public static void DecompressRar(string rarFile, IEnumerable<string> files, string decompressDir, bool isCreateDir = true)
        {
            if (!File.Exists(rarFile))
            {
                throw new FileNotFoundException("不能找到需要解压的文件", rarFile);
            }

            using (Unrar unrar = new Unrar(rarFile))
            {
                unrar.Open(Unrar.OpenMode.Extract);
                unrar.DestinationPath = decompressDir;
                bool notFind;
                while (unrar.ReadHeader())
                {
                    if (unrar.CurrentFile.IsDirectory)
                    {
                        unrar.Skip();
                    }
                    else
                    {
                        notFind = (from tmpItem in files where string.Equals(unrar.CurrentFile.FileName, tmpItem, StringComparison.OrdinalIgnoreCase) select tmpItem).Count() == 0;
                        if (notFind)
                        {
                            unrar.Skip();
                            continue;
                        }

                        if (isCreateDir)
                        {
                            unrar.Extract();
                        }
                        else
                        {
                            unrar.Extract(Path.Combine(decompressDir, Path.GetFileName(unrar.CurrentFile.FileName)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取rar文件内的文件列表
        /// </summary>
        /// <param name="rarFile">rar压缩文件路径</param>
        /// <returns>rar文件内的文件列表</returns>
        public static List<string> GetRarFileList(string rarFile)
        {
            if (!File.Exists(rarFile))
            {
                throw new FileNotFoundException("不能找到需要解压的文件", rarFile);
            }

            var files = new List<string>();
            using (Unrar unrar = new Unrar(rarFile))
            {
                unrar.Open(Unrar.OpenMode.Extract);
                unrar.DestinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), "TmpRar");
                while (unrar.ReadHeader())
                {
                    if (!unrar.CurrentFile.IsDirectory)
                    {
                        files.Add(unrar.CurrentFile.FileName);
                    }

                    unrar.Skip();
                }
            }

            return files;
        }
        #endregion
    }
}