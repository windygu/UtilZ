using org.in2bits.MyXls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.BaseEx.NExtendExcel
{
    /// <summary>
    /// MyXls导出Excel辅助类
    /// </summary>
    public class NExtendMyXlsExcel
    {
        /// <summary>
        /// 导出数据到Excel
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
            if (extension.Equals(".xls"))
            {
                if (dt.Columns.Count > 256)
                {
                    throw new ArgumentException("2003格式的表格列数上限256,当前超出上限");
                }
            }
            else
            {
                throw new ArgumentException(string.Format("不支持的类型{0}", extension));
            }

            if (dicHeadCaptions == null)
            {
                dicHeadCaptions = new Dictionary<string, string>();
            }

            NExtendMyXlsExcel.ExportExcel(dt, filePath, 65536, dicHeadCaptions);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetMaxRow">单个sheet支持最大行数</param>
        /// <param name="dicHeadCaptions">标题名称映射字典集合</param>
        private static void ExportExcel(DataTable dt, string filePath, int sheetMaxRow, Dictionary<string, string> dicHeadCaptions)
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

            XlsDocument doc = new XlsDocument();
            doc.FileName = filePath;
            string head = null;//列标题
            int currentDataRowCount = sheetRealDataRowCount;//当前sheet实际数据行数
            int sheetDataOffset = 0;//当前sheet数据在总数据中的偏移位置
            DataRow currentRow = null;//当前正在写入的数据行
            for (int i = 1; i <= sheetCount; i++)
            {
                Worksheet sheet = doc.Workbook.Worksheets.Add("Sheet" + i.ToString());
                //表头
                for (int j = 1; j <= dt.Columns.Count; j++)
                {
                    head = dt.Columns[j - 1].ColumnName;
                    if (dicHeadCaptions.ContainsKey(head))
                    {
                        head = dicHeadCaptions[head];
                    }

                    sheet.Cells.Add(1, j, head);
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
                    for (int k = 1; k <= colCount; k++)
                    {
                        sheet.Cells.Add(j + 1, k, currentRow[k - 1]);
                    }
                }
            }

            doc.Save(true);
        }
    }
}
