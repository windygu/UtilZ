using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UtilZ.Lib.Winform.Extend
{
    /// <summary>
    /// DataGridView扩展类
    /// </summary>
    public static class NExtendDataGridView
    {
        /// <summary>
        /// DataGridView绑定数据
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="hidenColumns">隐藏列集合</param>
        /// <param name="colHeadInfos">列标题映射字典[key:列名;value:列标题;默认为null]</param>
        /// <param name="allowEditColumns">允许编辑的列集合[当为null或空时,全部列都可编辑;默认为null]</param>
        public static void DataBinding(this DataGridView dgv, object dataSource, IEnumerable<string> hidenColumns = null, Dictionary<string, string> colHeadInfos = null, IEnumerable<string> allowEditColumns = null)
        {
            if (dgv == null)
            {
                throw new ArgumentNullException("dgv");
            }

            if (dgv.DataSource == dataSource)
            {
                return;
            }

            dgv.DataSource = dataSource;
            if (dataSource == null)
            {
                return;
            }

            if (hidenColumns == null)
            {
                hidenColumns = new List<string>();
            }

            if (colHeadInfos == null)
            {
                colHeadInfos = new Dictionary<string, string>();
            }

            if (allowEditColumns == null)
            {
                allowEditColumns = new List<string>();
            }

            string caption = null;
            string fieldName = null;
            bool isReadOnly;
            var dt = dgv.DataSource as System.Data.DataTable;
            dgv.ReadOnly = allowEditColumns.Count() == 0;
            foreach (DataGridViewColumn gridColumn in dgv.Columns)
            {
                //获取字段名
                fieldName = gridColumn.Name;
                if (hidenColumns.Contains(fieldName))
                {
                    gridColumn.Visible = false;
                    break;
                }

                isReadOnly = !allowEditColumns.Contains(fieldName);
                //设置为可编辑性
                if (isReadOnly != gridColumn.ReadOnly)
                {
                    gridColumn.ReadOnly = isReadOnly;
                }

                //设置显示标题
                if (colHeadInfos.ContainsKey(fieldName))
                {
                    caption = colHeadInfos[fieldName];
                }
                else if (dt != null && dt.Columns.Contains(fieldName))
                {
                    caption = dt.Columns[fieldName].Caption;
                }

                if (!string.IsNullOrEmpty(caption))
                {
                    gridColumn.HeaderText = caption;
                    caption = null;
                }
            }
        }
    }
}
