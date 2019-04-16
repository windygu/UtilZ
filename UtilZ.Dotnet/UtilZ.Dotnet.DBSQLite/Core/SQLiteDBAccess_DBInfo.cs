using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBModel.Common;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBSQLite.Core
{
    //SQLite数据库访问类-数据库信息相关
    public partial class SQLiteDBAccess
    {
        /// <summary>
        /// 判断表是否存在[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名[表名区分大小写的数据库:Oracle,SQLite]</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public override bool IsExistTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }

            //string sqlStr = @"SELECT COUNT(0) FROM sqlite_master where type='table' and name=@TABLENAME";
            //var paras = new NDbParameterCollection();
            //paras.Add("TABLENAME", tableName);
            //object value = this.ExecuteScalar(sqlStr, paras);
            //return Convert.ToInt32(value) > 0;
            string sqlStr = @"select tbl_name,'' from sqlite_master where type='table'";
            DataTable dt = this.QueryData(sqlStr);
            string dbTableName;
            foreach (DataRow row in dt.Rows)
            {
                dbTableName = row[0].ToString();
                if (tableName.Equals(dbTableName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断表中是否存在字段[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public override bool IsExistField(string tableName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName))
            {
                return false;
            }

            string sqlStr = string.Format(@"select * from {0} LIMIT 0,0", tableName);
            DataTable dt = this.QueryData(sqlStr, null);
            if (dt == null)
            {
                return false;
            }

            return dt.Columns.Contains(fieldName);
        }

        #region 获取表的字段信息
        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        protected override List<DBFieldInfo> InnerGetTableFieldInfos(IDbConnection con, string tableName)
        {
            DbConnectionInfo conInfo = null;
            if (con == null)
            {
                conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R);
                con = conInfo.Con;
            }

            try
            {
                //因为sqlite没有记录列信息的表,所以此处采用的是查询空表数据,然后从DataTable中获取列的信息
                string sqlStr = string.Format(@"select * from {0} LIMIT 0,0", tableName);
                DataTable dt = this.InnerQueryData(con, sqlStr);
                var dicFieldDbClrFieldType = this.GetFieldDbClrFieldType(tableName, dt.Columns);//字段的公共语言运行时类型字典集合
                var priKeyCols = this.InnerQueryPrikeyColumns(con, tableName);//主键列名集合
                List<DBFieldInfo> colInfos = new List<DBFieldInfo>();
                string fieldName;
                string dbTypeName;
                bool allowNull;
                object defaultValue;
                Type type;
                DBFieldType fieldType;

                foreach (DataColumn col in dt.Columns)
                {
                    fieldName = col.ColumnName;
                    dbTypeName = col.DataType.Name;
                    allowNull = col.AllowDBNull; ;
                    defaultValue = col.DefaultValue;
                    //comments:sqlite没有这一项
                    type = col.DataType;
                    fieldType = dicFieldDbClrFieldType[fieldName];
                    colInfos.Add(new DBFieldInfo(tableName, fieldName, dbTypeName, type, null, defaultValue, allowNull, fieldType, priKeyCols.Contains(fieldName)));
                }

                return colInfos;
            }
            finally
            {
                if (conInfo != null)
                {
                    conInfo.Dispose();
                }
            }
        }
        #endregion

        #region 获取表信息
        /// <summary>
        /// 获取当前用户有权限的所有表集合
        /// </summary>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>当前用户有权限的所有表集合</returns>
        public override List<DBTableInfo> GetTableInfos(bool isGetFieldInfo = false)
        {
            string sqlStr = @"select tbl_name,'' from sqlite_master where type='table'";
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                DataTable dt = this.InnerQueryData(conInfo.Con, sqlStr);
                List<DBTableInfo> tables = new List<DBTableInfo>();
                string tableName;

                foreach (DataRow row in dt.Rows)
                {
                    tableName = row[0].ToString();
                    if ("sqlite_sequence".Equals(tableName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    tables.Add(this.InnerGetTableInfo(conInfo.Con, tableName, isGetFieldInfo));
                }

                return tables;
            }
        }

        /// <summary>
        /// 查询主键列名集合
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>主键列名集合</returns>
        protected override List<string> InnerQueryPrikeyColumns(IDbConnection con, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            string sqlStr = string.Format(@"pragma table_info ('{0}')", tableName);
            DataTable dt = this.InnerQueryData(con, sqlStr);
            List<string> priKeyCols = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["pk"]) == 1)
                {
                    priKeyCols.Add(row[1].ToString());
                }
            }

            return priKeyCols;
        }

        /// <summary>
        /// 获取表信息[表不存在返回null]
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>表信息</returns>
        protected override DBTableInfo InnerGetTableInfo(IDbConnection con, string tableName, bool isGetFieldInfo = false)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            DbConnectionInfo conInfo = null;
            if (con == null)
            {
                conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R);
                con = conInfo.Con;
            }

            try
            {
                string sqlStr = @"select tbl_name from sqlite_master where type='table'";
                DataTable dt = this.InnerQueryData(con, sqlStr, null);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                List<DBFieldInfo> colInfos = null;//字段集合
                IEnumerable<DBFieldInfo> priKeyColInfos = null;//主键列字段集合
                string dbTableName;
                foreach (DataRow row in dt.Rows)
                {
                    dbTableName = row[0].ToString();
                    if (!tableName.Equals(dbTableName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (isGetFieldInfo)//获取字段信息
                    {
                        colInfos = this.InnerGetTableFieldInfos(con, tableName);//获取表所有字段集合
                        priKeyColInfos = from col in colInfos where col.IsPriKey select col;//获取主键列字段集合
                    }
                    else//不获取字段信息
                    {
                        colInfos = new List<DBFieldInfo>();
                        priKeyColInfos = new List<DBFieldInfo>();
                    }

                    return new DBTableInfo(tableName, string.Empty, new DBFieldInfoCollection(colInfos), new DBFieldInfoCollection(priKeyColInfos));
                }

                return null;
            }
            finally
            {
                if (conInfo != null)
                {
                    conInfo.Dispose();
                }
            }
        }
        #endregion

        #region 数据库表结构版本管理
        /// <summary>
        /// 获取创建数据库表结构版本号表sql语句
        /// </summary>
        /// <param name="dbVersionTableName">表名</param>
        /// <param name="dbStructVersionColName">版本列名</param>
        /// <returns>创建数据库表结构版本号表sql语句</returns>
        protected override string GetCreateDBVersionTableSql(string dbVersionTableName, string dbStructVersionColName)
        {
            return string.Format(@"CREATE TABLE {0} ({1} integer)", dbVersionTableName, dbStructVersionColName);
        }
        #endregion

        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns>数据库版本信息</returns>
        public override string GetDataBaseVersion()
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                string sqlStr = @"select sqlite_version();";
                object obj = this.InnerExecuteScalar(conInfo.Con, sqlStr);
                return obj == null ? string.Empty : obj.ToString();
            }
        }

        /// <summary>
        /// 获取数据库系统时间
        /// </summary>
        /// <returns>数据库系统时间</returns>
        public override DateTime GetDataBaseSysTime()
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                string sqlStr = @"select datetime('now','localtime')";
                return Convert.ToDateTime(this.InnerExecuteScalar(conInfo.Con, sqlStr));
            }
        }
    }
}
