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
        #region Zip
        /// <summary>
        /// ѹ�������ļ���zip�ļ�
        /// </summary>
        /// <param name="filePath">��ѹ�����ļ�</param>
        /// <param name="zipFilePath">ѹ���ļ�����·��</param>
        /// <param name="compressionLevel">ѹ���̶ȣ���Χ0-9����ֵԽ��ѹ������Խ��[Ĭ��Ϊ5]</param>
        /// <param name="blockSize">�ֿ��С[Ĭ��Ϊ1024]</param>
        public static void CompressFileToZip(string filePath, string zipFilePath, int compressionLevel = 5, int blockSize = 10204)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!System.IO.File.Exists(filePath))//����ļ�û���ҵ����򱨴�
            {
                throw new FileNotFoundException("Ŀ���ļ�������", filePath);
            }

            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                throw new ArgumentNullException(nameof(zipFilePath));
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
                using (FileStream fs = File.Create(zipFilePath))
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
        /// <param name="zipFilePath">ѹ���ļ�����·��</param>
        /// <param name="compressionLevel">ѹ���̶ȣ���Χ0-9����ֵԽ��ѹ������Խ��[Ĭ��Ϊ5]</param>
        public static void CompressDirectoryToZip(string directory, string zipFilePath, int compressionLevel = 5)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                throw new FileNotFoundException("Ŀ��Ŀ¼������", directory);
            }

            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            if (compressionLevel < 0 || compressionLevel > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(compressionLevel));
            }

            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            string rootMark = directory + "\\";//�õ���ǰ·����λ�ã��Ա�ѹ��ʱ����ѹ������ת������·����
            Crc32 crc = new Crc32();

            using (ZipOutputStream outPutStream = new ZipOutputStream(File.Create(zipFilePath)))
            {
                outPutStream.SetLevel(compressionLevel); // 0 - store only to 9 - means best compression
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
        /// <param name="zipFilePath">����ѹ�����ļ�·��</param>
        /// <param name="decompressDir">��ѹ��Ŀ¼</param>
        /// <returns>��ѹ����ļ��б�</returns>
        public static void DeCompressZip(string zipFilePath, string decompressDir)
        {
            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", zipFilePath);
            }

            if (string.IsNullOrWhiteSpace(decompressDir))
            {
                throw new ArgumentNullException(nameof(decompressDir));
            }

            //������Ŀ¼�Ƿ��ԡ�\\����β
            if (decompressDir.EndsWith("\\") == false || decompressDir.EndsWith(":\\") == false)
            {
                decompressDir += "\\";
            }

            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry = null;
                string fileName, directoryName;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    fileName = Path.GetFileName(theEntry.Name);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        //Ŀ¼
                        continue;
                    }

                    //���ɽ�ѹĿ¼���û���ѹ��Ӳ�̸�Ŀ¼ʱ������Ҫ������
                    directoryName = Path.GetDirectoryName(decompressDir);
                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    //����ļ���ѹ�����СΪ0��ô˵������ļ��ǿյ�,��˲���Ҫ���ж���д��,����ʵ����������Ҫ��
                    //if (theEntry.CompressedSize == 0)
                    //{
                    //    break;
                    //}

                    //��ѹ�ļ���ָ����Ŀ¼
                    directoryName = Path.GetDirectoryName(decompressDir + theEntry.Name);
                    //���������Ŀ¼����Ŀ¼
                    Directory.CreateDirectory(directoryName);

                    using (FileStream streamWriter = File.Create(decompressDir + theEntry.Name))
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
        /// <param name="zipFilePath">����ѹ�����ļ�·��</param>
        /// <param name="filePaths">ѹ������Ҫ��ѹ������ļ�·��·��</param>
        /// <param name="decompressDir">��ѹ��Ŀ¼</param>
        /// <returns>��ѹ����ļ��б�</returns>
        public static void DeCompressZip(string zipFilePath, IEnumerable<string> filePaths, string decompressDir)
        {
            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", zipFilePath);
            }

            if (filePaths == null || filePaths.Count() == 0)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            if (string.IsNullOrWhiteSpace(decompressDir))
            {
                throw new ArgumentNullException(nameof(decompressDir));
            }

            //������Ŀ¼�Ƿ��ԡ�\\����β
            if (decompressDir.EndsWith("\\") == false || decompressDir.EndsWith(":\\") == false)
            {
                decompressDir += "\\";
            }

            var filePathArr = filePaths.Select(t => { return t.Trim().Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToLower(); }).ToArray();
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry = null;
                string fileName, directoryName;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    fileName = Path.GetFileName(theEntry.Name);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        //Ŀ¼
                        continue;
                    }
                    else
                    {
                        if (!filePathArr.Contains(theEntry.Name.ToLower()))
                        {
                            continue;
                        }
                    }

                    //���ɽ�ѹĿ¼���û���ѹ��Ӳ�̸�Ŀ¼ʱ������Ҫ������
                    directoryName = Path.GetDirectoryName(decompressDir);
                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    //����ļ���ѹ�����СΪ0��ô˵������ļ��ǿյ�,��˲���Ҫ���ж���д��,����ʵ����������Ҫ��
                    //if (theEntry.CompressedSize == 0)
                    //{
                    //    break;
                    //}

                    //��ѹ�ļ���ָ����Ŀ¼
                    directoryName = Path.GetDirectoryName(decompressDir + theEntry.Name);
                    //���������Ŀ¼����Ŀ¼
                    Directory.CreateDirectory(directoryName);
                    string filePath = decompressDir + theEntry.Name;
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

        /// <summary>
        /// ��ȡrar�ļ��ڵ��ļ��б�
        /// </summary>
        /// <param name="zipFilePath">zipѹ���ļ�·��</param>
        /// <returns>rar�ļ��ڵ��ļ��б�</returns>
        public static List<string> GetZipFileList(string zipFilePath)
        {
            if (string.IsNullOrWhiteSpace(zipFilePath))
            {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", zipFilePath);
            }

            var fileList = new List<string>();
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry = null;
                string fileName;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    fileName = Path.GetFileName(theEntry.Name);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        //Ŀ¼
                        continue;
                    }
                    else
                    {
                        fileList.Add(theEntry.Name);
                    }
                }
            }

            GC.Collect();
            return fileList;
        }
        #endregion

        #region DecompressRar
        /// <summary>
        /// ��ѹrarѹ���ļ�
        /// </summary>
        /// <param name="rarFilePath">rarѹ���ļ�·��</param>
        /// <param name="decompressDir">��ѹĿ¼</param>
        /// <param name="isCreateDir">�Ƿ񴴽�ѹ���ļ��е�Ŀ¼,true:����ѹ���ļ��е��ļ�Ŀ¼�ṹ��ѹ,false:ѹ���ļ��е������ļ�ȫ����ѹ����ѹĿ¼��[Ĭ��Ϊtrue]</param>
        public static void DecompressRar(string rarFilePath, string decompressDir, bool isCreateDir = true)
        {
            if (string.IsNullOrWhiteSpace(rarFilePath))
            {
                throw new ArgumentNullException(nameof(rarFilePath));
            }

            if (!File.Exists(rarFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", rarFilePath);
            }

            if (string.IsNullOrWhiteSpace(decompressDir))
            {
                throw new ArgumentNullException(nameof(decompressDir));
            }

            using (Unrar unrar = new Unrar(rarFilePath))
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
        /// <param name="rarFilePath">rarѹ���ļ�·��</param>
        /// <param name="filePaths">Ŀ���ļ�������,ѹ���������·��</param>
        /// <param name="decompressDir">��ѹĿ¼</param>
        /// <param name="isCreateDir">�Ƿ񴴽�ѹ���ļ��е�Ŀ¼,true:����ѹ���ļ��е��ļ�Ŀ¼�ṹ��ѹ,false:ѹ���ļ��е������ļ�ȫ����ѹ����ѹĿ¼��[Ĭ��Ϊtrue]</param>
        public static void DecompressRar(string rarFilePath, IEnumerable<string> filePaths, string decompressDir, bool isCreateDir = true)
        {
            if (string.IsNullOrWhiteSpace(rarFilePath))
            {
                throw new ArgumentNullException(nameof(rarFilePath));
            }

            if (!File.Exists(rarFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", rarFilePath);
            }

            if (filePaths == null || filePaths.Count() == 0)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }

            if (string.IsNullOrWhiteSpace(decompressDir))
            {
                throw new ArgumentNullException(nameof(decompressDir));
            }

            var filePathArr = filePaths.Select(t => { return t.Trim().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).ToLower(); }).ToArray();

            using (Unrar unrar = new Unrar(rarFilePath))
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
                        if (!filePathArr.Contains(unrar.CurrentFile.FileName.ToLower()))
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
        /// <param name="rarFilePath">rarѹ���ļ�·��</param>
        /// <returns>rar�ļ��ڵ��ļ��б�</returns>
        public static List<string> GetRarFileList(string rarFilePath)
        {
            if (string.IsNullOrWhiteSpace(rarFilePath))
            {
                throw new ArgumentNullException(nameof(rarFilePath));
            }

            if (!File.Exists(rarFilePath))
            {
                throw new FileNotFoundException("Ŀ��ѹ���ļ�������", rarFilePath);
            }

            var fileList = new List<string>();
            using (Unrar unrar = new Unrar(rarFilePath))
            {
                unrar.Open(Unrar.OpenMode.Extract);
                unrar.DestinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), "TmpRar");
                while (unrar.ReadHeader())
                {
                    if (!unrar.CurrentFile.IsDirectory)
                    {
                        fileList.Add(unrar.CurrentFile.FileName);
                    }

                    unrar.Skip();
                }
            }

            return fileList;
        }
        #endregion
    }
}