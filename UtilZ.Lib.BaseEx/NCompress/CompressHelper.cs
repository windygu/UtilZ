using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Reader;
using UtilZ.Lib.Base.Extend;

namespace UtilZ.Lib.BaseEx.NCompress
{
    /// <summary>
    /// 压缩和解压文件辅助类
    /// </summary>
    public class CompressHelper : NExtendCompress
    {
        /// <summary>
        /// 压缩单个文件到zip文件
        /// </summary>
        /// <param name="file">待压缩的文件</param>
        /// <param name="compressFilePath">压缩文件保存路径</param>
        /// <param name="compressionLevel">压缩程度，范围0-9，数值越大，压缩程序越高[默认为5]</param>
        /// <param name="blockSize">分块大小[默认为1024]</param>
        public static void CompressFileToZip(string file, string compressFilePath, int compressionLevel = 5, int blockSize = 10204)
        {
            if (!System.IO.File.Exists(file))//如果文件没有找到，则报错
            {
                throw new FileNotFoundException("The specified file " + file + " could not be found. Zipping aborderd");
            }

            using (FileStream streamToZip = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fs = File.Create(compressFilePath))
                {
                    ZipOutputStream zipStream = new ZipOutputStream(fs);
                    ZipEntry zipEntry = new ZipEntry(file);
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

                    if (fileName != String.Empty)
                    {
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
            }
            GC.Collect();
        }

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

            Unrar unrar = new Unrar(rarFile);
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
                        unrar.Extract(decompressDir + Path.GetFileName(unrar.CurrentFile.FileName));
                    }
                }
            }
            unrar.Close();
        }
    }
}