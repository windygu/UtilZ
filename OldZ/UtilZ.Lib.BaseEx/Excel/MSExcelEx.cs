using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilZ.Lib.Extend.Excel
{
    /// <summary>
    /// Excel扩展方法
    /// </summary>
    public class MSExcelEx
    {
        /************************************************************************************
         * 添加.NET引用Microsoft.Office.Interop.Excell
         * Excel版本信息如下:
         * Excel版本	Sheet最大数		单个Sheet最大行数     单个Sheet最大列数
         * xls		    无限(97之前255)	65536			      256
         * xlsx		    无限			1048576				  16384
         * 注:
         * Sheet最大数无限,是指只要物理内存足够就可以无限增加
         ************************************************************************************/

        /*
        #region 结束Excel进程
        /// <summary>
        /// 结束Excel进程
        /// </summary>
        /// <param name="excelApp">Excel工作表对象</param>
        private static void ExitExcellApp(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            IntPtr ptr = new IntPtr(excelApp.Hwnd);

            try
            {
                //excelApp.Workbooks.Close();
                //excelApp.Workbooks.Application.Quit();
                //excelApp.Application.Quit();
                excelApp.Quit();
            }
            catch
            { }

            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp.Workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp.Application);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch
            { }

            try
            {
                int excellProcessID = 0;
                int ret = NExtendExcel.GetWindowThreadProcessId(ptr, out excellProcessID);
                if (excellProcessID == 0)
                {
                    return;
                }

                var pro = System.Diagnostics.Process.GetProcessById(excellProcessID);
                pro.Kill();
            }
            catch
            { }
        }
        #endregion

        #region breeze
        /// <summary>
        /// 创建Excel列头与表列字段名映射
        /// </summary>
        /// <param name="cols">表列集合</param>
        /// <returns>Excel列头与表列字段名映射</returns>
        private static Dictionary<string, string> CreateExcelHeadFieldMap(DataColumnCollection cols)
        {
            Dictionary<string, string> colHeadFieldMap = new Dictionary<string, string>();
            string colHead = string.Empty;

            for (int i = 0; i < cols.Count; i++)
            {
                if (i < 26)
                {
                    colHead = ((char)(65 + i)).ToString();
                }
                else if (i < 676)
                {
                    int c1 = i / 26;
                    int c2 = i % 26;
                    colHead = ((char)(65 + c1)).ToString() + ((char)(65 + c2)).ToString();
                }
                else if (i < 16384)//Excel2003的最大列数是256列，2007以上版本是16384列
                {
                    int c1 = (i - 675) / 676;
                    int c2 = (i - 676) / 26;
                    int c3 = i % 26;
                    colHead = ((char)(65 + c1)).ToString() + ((char)(65 + c2)).ToString() + ((char)(65 + c3)).ToString();
                }
                else
                {
                    throw new NotSupportedException(string.Format("列数超过17576,不支持"));
                }

                colHeadFieldMap.Add(cols[i].ColumnName, colHead);
            }

            return colHeadFieldMap;
        }

        /// <summary>
        /// Datatable导出到Excell,效率很低
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="fileName">Excell文件路径</param>
        protected static void ExportDatatableToExcel(DataTable dt, string fileName)
        {
            //创建 Excell工作簿
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            try
            {
                excelApp.Visible = false;
                // xBook.Workbooks.Open(excellPath);//打开excell文件
                excelApp.Workbooks.Add();//添加一个excell表格
                //获得sheet
                Microsoft.Office.Interop.Excel.Worksheet xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[1]);
                //单元格
                Microsoft.Office.Interop.Excel.Range cell;

                DataColumnCollection cols = dt.Columns;
                Dictionary<string, string> colHeadFieldMap = NExtendExcel.CreateExcelHeadFieldMap(cols);
                string excellRowIndexStr = null;

                excellRowIndexStr = "1";
                foreach (DataColumn col in cols)
                {
                    cell = xSheet.get_Range(colHeadFieldMap[col.ColumnName] + excellRowIndexStr);
                    cell.Value = col.ColumnName;
                }

                int excellRowIndex = 2;
                foreach (DataRow row in dt.Rows)
                {
                    excellRowIndexStr = excellRowIndex++.ToString();
                    foreach (DataColumn col in cols)
                    {
                        cell = xSheet.get_Range(colHeadFieldMap[col.ColumnName] + excellRowIndexStr);
                        cell.Value = row[col.ColumnName];
                    }
                }

                xSheet.SaveAs(fileName);
            }
            finally
            {
                NExtendExcel.ExitExcellApp(excelApp);
            }
        }
        #endregion

        static NExtendExcel()
        {
            NExtendExcel._binaryType = typeof(byte[]);
        }

        

        /// <summary>
        /// 二进制数据类型
        /// </summary>
        private static readonly Type _binaryType = null;

        /// <summary>
        /// Datatable导出到Excell
        /// </summary>
        /// <param name="excelVer">Excel版本,true:2007以上的版本,false:2003及以下的版本</param>
        /// <param name="dt">Datatable</param>
        /// <param name="fileName">Excell文件路径</param>
        /// <param name="fieldColHeadMap">表字段列标题映射集合,无集合传null[key:数据库字段名(要求必须全部大写),value:Excel中显示的列标题]</param>
        /// <param name="ignoreCols">忽略的列集合(要求必须全部大写)</param>
        /// <param name="sheetPageSize">每页数据的大小(每个sheet里面存放数据的条数)[默认为最大值65535,每个sheet最大行数为65535,最大值为65534是因为列标题要占一行,1997-2003版本的xls中每个表单最大只支持65536行，2010可以支持1048576行]</param>
        public static void ExportDatatableToExcel(bool excelVer, DataTable dt, string fileName, Dictionary<string, string> fieldColHeadMap = null, IEnumerable<string> ignoreCols = null, int sheetPageSize = UInt16.MaxValue)
        {
            if (dt == null)
            {
                throw new ArgumentNullException("dt");
            }

            if (sheetPageSize < 1)
            {
                throw new ArgumentException(string.Format("pageSize值应为大于0的正数,当前值:{0}无效", sheetPageSize));
            }

            int rowMaxcount = 0;
            int colMaxcount = 0;
            string extention = Path.GetExtension(fileName);
            if (excelVer)
            {
                //2007以上的版本
                if (extention.ToLower().Equals(".xls"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}不是有效的2007及以上版本的扩展名", extention));
                }
                else if (!extention.ToLower().Equals(".xlsx"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}无效", extention));
                }

                rowMaxcount = 1048575;
                if (sheetPageSize > rowMaxcount)
                {
                    throw new ArgumentException(string.Format("单sheet数据页:{0}大小无效,Excell2007及以上的版本最大行数为:{1}", sheetPageSize, rowMaxcount));
                }

                colMaxcount = 16384;
                if (dt.Columns.Count > colMaxcount)
                {
                    throw new ArgumentException(string.Format("列数过多,不是能导出成2007及以上版本的Exce表格,当前列数:{0},2007及以上版本支持最大列数为:{1},无法导出", dt.Columns.Count, colMaxcount));
                }
            }
            else
            {
                //2003及以下的版本
                if (extention.ToLower().Equals(".xlsx"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}不是有效的2003及以下版本的扩展名", extention));
                }
                else if (!extention.ToLower().Equals(".xls"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}无效", extention));
                }

                rowMaxcount = UInt16.MaxValue;
                if (sheetPageSize > rowMaxcount)
                {
                    throw new ArgumentException(string.Format("单sheet数据页:{0}大小无效,Excell2003及以下的版本最大行数为:{1}", sheetPageSize, rowMaxcount));
                }

                colMaxcount = 256;
                if (dt.Columns.Count > colMaxcount)
                {
                    throw new ArgumentException(string.Format("列数过多,不是能导出成2003及以下版本的Exce表格,当前列数:{0},2003及以下版本支持最大列数为:{1},那上导出成2007及以上的格式", dt.Columns.Count, colMaxcount));
                }
            }

            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == NExtendExcel._binaryType)
                {
                    throw new ArgumentException(string.Format("要导出到Excel的表格中的列不能是二进制列,列:{0}为二进制列", col.ColumnName));
                }
            }

            if (fieldColHeadMap == null)
            {
                fieldColHeadMap = new Dictionary<string, string>();
            }

            if (ignoreCols != null)
            {
                foreach (string ignorCol in ignoreCols)
                {
                    dt.Columns.Remove(ignorCol);
                }
            }

            //Datatable导出到Excell
            NExtendExcel.ExportDatatableToExcel(dt, fileName, fieldColHeadMap, sheetPageSize);
        }

        /// <summary>
        /// Datatable导出到Excell
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="fileName">Excell文件路径</param>
        /// <param name="fieldColHeadMap">表字段列标题映射集合,无集合传null[key:数据库字段名(要求必须全部大写),value:Excel中显示的列标题]</param>
        /// <param name="sheetPageSize">每页数据的大小(每个sheet里面存放数据的条数)[默认为最大值65535,每个sheet最大行数为65535,最大值为65534是因为列标题要占一行,1997-2003版本的xls中每个表单最大只支持65536行，2010可以支持1048576行]</param>
        private static void ExportDatatableToExcel(DataTable dt, string fileName, Dictionary<string, string> fieldColHeadMap, int sheetPageSize = UInt16.MaxValue)
        {
            //创建 Excell工作簿
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                excelApp.Visible = false;//设置不可见
                excelApp.Workbooks.Add();//添加一个excell表格
                //计算所需sheet数
                int rowCount = dt.Rows.Count;
                int sheetCount = rowCount / sheetPageSize;
                if (rowCount % sheetPageSize > 0)
                {
                    sheetCount += 1;
                }

                //如果excell默认表格中sheet不够,则添加不够的sheet
                if (sheetCount > excelApp.Sheets.Count)
                {
                    int addCount = sheetCount - excelApp.Sheets.Count;
                    for (int i = 0; i < addCount; i++)
                    {
                        //excelApp.Sheets.Count:Worksheet总数,注:1997以前的版本上限为255个,1997(包含)及以后的版本中无上限,只要硬件够强
                        excelApp.Sheets.Add(Missing.Value, excelApp.Sheets[excelApp.Sheets.Count], Missing.Value, Missing.Value);
                    }
                }

                int dataPosition = 0;//在集合中当前数据写入的位置
                Microsoft.Office.Interop.Excel.Worksheet xSheet = null;//当前要写入数据的sheet
                DataColumnCollection cols = dt.Columns;//DT列集合
                int colCount = cols.Count;//列数
                string colName = string.Empty;//DT中当前操作的列名
                string upperColName = string.Empty;//DT中当前操作的列大写列名
                string colHead = string.Empty;//列标题
                object[,] arr = null;//每次写的数据块
                int availableRowCount = 0;//可用行数

                for (int i = 1; i <= sheetCount; i++)
                {
                    xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[i]);
                    xSheet.Name = string.Format("第{0}页", i);

                    if (rowCount - dataPosition > sheetPageSize)
                    {
                        arr = new object[sheetPageSize + 1, colCount];
                    }
                    else
                    {
                        arr = new object[rowCount - dataPosition + 1, colCount];
                    }

                    if (rowCount - dataPosition >= sheetPageSize)
                    {
                        availableRowCount = sheetPageSize;
                    }
                    else
                    {
                        availableRowCount = rowCount - dataPosition;
                    }

                    for (int j = 0; j < colCount; j++)
                    {
                        colName = cols[j].ColumnName;
                        colHead = colName;
                        upperColName = colName.ToUpper();
                        if (fieldColHeadMap.ContainsKey(upperColName))
                        {
                            colHead = fieldColHeadMap[upperColName];
                        }

                        arr.SetValue(colHead, 0, j);
                        for (int k = 0; k < availableRowCount; k++)
                        {
                            arr.SetValue(dt.Rows[k + dataPosition][colName], k + 1, j);
                        }
                    }

                    xSheet.Range["A1"].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;
                    dataPosition += sheetPageSize;
                }

                xSheet.SaveAs(fileName);
            }
            finally
            {
                NExtendExcel.ExitExcellApp(excelApp);
            }
        }

        /// <summary>
        /// IEnumerable<T>导出到Excell
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="excelVer">Excel版本,true:2007以上的版本,false:2003及以下的版本</param>
        /// <param name="items">object集合</param>
        /// <param name="fileName">Excell文件路径</param>
        /// <param name="fieldColHeadMap">表字段列标题映射集合,无集合传null[key:数据库字段名(要求必须全部大写),value:Excel中显示的列标题]</param>
        /// <param name="ignoreCols">忽略的列集合(要求必须全部大写)</param>
        /// <param name="pageSize">每页数据的大小(每个sheet里面存放数据的条数)[默认为最大值65535,每个sheet最大行数为65535,最大值为65534是因为列标题要占一行,1997-2003版本的xls中每个表单最大只支持65536行，2010可以支持1048576行]</param>
        public static void ExportObjectToExcel<T>(bool excelVer, IEnumerable<T> items, string fileName, Dictionary<string, string> fieldColHeadMap = null, IEnumerable<string> ignoreFields = null, int sheetPageSize = UInt16.MaxValue)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (sheetPageSize < 1)
            {
                throw new ArgumentException(string.Format("pageSize值应为大于0的正数,当前值:{0}无效", sheetPageSize));
            }

            Type paraType = typeof(T);
            var allPubProperties = paraType.GetProperties().ToList();
            //移除掉忽略的属性
            int ignoreFieldCount = allPubProperties.RemoveAll((propertyInfo) => { return ignoreFields.Contains(propertyInfo.Name); });

            int rowMaxcount = 0;
            int colMaxcount = 0;
            string extention = Path.GetExtension(fileName);
            if (excelVer)
            {
                if (extention.ToLower().Equals(".xls"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}不是有效的2007及以上版本的扩展名", extention));
                }
                else if (!extention.ToLower().Equals(".xlsx"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}无效", extention));
                }

                rowMaxcount = 1048575;
                if (sheetPageSize > rowMaxcount)
                {
                    throw new ArgumentException(string.Format("单sheet数据页:{0}大小无效,Excell2007及以上的版本最大行数为:{1}", sheetPageSize, rowMaxcount));
                }

                colMaxcount = 16384;
                if (allPubProperties.Count > colMaxcount)
                {
                    throw new ArgumentException(string.Format("要导出的字段数过多,不是能导出成2007及以上版本的Exce表格,当前要导出的字段数数:{0},2007及以上版本支持最大列数为:{1},无法导出", allPubProperties.Count, colMaxcount));
                }
            }
            else
            {
                if (extention.ToLower().Equals(".xlsx"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}不是有效的2003及以下版本的扩展名", extention));
                }
                else if (!extention.ToLower().Equals(".xls"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}无效", extention));
                }

                rowMaxcount = UInt16.MaxValue;
                if (sheetPageSize > rowMaxcount)
                {
                    throw new ArgumentException(string.Format("单sheet数据页:{0}大小无效,Excell2003及以下的版本最大行数为:{1}", sheetPageSize, rowMaxcount));
                }

                colMaxcount = 256;
                if (allPubProperties.Count > colMaxcount)
                {
                    throw new ArgumentException(string.Format("导出的字段数过多,不是能导出成2003及以下版本的Exce表格,当前要导出的字段数数:{0},2003及以下版本支持最大列数为:{1},那上导出成2007及以上的格式", allPubProperties.Count, colMaxcount));
                }
            }

            if (fieldColHeadMap == null)
            {
                fieldColHeadMap = new Dictionary<string, string>();
            }

            if (ignoreFields == null)
            {
                ignoreFields = new List<string>();
            }

            //IEnumerable<T>导出到Excell
            NExtendExcel.ExportObjectToExcel<T>(allPubProperties, items, fileName, fieldColHeadMap, ignoreFields, sheetPageSize);
        }

        /// <summary>
        /// IEnumerable<T>导出到Excell
        /// </summary>
        /// <param name="allPubProperties">要导出的属性集合</param>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="items">object集合</param>
        /// <param name="fileName">Excell文件路径</param>
        /// <param name="fieldColHeadMap">表字段列标题映射集合,无集合传null[key:数据模型中的属性名(区分大小写),value:Excel中显示的列标题]</param>
        /// <param name="ignoreCols">忽略的列集合(数据模型中的属性名(区分大小写))</param>
        /// <param name="pageSize">每页数据的大小(每个sheet里面存放数据的条数)[默认为最大值65535,每个sheet最大行数为65535,最大值为65534是因为列标题要占一行,1997-2003版本的xls中每个表单最大只支持65536行，2010可以支持1048576行]</param>
        private static void ExportObjectToExcel<T>(List<PropertyInfo> allPubProperties, IEnumerable<T> items, string fileName, Dictionary<string, string> fieldColHeadMap, IEnumerable<string> ignoreFields, int sheetPageSize = UInt16.MaxValue)
        {
            //创建 Excell工作簿
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                excelApp.Visible = false;//设置不可见
                excelApp.Workbooks.Add();//添加一个excell表格

                //计算所需sheet数
                int rowCount = items.Count();
                int sheetCount = rowCount / sheetPageSize;
                if (rowCount % sheetPageSize > 0)
                {
                    sheetCount += 1;
                }

                //如果excell默认表格中sheet不够,则添加不够的sheet
                if (sheetCount > excelApp.Sheets.Count)
                {
                    int addCount = sheetCount - excelApp.Sheets.Count;
                    for (int i = 0; i < addCount; i++)
                    {
                        //excelApp.Sheets.Count:Worksheet总数,注:1997以前的版本上限为255个,1997(包含)及以后的版本中无上限,只要硬件够强
                        excelApp.Sheets.Add(Missing.Value, excelApp.Sheets[excelApp.Sheets.Count], Missing.Value, Missing.Value);
                    }
                }

                int dataPosition = 0;//在集合中当前数据写入的位置
                Microsoft.Office.Interop.Excel.Worksheet xSheet = null;//当前要写入数据的sheet
                int colCount = allPubProperties.Count;//列数
                PropertyInfo propertyInfo = null;//当前正在写入的属性
                string colHead = string.Empty;//列标题
                object[,] arr = null;//每次写的数据块
                int availableRowCount = 0;//可用行数
                object propertyValue = null;//属性值
                bool isSimpleType = true;//属性是否是简单数据类型,即值类型,字符串类型

                for (int i = 1; i <= sheetCount; i++)
                {
                    xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[i]);
                    xSheet.Name = string.Format("第{0}页", i);

                    if (rowCount - dataPosition > sheetPageSize)
                    {
                        arr = new object[sheetPageSize + 1, colCount];
                    }
                    else
                    {
                        arr = new object[rowCount - dataPosition + 1, colCount];
                    }

                    if (rowCount - dataPosition >= sheetPageSize)
                    {
                        availableRowCount = sheetPageSize;
                    }
                    else
                    {
                        availableRowCount = rowCount - dataPosition;
                    }

                    for (int j = 0; j < colCount; j++)
                    {
                        propertyInfo = allPubProperties[j];
                        colHead = propertyInfo.Name;
                        if (fieldColHeadMap.ContainsKey(propertyInfo.Name))
                        {
                            colHead = fieldColHeadMap[propertyInfo.Name];
                        }

                        //列标题
                        arr.SetValue(colHead, 0, j);

                        //行属性数据
                        isSimpleType = propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.FullName.Equals(typeof(string).FullName);
                        for (int k = 0; k < availableRowCount; k++)
                        {
                            if (isSimpleType)
                            {
                                propertyValue = propertyInfo.GetValue(items.ElementAt(k + dataPosition), null);
                            }
                            else
                            {
                                propertyValue = propertyInfo.GetValue(items.ElementAt(k + dataPosition), null);
                                if (propertyValue != null)
                                {
                                    propertyValue = propertyValue.ToString();
                                }
                            }
                            arr.SetValue(propertyValue, k + 1, j);
                        }
                    }

                    xSheet.Range["A1"].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;
                    dataPosition += sheetPageSize;
                }

                xSheet.SaveAs(fileName);
            }
            finally
            {
                NExtendExcel.ExitExcellApp(excelApp);
            }
        }

        /// <summary>
        /// 追加IEnumerable数据到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="fileName"></param>
        /// <param name="fieldColHeadMap"></param>
        /// <param name="ignoreFields"></param>
        /// <param name="pageSize"></param>
        protected static void AppendObjectToExcel<T>(IEnumerable<T> items, string fileName, Dictionary<string, string> fieldColHeadMap, IEnumerable<string> ignoreFields, int pageSize = UInt16.MaxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 追加DataTable数据到Excel
        /// </summary>
        /// <param name="excelVer"></param>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="fieldColHeadMap"></param>
        /// <param name="ignoreCols"></param>
        /// <param name="pageSize"></param>
        protected static void AppendDatatableToExcel(bool excelVer, DataTable dt, string fileName, Dictionary<string, string> fieldColHeadMap = null, IEnumerable<string> ignoreCols = null, int pageSize = UInt16.MaxValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从数据库导出数据到Excel
        /// </summary>
        /// <param name="excelVer">Excel版本,true:2007以上的版本,false:2003及以下的版本</param>
        /// <param name="pageCount">数据页数</param>
        /// <param name="pageSize">查询数据页大小</param>
        /// <param name="queryAction">查询数据委托</param>
        /// <param name="fileName">Excell文件路径</param>
        /// <param name="colCount">查询结果列数</param>
        /// <param name="fieldColHeadMap">表字段列标题映射集合,无集合传null[key:数据库字段名(要求必须全部大写),value:Excel中显示的列标题]</param>
        /// <param name="ignoreCols">忽略的列集合(要求必须全部大写)</param>
        /// <param name="sheetPageSize">每页数据的大小(每个sheet里面存放数据的条数)[默认为最大值65535,每个sheet最大行数为65535,最大值为65534是因为列标题要占一行,1997-2003版本的xls中每个表单最大只支持65536行，2010可以支持1048576行]</param>
        public static void ExportDBToExcel(bool excelVer, int pageCount, int pageSize, Func<int, DataTable> queryAction, string fileName, int colCount, Dictionary<string, string> fieldColHeadMap = null, int sheetPageSize = UInt16.MaxValue)
        {
            if (queryAction == null)
            {
                throw new ArgumentException("查询委托不能为null");
            }

            if (pageCount < 1)
            {
                throw new ArgumentException(string.Format("pageCount值应为大于0,当前值:{0}无效", pageCount));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException(string.Format("pageSize值应为大于0,当前值:{0}无效", pageSize));
            }

            int rowMaxcount = 0;
            int colMaxcount = 0;
            string extention = Path.GetExtension(fileName);
            if (excelVer)
            {
                //2007以上的版本
                if (extention.ToLower().Equals(".xls"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}不是有效的2007及以上版本的扩展名", extention));
                }
                else if (!extention.ToLower().Equals(".xlsx"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}无效", extention));
                }

                rowMaxcount = 1048575;
                if (sheetPageSize > rowMaxcount)
                {
                    throw new ArgumentException(string.Format("单sheet数据页:{0}大小无效,Excell2007及以上的版本最大行数为:{1}", sheetPageSize, rowMaxcount));
                }

                colMaxcount = 16384;
                if (colCount > colMaxcount)
                {
                    throw new ArgumentException(string.Format("列数过多,不是能导出成2007及以上版本的Exce表格,当前列数:{0},2007及以上版本支持最大列数为:{1},无法导出", colCount, colMaxcount));
                }
            }
            else
            {
                //2003及以下的版本
                if (extention.ToLower().Equals(".xlsx"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}不是有效的2003及以下版本的扩展名", extention));
                }
                else if (!extention.ToLower().Equals(".xls"))
                {
                    throw new ArgumentException(string.Format("文件扩展名:{0}无效", extention));
                }

                rowMaxcount = UInt16.MaxValue;
                if (sheetPageSize > rowMaxcount)
                {
                    throw new ArgumentException(string.Format("单sheet数据页:{0}大小无效,Excell2003及以下的版本最大行数为:{1}", sheetPageSize, rowMaxcount));
                }

                colMaxcount = 256;
                if (colCount > colMaxcount)
                {
                    throw new ArgumentException(string.Format("列数过多,不是能导出成2003及以下版本的Exce表格,当前列数:{0},2003及以下版本支持最大列数为:{1},那上导出成2007及以上的格式", colCount, colMaxcount));
                }
            }

            NExtendExcel.ExportDBToExcel(pageCount, pageSize, queryAction, fileName, colCount, fieldColHeadMap, sheetPageSize);
        }
       
        /// <summary>
        /// 从数据库导出数据到Excel
        /// </summary>
        /// <param name="pageCount">数据页数</param>
        /// <param name="pageSize">查询数据页大小</param>
        /// <param name="queryAction">查询数据委托</param
        /// <param name="fileName">Excell文件路径</param>
        /// <param name="colCount">查询结果列数</param>
        /// <param name="fieldColHeadMap">表字段列标题映射集合,无集合传null[key:数据库字段名(要求必须全部大写),value:Excel中显示的列标题]</param>
        /// <param name="sheetPageSize">每页数据的大小(每个sheet里面存放数据的条数)[默认为最大值65535,每个sheet最大行数为65535,最大值为65534是因为列标题要占一行,1997-2003版本的xls中每个表单最大只支持65536行，2010可以支持1048576行]</param>
        private static void ExportDBToExcel(int pageCount, int pageSize, Func<int, DataTable> queryAction, string fileName, int colCount, Dictionary<string, string> fieldColHeadMap = null, int sheetPageSize = UInt16.MaxValue)
        {
            //创建 Excell工作簿
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                excelApp.Visible = false;//设置不可见
                excelApp.Workbooks.Add();//添加一个excell表格

                Microsoft.Office.Interop.Excel.Worksheet xSheet = null;//当前要写入数据的sheet
                int sheetIndex = 0;//当前写入数据的Sheet索引
                int sheetDataPosition = -1;//在集合中当前数据写入的位置

                DataTable dt = null;
                DataColumnCollection cols = null;//DT列集合
                string colName = string.Empty;//DT中当前操作的列名
                string upperColName = string.Empty;//DT中当前操作的列大写列名
                string colHead = string.Empty;//列标题
                object[,] arr = null;//每次写的数据块

                for (int pageIndex = 1; pageIndex <= pageCount; pageIndex++)
                {
                    dt = queryAction(pageIndex);
                    if (dt == null)
                    {
                        throw new Exception("查询结果为null");
                    }

                    if (dt.Columns.Count != colCount)
                    {
                        throw new Exception(string.Format("查询结果中的列数:{0}与期望列数:{1}不匹配", dt.Columns.Count, colCount));
                    }

                    if (pageIndex == 1)
                    {
                        cols = dt.Columns;
                    }

                    if (pageSize <= sheetPageSize)
                    {
                        #region less
                        if (sheetDataPosition == -1)
                        {
                            sheetIndex++;
                            if (sheetIndex > excelApp.Sheets.Count)
                            {
                                //excelApp.Sheets.Count:Worksheet总数,注:1997以前的版本上限为255个,1997(包含)及以后的版本中无上限,只要硬件够强
                                excelApp.Sheets.Add(Missing.Value, excelApp.Sheets[excelApp.Sheets.Count], Missing.Value, Missing.Value);
                            }

                            xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[sheetIndex]);
                            xSheet.Name = string.Format("第{0}页", sheetIndex);

                            arr = new object[dt.Rows.Count + 1, cols.Count];
                            for (int j = 0; j < colCount; j++)
                            {
                                colName = cols[j].ColumnName;
                                colHead = colName;
                                upperColName = colName.ToUpper();
                                if (fieldColHeadMap.ContainsKey(upperColName))
                                {
                                    colHead = fieldColHeadMap[upperColName];
                                }

                                arr.SetValue(colHead, 0, j);
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    arr.SetValue(dt.Rows[k][colName], k + 1, j);
                                }
                            }

                            xSheet.Range["A1"].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;
                            sheetDataPosition = arr.GetLength(0);
                        }
                        else
                        {
                            int availableCount = sheetPageSize - sheetDataPosition;
                            if (availableCount <= dt.Rows.Count)
                            {
                                arr = new object[availableCount, cols.Count];
                            }
                            else
                            {
                                arr = new object[dt.Rows.Count, cols.Count];
                            }

                            for (int j = 0; j < colCount; j++)
                            {
                                colName = cols[j].ColumnName;
                                for (int k = 0; k < arr.GetLength(0); k++)
                                {
                                    arr.SetValue(dt.Rows[k][colName], k, j);
                                }
                            }
                            xSheet.Range["A" + (sheetDataPosition + 1).ToString()].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;
                            if (availableCount > dt.Rows.Count)
                            {
                                sheetDataPosition += arr.GetLength(0);
                                continue;
                            }

                            sheetIndex++;
                            if (sheetIndex > excelApp.Sheets.Count)
                            {
                                //excelApp.Sheets.Count:Worksheet总数,注:1997以前的版本上限为255个,1997(包含)及以后的版本中无上限,只要硬件够强
                                excelApp.Sheets.Add(Missing.Value, excelApp.Sheets[excelApp.Sheets.Count], Missing.Value, Missing.Value);
                            }

                            xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[sheetIndex]);
                            xSheet.Name = string.Format("第{0}页", sheetIndex);
                            sheetDataPosition = -1;

                            arr = new object[dt.Rows.Count - availableCount + 1, cols.Count];
                            for (int j = 0; j < colCount; j++)
                            {
                                colName = cols[j].ColumnName;
                                colHead = colName;
                                upperColName = colName.ToUpper();
                                if (fieldColHeadMap.ContainsKey(upperColName))
                                {
                                    colHead = fieldColHeadMap[upperColName];
                                }

                                arr.SetValue(colHead, 0, j);
                                for (int k = availableCount; k < dt.Rows.Count; k++)
                                {
                                    arr.SetValue(dt.Rows[k][colName], k - availableCount + 1, j);
                                }
                            }

                            xSheet.Range["A1"].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;
                            sheetDataPosition = arr.GetLength(0);
                        }
                        #endregion
                    }
                    else
                    {
                        #region large
                        int rowPositionOffsetBegin = 0;
                        if (sheetDataPosition == -1)
                        {
                            int needSheetCount = dt.Rows.Count / sheetPageSize;
                            if (dt.Rows.Count % sheetPageSize != 0)
                            {
                                needSheetCount += 1;
                            }

                            for (int i = 1; i <= needSheetCount; i++)
                            {
                                //从0开始写
                                sheetIndex++;
                                if (sheetIndex > excelApp.Sheets.Count)
                                {
                                    //excelApp.Sheets.Count:Worksheet总数,注:1997以前的版本上限为255个,1997(包含)及以后的版本中无上限,只要硬件够强
                                    excelApp.Sheets.Add(Missing.Value, excelApp.Sheets[excelApp.Sheets.Count], Missing.Value, Missing.Value);
                                }

                                xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[sheetIndex]);
                                xSheet.Name = string.Format("第{0}页", sheetIndex);

                                if (i == needSheetCount)
                                {
                                    if (dt.Rows.Count % sheetPageSize == 0)
                                    {
                                        arr = new object[sheetPageSize + 1, cols.Count];
                                    }
                                    else
                                    {
                                        arr = new object[dt.Rows.Count % sheetPageSize + 1, cols.Count];
                                    }
                                }
                                else
                                {
                                    arr = new object[sheetPageSize + 1, cols.Count];
                                }

                                rowPositionOffsetBegin = (i - 1) * sheetPageSize;
                                for (int j = 0; j < colCount; j++)
                                {
                                    colName = cols[j].ColumnName;
                                    colHead = colName;
                                    upperColName = colName.ToUpper();
                                    if (fieldColHeadMap.ContainsKey(upperColName))
                                    {
                                        colHead = fieldColHeadMap[upperColName];
                                    }

                                    arr.SetValue(colHead, 0, j);
                                    for (int k = 0; k < arr.GetLength(0) - 1; k++)
                                    {
                                        arr.SetValue(dt.Rows[rowPositionOffsetBegin + k][colName], k + 1, j);
                                    }
                                }

                                xSheet.Range["A1"].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;

                                if (i == needSheetCount)
                                {
                                    sheetDataPosition = arr.GetLength(0);
                                    if (sheetDataPosition == sheetPageSize + 1)
                                    {
                                        sheetDataPosition = -1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //把上次没有写满的写满
                            int remainSheetRowsCount = sheetPageSize - sheetDataPosition + 1;
                            arr = new object[remainSheetRowsCount, cols.Count];
                            for (int i = 0; i < remainSheetRowsCount; i++)
                            {
                                for (int j = 0; j < colCount; j++)
                                {
                                    colName = cols[j].ColumnName;
                                    for (int k = 0; k < arr.GetLength(0); k++)
                                    {
                                        arr.SetValue(dt.Rows[k][colName], k, j);
                                    }
                                }
                            }

                            xSheet.Range["A" + (sheetDataPosition + 1).ToString()].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;

                            //写入剩下的
                            int remainRowCount = dt.Rows.Count - remainSheetRowsCount;
                            int needSheetCount = remainRowCount / sheetPageSize;
                            if (remainRowCount % sheetPageSize != 0)
                            {
                                needSheetCount += 1;
                            }

                            for (int i = 1; i <= needSheetCount; i++)
                            {
                                //从0开始写
                                sheetIndex++;
                                if (sheetIndex > excelApp.Sheets.Count)
                                {
                                    //excelApp.Sheets.Count:Worksheet总数,注:1997以前的版本上限为255个,1997(包含)及以后的版本中无上限,只要硬件够强
                                    excelApp.Sheets.Add(Missing.Value, excelApp.Sheets[excelApp.Sheets.Count], Missing.Value, Missing.Value);
                                }

                                xSheet = ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.Sheets[sheetIndex]);
                                xSheet.Name = string.Format("第{0}页", sheetIndex);

                                if (i == needSheetCount)
                                {
                                    if (remainRowCount % sheetPageSize == 0)
                                    {
                                        arr = new object[sheetPageSize + 1, cols.Count];
                                    }
                                    else
                                    {
                                        arr = new object[remainRowCount % sheetPageSize + 1, cols.Count];
                                    }
                                }
                                else
                                {
                                    arr = new object[sheetPageSize + 1, cols.Count];
                                }

                                rowPositionOffsetBegin = (i - 1) * sheetPageSize + remainSheetRowsCount;
                                for (int j = 0; j < colCount; j++)
                                {
                                    colName = cols[j].ColumnName;
                                    colHead = colName;
                                    upperColName = colName.ToUpper();
                                    if (fieldColHeadMap.ContainsKey(upperColName))
                                    {
                                        colHead = fieldColHeadMap[upperColName];
                                    }

                                    arr.SetValue(colHead, 0, j);
                                    for (int k = 0; k < arr.GetLength(0) - 1; k++)
                                    {
                                        arr.SetValue(dt.Rows[rowPositionOffsetBegin + k][colName], k + 1, j);
                                    }
                                }

                                xSheet.Range["A1"].Resize[arr.GetLength(0), arr.GetLength(1)].Value2 = arr;

                                if (i == needSheetCount)
                                {
                                    sheetDataPosition = arr.GetLength(0);
                                    if (sheetDataPosition == sheetPageSize + 1)
                                    {
                                        sheetDataPosition = -1;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                xSheet.SaveAs(fileName);
            }
            finally
            {
                NExtendExcel.ExitExcellApp(excelApp);
            }
        }
         */
    }
}