using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBBase.Core;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.DBIBase.DBModel.Common;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBMySql.Core
{
    //MySql数据库访问类-数据库信息相关
    public partial class MySqlDBAccess
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

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                // string sqlStr =@"SHOW TABLES LIKE '表名'";   -- 当前连接库中是否存在表
                // string sqlStr = @"select Count(0) from INFORMATION_SCHEMA.TABLES where TABLE_NAME='表名' AND TABLE_SCHEMA='数据库名'";
                string sqlStr = string.Format(@"select Count(0) from INFORMATION_SCHEMA.TABLES where TABLE_NAME=?TABLENAME AND TABLE_SCHEMA='{0}'", conInfo.Connection.Database);
                var paras = new NDbParameterCollection();
                paras.Add("TABLENAME", tableName);
                object value = base.InnerExecuteScalar(conInfo.Connection, sqlStr, paras);
                return Convert.ToInt32(value) > 0;
            }
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

            //string sqlStr = string.Format(@"SHOW COLUMNS FROM {0}TABLENAME where Field={0}FIELDNAME", dbParaSign);
            string sqlStr = string.Format(@"select count(0) from information_schema.columns WHERE table_name =?TABLENAME and column_name=?FIELDNAME");
            var paras = new NDbParameterCollection();
            paras.Add("FIELDNAME", fieldName);
            paras.Add("TABLENAME", tableName);
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                return Convert.ToInt64(base.InnerExecuteScalar(conInfo.Connection, sqlStr, paras)) > 0;
            }
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
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            DbConnectionInfo conInfo = null;
            if (con == null)
            {
                conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R);
                con = conInfo.Connection;
            }

            try
            {
                var priKeyCols = this.InnerQueryPrikeyColumns(con, tableName);//主键列名集合
                DataTable dt = this.InnerQueryData(con, string.Format("SELECT * FROM {0} limit 0,0", tableName));
                var dicFieldDbClrFieldType = this.GetFieldDbClrFieldType(tableName, dt.Columns);//字段的公共语言运行时类型字典集合

                //查询表C#中列信息,从空表中获得
                Dictionary<string, Type> colDBType = new Dictionary<string, Type>();
                foreach (DataColumn col in dt.Columns)
                {
                    colDBType.Add(col.ColumnName, col.DataType);
                }


                IDbCommand cmd = con.CreateCommand();
                cmd.CommandText = string.Format(@"SHOW FULL FIELDS FROM {0}", tableName);
                List<DBFieldInfo> colInfos = new List<DBFieldInfo>();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    object value;
                    string fieldName;
                    string dbTypeName;
                    bool allowNull;
                    object defaultValue;
                    string comments;
                    Type type;
                    DBFieldType fieldType;
                    string caption = null;
                    string description = null;

                    while (reader.Read())
                    {
                        fieldName = reader["Field"].ToString();
                        dbTypeName = reader["Type"].ToString();
                        allowNull = reader["Null"].ToString().ToUpper().Equals("NO") ? false : true;
                        value = reader["Default"];
                        defaultValue = DBNull.Value.Equals(value) ? null : value;
                        comments = reader["Comment"].ToString();
                        type = colDBType[fieldName];
                        fieldType = dicFieldDbClrFieldType[fieldName];
                        DBHelper.ParseComments(fieldName, comments, out caption, out description);
                        colInfos.Add(new DBFieldInfo(tableName, caption, description, fieldName, dbTypeName, type, comments, defaultValue, allowNull, fieldType, priKeyCols.Contains(fieldName)));
                    }
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
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                string sqlStr = string.Format(@"select TABLE_NAME,TABLE_COMMENT from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='{0}'", conInfo.Connection.Database);
                DataTable dt = this.InnerQueryData(conInfo.Connection, sqlStr);
                List<DBTableInfo> tables = new List<DBTableInfo>();
                foreach (DataRow row in dt.Rows)
                {
                    tables.Add(this.InnerGetTableInfo(conInfo.Connection, row[0].ToString(), isGetFieldInfo));
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

            string sqlStr = string.Format(@"select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where table_name='{0}' AND COLUMN_KEY='PRI'", tableName);
            DataTable dt = this.InnerQueryData(con, sqlStr);
            List<string> priKeyCols = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                priKeyCols.Add(row[0].ToString()); return priKeyCols;
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
                con = conInfo.Connection;
            }

            try
            {
                string sqlStr = string.Format(@"select TABLE_NAME,TABLE_COMMENT from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='{0}' AND TABLE_NAME = '{1}'", con.Database, tableName);
                DataTable dt = this.InnerQueryData(con, sqlStr, null);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }

                string comments;//备注
                object tmpValue = null;//临时变量
                List<DBFieldInfo> colInfos = null;//字段集合
                IEnumerable<DBFieldInfo> priKeyColInfos = null;//主键列字段集合
                foreach (DataRow row in dt.Rows)
                {
                    tableName = row[0].ToString();
                    tmpValue = row[1];
                    if (tmpValue != null)
                    {
                        comments = tmpValue.ToString();
                    }
                    else
                    {
                        comments = null;
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

                    return new DBTableInfo(tableName, comments, new DBFieldInfoCollection(colInfos), new DBFieldInfoCollection(priKeyColInfos));
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

        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns>数据库版本信息</returns>
        public override string GetDataBaseVersion()
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                string sqlStr = @"select version()";
                object obj = base.InnerExecuteScalar(conInfo.Connection, sqlStr);
                return obj == null ? string.Empty : obj.ToString();
            }
        }

        /// <summary>
        /// 获取数据库系统时间
        /// </summary>
        /// <returns>数据库系统时间</returns>
        public override DateTime GetDataBaseSysTime()
        {
            string sqlStr = @"select CURRENT_TIMESTAMP()";
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                return Convert.ToDateTime(base.InnerExecuteScalar(conInfo.Connection, sqlStr));
            }
        }
    }
}
