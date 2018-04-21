using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Core;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBSqlServer.Core
{
    //SQLServer数据库访问类-插入泛型数据
    public partial class SQLServerDBAccess
    {
        /// <summary>
        /// SQLServer批量插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">数据表</param>
        /// <param name="con">数据库连接对象</param>
        private void SqlBulkCopyInsert(string tableName, DataTable dt, IDbConnection con)
        {
            string cacheKey = string.Format("{0}{1}", this.Config.DBID, tableName);
            DataTable tempTable = CacheManager.GetCacheData(cacheKey) as DataTable;
            if (tempTable == null)
            {
                tempTable = this.InnerQueryData(con, string.Format("select top 0 * from {0}", tableName));
                CacheManager.StoreCacheData(cacheKey, tempTable, CacheManager.CacheTime);
            }

            DataTable insertTable = tempTable.Clone();
            DataRow newRow;
            foreach (DataRow dataRow in dt.Rows)
            {
                newRow = insertTable.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    if (!tempTable.Columns.Contains(col.ColumnName))
                    {
                        throw new ApplicationException(string.Format("表{0}中不包含字段{1}", tableName, col));
                    }

                    newRow[col.ColumnName] = dataRow[col];
                }

                insertTable.Rows.Add(newRow);
            }

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy((SqlConnection)con))
            {
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.BatchSize = insertTable.Rows.Count;
                sqlBulkCopy.WriteToServer(insertTable);
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
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return 0;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return 0;
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                this.SqlBulkCopyInsert(tableName, dt, conInfo.Con);
                return dt.Rows.Count;
            }
        }

        /// <summary>
        /// 数据表字典集合[key:表名;value:DataTable]
        /// </summary>
        private readonly ConcurrentDictionary<string, DataTable> _dicDataTables = new ConcurrentDictionary<string, DataTable>();

        /// <summary>
        /// 数据库连接写对象字典集合线程锁
        /// </summary>
        private readonly object _dicDataTablesMonitor = new object();

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
            if (cols == null || cols.Count() == 0 || data == null || data.Count() == 0)
            {
                return 0L;
            }

            DataTable dt;
            if (this._dicDataTables.ContainsKey(tableName))
            {
                if (!this._dicDataTables.TryGetValue(tableName, out dt))
                {
                    lock (this._dicDataTablesMonitor)
                    {
                        this._dicDataTables.TryRemove(tableName, out dt);
                        dt = this.InnerQueryData(con, string.Format(@"select {0} from {1} where0=1", string.Join(",", cols), tableName));
                        this._dicDataTables.TryAdd(tableName, dt);
                    }
                }
            }
            else
            {
                lock (this._dicDataTablesMonitor)
                {
                    if (this._dicDataTables.ContainsKey(tableName))
                    {
                        if (!this._dicDataTables.TryGetValue(tableName, out dt))
                        {
                            this._dicDataTables.TryRemove(tableName, out dt);
                            dt = this.InnerQueryData(con, string.Format(@"select top 0 {0} from {1}", string.Join(",", cols), tableName));
                            this._dicDataTables.TryAdd(tableName, dt);
                        }
                    }
                    else
                    {
                        dt = this.InnerQueryData(con, string.Format(@"select top 0 {0} from {1}", string.Join(",", cols), tableName));
                        this._dicDataTables.TryAdd(tableName, dt);
                    }
                }
            }

            dt = dt.Clone();
            DataRow row;
            int rowCount = data.Count();
            int colCount = cols.Count();
            object[] value;
            for (int r = 0; r < rowCount; r++)
            {
                row = dt.NewRow();
                value = data.ElementAt(r);
                for (int c = 0; c < colCount; c++)
                {
                    row[cols.ElementAt(c)] = value[c];

                }

                dt.Rows.Add(row);
            }

            this.SqlBulkCopyInsert(tableName, dt, con);
            return rowCount;
        }
    }
}
