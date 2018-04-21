using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBModel.Common;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBOracle.Core
{
    //SQLServer数据库访问类-数据库信息相关
    public partial class OracleDBAccess
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
                //string sqlStr = @"select count(0) from tabs where table_name ='表名'";
                //string sqlStr= @"select count(0) into tableExistedCount from user_tables t where t.table_name = upper('表名')";// --从系统表中查询当表是否存在
                //string sqlStr = string.Format(@"select count(0) into tableExistedCount from user_tables t where t.table_name = upper({0}TABLENAME)", dbParaSign);
                tableName = tableName.ToUpper();
                string sqlStr = @"select count(0) from tabs where TABLE_NAME =:TABLENAME";
                var paras = new NDbParameterCollection();
                paras.Add("TABLENAME", tableName);
                object value = this.InnerExecuteScalar(conInfo.Con, sqlStr, paras);
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

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                string sqlStr = @"SELECT count(0) FROM USER_TAB_COLUMNS WHERE TABLE_NAME = :TABLENAME AND COLUMN_NAME = :FIELDNAME";
                var paras = new NDbParameterCollection();
                paras.Add("TABLENAME", tableName.ToUpper());
                paras.Add("FIELDNAME", fieldName.ToUpper());
                return Convert.ToInt64(this.InnerExecuteScalar(conInfo.Con, sqlStr, paras)) > 0;
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
                con = conInfo.Con;
            }

            try
            {
                var priKeyCols = this.InnerQueryPrikeyColumns(con, tableName);//主键列名集合
                string sqlStr = string.Format("SELECT * FROM (SELECT A.*, ROWNUM RN FROM (SELECT * FROM {0}) A WHERE ROWNUM <= 1) WHERE RN >=0", tableName);
                DataTable dt = this.InnerQueryData(con, sqlStr);
                var dicFieldDbClrFieldType = this.GetFieldDbClrFieldType(tableName, dt.Columns);//字段的公共语言运行时类型字典集合
                Dictionary<string, Type> colDBType = new Dictionary<string, Type>();
                //查询表C#中列信息
                foreach (DataColumn col in dt.Columns)
                {
                    colDBType.Add(col.ColumnName, col.DataType);
                }

                IDbCommand cmd = this.CreateCommand(con);
                cmd.CommandText = string.Format(@"select t.COLUMN_NAME,t.DATA_TYPE,t.NULLABLE,t.DATA_DEFAULT,c.COMMENTS from user_tab_columns t,user_col_comments c where t.table_name = c.table_name and t.column_name = c.column_name and t.table_name = '{0}'", tableName.ToUpper());

                List<DBFieldInfo> colInfos = new List<DBFieldInfo>();
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
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fieldName = reader.GetString(0);
                        dbTypeName = reader.GetString(1);
                        allowNull = reader.GetString(2).ToUpper().Equals("Y") ? true : false;
                        defaultValue = reader.GetValue(3);
                        value = reader[4];
                        if (value != null)
                        {
                            comments = value.ToString();
                            DBHelper.ParseComments(fieldName, comments, out caption, out description);
                        }
                        else
                        {
                            comments = string.Empty;
                        }

                        type = colDBType[fieldName];
                        fieldType = dicFieldDbClrFieldType[fieldName];
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
            string sqlStr = @"SELECT TABLE_NAME,COMMENTS FROM USER_TAB_COMMENTS WHERE TABLE_TYPE='TABLE'";
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                DataTable dt = this.InnerQueryData(conInfo.Con, sqlStr);
                List<DBTableInfo> tables = new List<DBTableInfo>();
                foreach (DataRow row in dt.Rows)
                {
                    tables.Add(this.InnerGetTableInfo(conInfo.Con, row[0].ToString(), isGetFieldInfo));
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

            //string sqlStr = @"select cu.* from user_cons_columns cu, user_constraints au where cu.constraint_name = au.constraint_name and au.constraint_type = 'P' and au.table_name = 'PERSON'";
            string sqlStr = string.Format(@"select cu.column_name from user_cons_columns cu, user_constraints au where cu.constraint_name = au.constraint_name and au.constraint_type = 'P' and au.table_name = '{0}'", tableName.ToUpper());
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
                con = conInfo.Con;
            }

            try
            {
                string sqlStr = string.Format(@"SELECT TABLE_NAME,COMMENTS FROM USER_TAB_COMMENTS WHERE TABLE_TYPE='TABLE' AND TABLE_NAME='{0}'", tableName.ToUpper());
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

                    if (isGetFieldInfo)
                    {
                        //获取字段信息
                        colInfos = this.InnerGetTableFieldInfos(con, tableName);//获取表所有字段集合
                        priKeyColInfos = from col in colInfos where col.IsPriKey select col;//获取主键列字段集合
                    }
                    else
                    {
                        //不获取字段信息
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

        #region 数据库表结构版本管理
        /// <summary>
        /// 获取创建数据库表结构版本号表sql语句
        /// </summary>
        /// <param name="dbVersionTableName">表名</param>
        /// <param name="dbStructVersionColName">版本列名</param>
        /// <returns>创建数据库表结构版本号表sql语句</returns>
        protected override string GetCreateDBVersionTableSql(string dbVersionTableName, string dbStructVersionColName)
        {
            return string.Format(@"CREATE TABLE {0} ({1} NUMBER(4))", dbVersionTableName, dbStructVersionColName);
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
                //string sqlStr = @"select * from product_component_version";
                string sqlStr = @"select * from v$version";
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
                string sqlStr = @"select current_date from dual";
                return Convert.ToDateTime(this.InnerExecuteScalar(conInfo.Con, sqlStr));
            }
        }
    }
}
