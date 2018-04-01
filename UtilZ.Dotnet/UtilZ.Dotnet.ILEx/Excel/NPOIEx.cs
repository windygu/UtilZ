using UtilZ.Lib.Base;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Foundation;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Lib.Extend.Excel
{
    /// <summary>
    /// NPOI导出DataTable到Excel
    /// </summary>
    public class NPOIEx
    {
        /// <summary>
        /// 导出DataTable到Excel
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="dicHeadCaptions">标题名称映射字典集合</param>
        public static void ExportToExcel(DataTable dt, string filePath, Dictionary<string, string> dicHeadCaptions = null)
        {
            /************************************************************************************
            * version       MaxSheet                MaxRow    MaxCol
            * xls           无限(<2003上限为255)    65536     256
            * xlsx          无限                    1048576   16384
            * 
            * 注:无限为只要硬件够强,可以无限支撑
            ************************************************************************************/

            if (dt == null)
            {
                throw new ArgumentNullException("dt", "数据源不能为空");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath", "文件路径不能为空");
            }

            string extension = Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentException("文件扩展名不能为空");
            }

            extension = extension.ToLower();//扩展名转换为小写,方便后面比较            
            //IWorkbook workbook = WorkbookFactory.Create(filePath);//使用接口，自动识别excel2003/2007格式,但是总是报异常,没搞懂为毛,所以就不用这个方式
            int sheetMaxRow = 0;//单个sheet支持最大行数
            IWorkbook workbook = null;//表格对象
            if (extension.Equals(".xls"))
            {
                if (dt.Columns.Count > 256)
                {
                    throw new ArgumentException("2003格式的表格列数上限256,当前超出上限");
                }

                workbook = new HSSFWorkbook();
                sheetMaxRow = 65536;
            }
            else if (extension.Equals(".xlsx"))
            {
                if (dt.Columns.Count > 16384)
                {
                    throw new ArgumentException("2007以后格式的表格列数上限16384,当前超出上限");
                }

                workbook = new XSSFWorkbook();
                sheetMaxRow = 1048576;
            }
            else
            {
                throw new ArgumentException(string.Format("不支持的类型{0}", extension));
            }

            if (dicHeadCaptions == null)
            {
                dicHeadCaptions = new Dictionary<string, string>();
            }

            Export(workbook, dt, filePath, sheetMaxRow, dicHeadCaptions);
        }

        /// <summary>
        /// 导出DataTable到Excel
        /// </summary>
        /// <param name="workbook">IWorkbook</param>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetMaxRow">单个sheet支持最大行数</param>
        /// <param name="dicHeadCaptions">标题名称影射</param>
        private static void Export(IWorkbook workbook, DataTable dt, string filePath, int sheetMaxRow, Dictionary<string, string> dicHeadCaptions)
        {
            int rowCount = dt.Rows.Count;//总数据行数
            int colCount = dt.Columns.Count;//总列数
            int sheetRealDataRowCount = sheetMaxRow - 1;//单个sheet实际数据最大行数
            int lastSheetRealDataRowCount = rowCount % sheetRealDataRowCount;//最后一个sheet实际数据最大行数
            int sheetCount = rowCount / sheetRealDataRowCount;//sheet个数
            if (lastSheetRealDataRowCount != 0)//如果最后一个sheet实际数据最大行数不为0,则多加一个非满数据的sheet
            {
                sheetCount = sheetCount + 1;
            }

            string head = null;//列标题
            int currentDataRowCount = sheetRealDataRowCount;//当前sheet实际数据行数
            int sheetDataOffset = 0;//当前sheet数据在总数据中的偏移位置
            DataRow currentRow = null;//当前正在写入的数据行
            Type cellVelueType = null;
            object cellValue = null;
            for (int i = 1; i <= sheetCount; i++)
            {
                ISheet sheet = workbook.CreateSheet("Sheet" + i.ToString());
                //表头
                IRow headRow = sheet.CreateRow(0);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    head = dt.Columns[j].ColumnName;
                    if (dicHeadCaptions.ContainsKey(head))
                    {
                        head = dicHeadCaptions[head];
                    }

                    ICell headCell = headRow.CreateCell(j);
                    headCell.SetCellValue(head);
                }

                if (i == sheetCount)//最后一个sheet
                {
                    currentDataRowCount = lastSheetRealDataRowCount;
                }

                //计算当前sheet数据在总数据中的偏移位置
                //减1是因为写入数据时循环是从1开始,为了提升效率,将本在得到原始数据行处的减1写到了此处
                sheetDataOffset = sheetRealDataRowCount * (i - 1) - 1;

                //数据
                for (int j = 1; j <= currentDataRowCount; j++)
                {
                    currentRow = dt.Rows[sheetDataOffset + j];//得到原始数据行
                    IRow dataRow = sheet.CreateRow(j);//创建excel数据行
                    for (int k = 0; k < colCount; k++)
                    {
                        ICell dataCell = dataRow.CreateCell(k);//创建excel单格
                        //dataCell.SetCellValue(currentRow[k].ToString());//设置单元格数据

                        //单元格写入值
                        cellValue = currentRow[k];
                        if (cellValue == null || DBNull.Value == cellValue)
                        {
                            dataCell.SetCellValue(string.Empty);
                            continue;
                        }

                        cellVelueType = cellValue.GetType();
                        if (cellVelueType == ClrSystemType.BoolType)//bool类型
                        {
                            dataCell.SetCellValue(Convert.ToBoolean(cellValue));
                        }
                        else if (cellVelueType == ClrSystemType.DateTimeType)//时间类型
                        {
                            dataCell.SetCellValue(Convert.ToDateTime(cellValue));
                        }
                        else if (ClrSystemType.IsSystemNumberType(cellVelueType))//如果是系统数值类型
                        {
                            dataCell.SetCellValue(Convert.ToDouble(cellValue));
                        }
                        else if (cellVelueType == ClrSystemType.StringType || cellVelueType == ClrSystemType.CharType)//字符串或字符类型
                        {
                            dataCell.SetCellValue(cellValue.ToString());
                        }
                        else if (cellValue as IRichTextString != null)//RichTextString类型
                        {
                            dataCell.SetCellValue((IRichTextString)cellValue);
                        }
                        else//默认当作字符串类型处理
                        {
                            dataCell.SetCellValue(cellVelueType.ToString());
                        }
                    }
                }
            }

            //转为字节数组
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                var buffer = stream.ToArray();

                //保存为Excel文件
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                }
            }
        }
    }
}
