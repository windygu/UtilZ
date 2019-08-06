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
    /// ѹ���ͽ�ѹ�ļ�������
    /// </summary>
    public class CompressHelper : CompressEx
    {
        /// <summary>
        /// ѹ�������ļ���zip�ļ�
        /// </summary>
        /// <param name="filePath">��ѹ�����ļ�</param>
        /// <param name="compressFilePath">ѹ���ļ�����·��</param>
        /// <param name="compressionLevel">ѹ���̶ȣ���Χ0-9����ֵԽ��ѹ������Խ��[Ĭ��Ϊ5]</param>
        /// <param name="blockSize">�ֿ��С[Ĭ��Ϊ1024]</param>
        public static void CompressFileToZip(string filePath, string compressFilePath, int compressionLevel = 5, int blockSize = 10204)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!System.IO.File.Exists(filePath))//����ļ�û���ҵ����򱨴�
            {
                throw new FileNotFoundException("Ŀ���ļ�������", filePath);
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
        /// ѹ��Ŀ¼��zip�ļ���������Ŀ¼�������ļ���
        /// </summary>
        /// <param name="directory">��ѹ����Ŀ¼</param>
        /// <param name="compressFilePath">ѹ���ļ�����·��</param>
        /// <param name="compressLevel">ѹ���̶ȣ���Χ0-9����ֵԽ��ѹ������Խ��[Ĭ��Ϊ5]</param>
        public static void CompressDirectoryToZip(string directory, string compressFilePath, int compressLevel = 5)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                throw new FileNotFoundException("Ŀ��Ŀ¼������", directory);
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

            string rootMark = directory + "\\";//�õ���ǰ·����λ�ã��Ա�ѹ��ʱ����ѹ������ת������·����
            Crc32 crc = new Crc32();

            using (ZipOutputStream outPutStream = new ZipOutputStream(File.Create(compressFilePath)))
            {
                outPutStream.SetLevel(compressLevel); // 0 - store only to 9 - means best compression
                foreach (string file in files)
                {
                    //��ѹ���ļ�
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
        /// ��zipѹ���ļ�(ѹ���ļ��к�����Ŀ¼)
        /// </summary>
        /// <param name="compressFilePath">����ѹ�����ļ�·��</param>
        /// <param name="deCompressDirectory">��ѹ��Ŀ¼</param>
        /// <returns>��ѹ����ļ��б�</returns>
        public static void DeCompressZip(string compressFilePath, string deCompressDirectory)
        {
            if (string.IsNullOrWhiteSpace(compressFilePath))
            {
                throw new ArgumentNullException(nameof(compressFilePath));
            }

            if (!File.Exists(compressFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", compressFilePath);
            }

            if (string.IsNullOrWhiteSpace(deCompressDirectory))
            {
                throw new ArgumentNullException(nameof(deCompressDirectory));
            }

            //������Ŀ¼�Ƿ��ԡ�\\����β
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

                    //���ɽ�ѹĿ¼���û���ѹ��Ӳ�̸�Ŀ¼ʱ������Ҫ������
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    //����ļ���ѹ�����СΪ0��ô˵������ļ��ǿյ�,��˲���Ҫ���ж���д��,����ʵ����������Ҫ��
                    //if (theEntry.CompressedSize == 0)
                    //{
                    //    break;
                    //}

                    //��ѹ�ļ���ָ����Ŀ¼
                    directoryName = Path.GetDirectoryName(deCompressDirectory + theEntry.Name);
                    //���������Ŀ¼����Ŀ¼
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
        /// ��zipѹ���ļ��е�ָ���ļ�
        /// </summary>
        /// <param name="compressFilePath">����ѹ�����ļ�·��</param>
        /// <param name="filePaths">ѹ������Ҫ��ѹ������ļ�·��·��</param>
        /// <param name="deCompressDirectory">��ѹ��Ŀ¼</param>
        /// <returns>��ѹ����ļ��б�</returns>
        public static void DeCompressZip(string compressFilePath, IEnumerable<string> filePaths, string deCompressDirectory)
        {
            if (string.IsNullOrWhiteSpace(compressFilePath))
            {
                throw new ArgumentNullException(nameof(compressFilePath));
            }

            if (!File.Exists(compressFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", compressFilePath);
            }

            if (filePaths == null || filePaths.Count() == 0)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            if (string.IsNullOrWhiteSpace(deCompressDirectory))
            {
                throw new ArgumentNullException(nameof(deCompressDirectory));
            }

            //������Ŀ¼�Ƿ��ԡ�\\����β
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

                    //���ɽ�ѹĿ¼���û���ѹ��Ӳ�̸�Ŀ¼ʱ������Ҫ������
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    //����ļ���ѹ�����СΪ0��ô˵������ļ��ǿյ�,��˲���Ҫ���ж���д��,����ʵ����������Ҫ��
                    //if (theEntry.CompressedSize == 0)
                    //{
                    //    break;
                    //}

                    //��ѹ�ļ���ָ����Ŀ¼
                    directoryName = Path.GetDirectoryName(deCompressDirectory + theEntry.Name);
                    //���������Ŀ¼����Ŀ¼
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
        /// ��ѹrarѹ���ļ�
        /// </summary>
        /// <param name="rarFile">rarѹ���ļ�·��</param>
        /// <param name="decompressDir">��ѹĿ¼</param>
        /// <param name="isCreateDir">�Ƿ񴴽�ѹ���ļ��е�Ŀ¼,true:����ѹ���ļ��е��ļ�Ŀ¼�ṹ��ѹ,false:ѹ���ļ��е������ļ�ȫ����ѹ����ѹĿ¼��[Ĭ��Ϊtrue]</param>
        public static void DecompressRar(string rarFile, string decompressDir, bool isCreateDir = true)
        {
            if (!File.Exists(rarFile))
            {
                throw new FileNotFoundException("�����ҵ���Ҫ��ѹ���ļ�", rarFile);
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
        /// ��ѹrarѹ���ļ�
        /// </summary>
        /// <param name="rarFile">rarѹ���ļ�·��</param>
        /// <param name="files">Ŀ���ļ�������,������·��</param>
        /// <param name="decompressDir">��ѹĿ¼</param>
        /// <param name="isCreateDir">�Ƿ񴴽�ѹ���ļ��е�Ŀ¼,true:����ѹ���ļ��е��ļ�Ŀ¼�ṹ��ѹ,false:ѹ���ļ��е������ļ�ȫ����ѹ����ѹĿ¼��[Ĭ��Ϊtrue]</param>
        public static void DecompressRar(string rarFile, IEnumerable<string> files, string decompressDir, bool isCreateDir = true)
        {
            if (!File.Exists(rarFile))
            {
                throw new FileNotFoundException("�����ҵ���Ҫ��ѹ���ļ�", rarFile);
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
        /// ��ȡrar�ļ��ڵ��ļ��б�
        /// </summary>
        /// <param name="rarFile">rarѹ���ļ�·��</param>
        /// <returns>rar�ļ��ڵ��ļ��б�</returns>
        public static List<string> GetRarFileList(string rarFile)
        {
            if (!File.Exists(rarFile))
            {
                throw new FileNotFoundException("�����ҵ���Ҫ��ѹ���ļ�", rarFile);
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