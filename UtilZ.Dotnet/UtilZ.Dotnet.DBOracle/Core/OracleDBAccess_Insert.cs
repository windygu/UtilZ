using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Core;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Factory;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBInfo;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBOracle.Core
{
    //SQLServer数据库访问类-插入泛型数据
    public partial class OracleDBAccess
    {
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <param name="rowCount">行数</param>
        private long OracleBatchInsert(IDbConnection con, string tableName, IEnumerable<string> cols, List<object> data, int rowCount)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            if (string.IsNullOrWhiteSpace(tableName) ||
                cols == null || cols.Count() == 0 ||
                data == null || data.Count() == 0)
            {
                return 0;
            }

            //插入的SQL
            StringBuilder sbCmdText = new StringBuilder();
            sbCmdText.AppendFormat("INSERT INTO {0}(", tableName);
            sbCmdText.Append(string.Join(",", cols));
            sbCmdText.Append(") VALUES (");
            sbCmdText.Append(":" + string.Join(",:", cols));
            sbCmdText.Append(")");

            bool closeFlag = false;
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
                closeFlag = true;
            }

            OracleCommand cmd = this.CreateCommand(con) as OracleCommand;
            //绑定批处理的行数
            cmd.ArrayBindCount = rowCount;
            cmd.BindByName = true;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sbCmdText.ToString();

            //创建参数
            List<IDbDataParameter> cacher = new List<IDbDataParameter>();
            OracleParameter parameter;
            for (int colIndex = 0; colIndex < data.Count; colIndex++)
            {
                parameter = new OracleParameter();
                parameter.ParameterName = cols.ElementAt(colIndex);
                parameter.Value = data[colIndex];
                parameter.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(parameter);
            }

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (closeFlag)
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">DataTable</param>
        /// <returns>返回受影响的行数</returns>
        public override long BatchInsert(string tableName, DataTable dt)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName");
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return 0;
            }

            List<string> cols = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                cols.Add(col.ColumnName);
            }

            //二维数组数据倒置行列转换
            int rowCount = dt.Rows.Count;
            int colCount = dt.Columns.Count;
            List<object> data = new List<object>(colCount);
            Array newRowData;
            for (int col = 0; col < colCount; col++)
            {
                newRowData = Array.CreateInstance(dt.Columns[col].DataType, rowCount);
                for (int row = 0; row < rowCount; row++)
                {
                    newRowData.SetValue(dt.Rows[row].ItemArray[col], row);
                }

                data.Add(newRowData);
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                return this.OracleBatchInsert(conInfo.Con, tableName, cols, data, rowCount);
            }
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>返回受影响的行数</returns>
        protected override long InnerBatchInsert(IDbConnection con, string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
        {
            if (cols.Count() == 0 || data == null || data.Count() == 0)
            {
                return 0L;
            }

            int colCount = cols.Count();
            //二维数组倒置，行列转换
            List<object> newData = new List<object>(colCount);
            int rowCount = data.Count();

            DBTableInfo dbTableInfo = CacheManager.GetDBTableInfo(con, this, tableName);
            Array newRowData;
            string colName;
            DBFieldInfo dbFieldInfo;
            object value;
            for (int i = 0; i < colCount; i++)
            {
                colName = cols.ElementAt(i).ToUpper();
                if (!dbTableInfo.DbFieldInfos.Contains(colName))
                {
                    throw new ApplicationException(string.Format("表{0}中不包含字段{1}", tableName, cols.ElementAt(i)));
                }

                dbFieldInfo = dbTableInfo.DbFieldInfos[colName];
                newRowData = Array.CreateInstance(dbFieldInfo.DataType, rowCount);
                for (var j = 0; j < rowCount; j++)
                {
                    value = data.ElementAt(j)[i];
                    if (value != DBNull.Value && value.GetType() != dbFieldInfo.DataType)
                    {
                        value = ConvertEx.ToObject(dbFieldInfo.DataType, value);
                    }

                    newRowData.SetValue(value, j);
                }

                newData.Add(newRowData);
            }

            return this.OracleBatchInsert(con, tableName, cols, newData, rowCount);
        }
    }
}
