using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.IWindowEx.Winform.PageGrid
{
    /// <summary>
    /// 表格控件辅助类
    /// </summary>
    public class PageGridHelper
    {
        /// <summary>
        /// 获取行数据
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>行数据</returns>
        public static object GetRowValue(object row)
        {
            object value = row;
            if (value is System.Data.DataRowView)
            {
                value = ((System.Data.DataRowView)value).Row;
            }

            return value;
        }

        /// <summary>
        /// 修改DataGridView指定列为DataGridViewComboBoxColumn
        /// </summary>
        /// <param name="dgv">表格控件</param>
        /// <param name="colName">列名</param>
        /// <param name="headText">列标题单元格的标题文本</param>
        /// <param name="valueType">列单元格中值的数据类型</param>
        /// <param name="dataSource"></param>
        public static void ChangeColumnToComboBoxColumn(System.Windows.Forms.DataGridView dgv, string colName, string headText, Type valueType, object dataSource)
        {
            if (dgv == null)
            {
                throw new ArgumentNullException("表格控件不能为null", "dgv");
            }

            if (string.IsNullOrEmpty(colName))
            {
                throw new ArgumentNullException("列名不能为空或null", "colName");
            }

            if (!dgv.Columns.Contains(colName))
            {
                throw new ArgumentException(string.Format("表格控件{0}中不包含列{1}", dgv.Name, colName), "colName");
            }

            if (string.IsNullOrEmpty(headText))
            {
                headText = colName;
            }

            if (valueType == null)
            {
                throw new ArgumentNullException("列单元格中值的数据类型", "valueType");
            }

            int index = dgv.Columns[colName].Index;
            //移除旧的列
            dgv.Columns.Remove(colName);

            var cobCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            cobCol.DataSource = dataSource;
            cobCol.HeaderText = headText;
            cobCol.Name = colName;
            cobCol.ValueType = valueType;
            cobCol.DataPropertyName = colName;
            //插入新的列
            dgv.Columns.Insert(index, cobCol);
        }

        /// <summary>
        /// 验证两个数据源是否是相同的数据
        /// </summary>
        /// <param name="dataSourceName1">数据源1名称</param>
        /// <param name="dataSourceName2">数据源2名称</param>
        /// <param name="dataSource1">数据源1</param>
        /// <param name="dataSource2">数据源2</param>
        /// <returns></returns>
        public static bool ValidateSampleData(string dataSourceName1, string dataSourceName2, object dataSource1, object dataSource2)
        {
            if (string.IsNullOrEmpty(dataSourceName1) && string.IsNullOrEmpty(dataSourceName2))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(dataSourceName1) && string.IsNullOrEmpty(dataSourceName2) ||
                string.IsNullOrEmpty(dataSourceName1) && !string.IsNullOrEmpty(dataSourceName2))
            {
                return false;
            }

            if (!dataSourceName1.Equals(dataSourceName2))
            {
                return false;
            }

            if (dataSource1 == dataSource2)
            {
                return true;
            }

            if (dataSource1 == null && dataSource2 != null ||
                dataSource1 != null && dataSource2 == null)
            {
                return false;
            }

            if (dataSource1.GetType() != dataSource2.GetType())
            {
                return false;
            }

            if (dataSource1 is DataTable)
            {
                DataTable dt1 = dataSource1 as DataTable;
                DataTable dt2 = dataSource2 as DataTable;
                if (dt1 == dt2)
                {
                    return true;
                }

                if (dt1 == null && dt2 != null ||
                    dt1 != null && dt2 == null)
                {
                    return false;
                }

                if (dt1.Columns.Count != dt2.Columns.Count)
                {
                    return false;
                }

                DataColumn col1 = null;
                //遍历列判断类型
                foreach (DataColumn col2 in dt2.Columns)
                {
                    //如果新数据源列名在老数据源中不包含,返回true
                    if (!dt1.Columns.Contains(col2.ColumnName))
                    {
                        return false;
                    }

                    col1 = dt1.Columns[col2.ColumnName];
                    //如果新数据源列数据类型与老数据源中列数据类型不同,返回true
                    if (col1.DataType != col2.DataType)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void UpdateData(DataTable dt1, DataTable dt2)
        {

        }
    }
}
