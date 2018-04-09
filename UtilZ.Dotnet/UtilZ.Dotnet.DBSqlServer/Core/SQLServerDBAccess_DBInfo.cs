using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Factory;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Common;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBInfo;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model;

namespace UtilZ.Dotnet.DBSqlServer.Core
{
    //SQLServer数据库访问类-数据库信息相关
    public partial class SQLServerDBAccess
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

            //string sqlStr =@"select COUNT(0) from sysobjects where id = object_id('表名') and type = 'u'";
            //string sqlStr = @"select COUNT(0) from sys.tables where name='表名' and type = 'u';";
            string sqlStr = string.Format(@"select COUNT(0) from sys.tables where name={0}TABLENAME and type = 'u'", this.ParaSign);
            var paras = new NDbParameterCollection();
            paras.Add("TABLENAME", tableName);
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
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

            string sqlStr = @"select count(0) from syscolumns where name=@FIELDNAME and objectproperty(id,'IsUserTable')=1 and object_name(id)=@TABLENAME";
            var paras = new NDbParameterCollection();
            paras.Add("FIELDNAME", fieldName);
            paras.Add("TABLENAME", tableName);
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
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
                string sqlStr = string.Format("SELECT TOP 0 *  FROM {0}", tableName);
                DataTable dt = this.InnerQueryData(con, sqlStr);
                var dicFieldDbClrFieldType = this.GetFieldDbClrFieldType(tableName, dt.Columns);//字段的公共语言运行时类型字典集合
                Dictionary<string, Type> colDBType = new Dictionary<string, Type>();
                foreach (DataColumn col in dt.Columns)
                {
                    colDBType.Add(col.ColumnName, col.DataType);
                }

                IDbCommand cmd = this.CreateCommand(con);
                cmd.CommandText = string.Format(@"SELECT  C.name as [字段名]
	                                                    ,T.name as [字段类型]
                                                        ,convert(bit,C.IsNullable)  as [可否为空]
                                                        ,convert(bit,case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=c.id and name in (
                                                            SELECT name FROM sysindexes WHERE indid in(
                                                                SELECT indid FROM sysindexkeys WHERE id = c.id AND colid=c.colid))) then 1 else 0 end) 
                                                                    as [是否主键]
                                                        ,convert(bit,COLUMNPROPERTY(c.id,c.name,'IsIdentity')) as [自动增长]
                                                        ,C.Length as [占用字节] 
                                                        ,COLUMNPROPERTY(C.id,C.name,'PRECISION') as [长度]
                                                        ,isnull(COLUMNPROPERTY(c.id,c.name,'Scale'),0) as [小数位数]
                                                        ,ISNULL(CM.text,'') as [默认值]
                                                        ,isnull(ETP.value,'') AS [字段描述]
                                                        --,ROW_NUMBER() OVER (ORDER BY C.name) AS [Row]
                                                FROM syscolumns C
                                                INNER JOIN systypes T ON C.xusertype = T.xusertype 
                                                left JOIN sys.extended_properties ETP   ON  ETP.major_id = c.id AND ETP.minor_id = C.colid AND ETP.name ='MS_Description' 
                                                left join syscomments CM on C.cdefault=CM.id
                                                WHERE C.id = object_id('{0}')", tableName);

                List<DBFieldInfo> colInfos = new List<DBFieldInfo>();
                using (IDataReader reader = cmd.ExecuteReader())
                {
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
                        fieldName = reader.GetString(0);
                        dbTypeName = reader.GetString(1);
                        allowNull = reader.GetBoolean(2);
                        defaultValue = reader.GetValue(8);
                        comments = reader.GetString(9);
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
            //string sqlStr= @"select name from sysobjects where xtype='u'";
            string sqlStr = @"select c.name,cast(isnull(f.[value], '') as nvarchar(100)) as remark from sys.objects c left join sys.extended_properties f on f.major_id=c.object_id and f.minor_id=0 and f.class=1 where c.type='u'";
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

            //string sqlStr = @"select column_name as primarykey from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where Table_name='Person' and constraint_name like 'PK_%'";
            string sqlStr = string.Format(@"select column_name as primarykey from INFORMATION_SCHEMA.KEY_COLUMN_USAGE where Table_name='{0}'", tableName);
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
                //      string sqlStr = @"select name from sysobjects where xtype='u'";
                string sqlStr = string.Format(@"select c.name,cast(isnull(f.[value], '') as nvarchar(100)) as remark from sys.objects c left join sys.extended_properties f on f.major_id=c.object_id and f.minor_id=0 and f.class=1 where c.type='u' and c.name='{0}'", tableName);
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

        #region 数据库表结构版本管理
        /// <summary>
        /// 获取创建数据库表结构版本号表sql语句
        /// </summary>
        /// <param name="dbVersionTableName">表名</param>
        /// <param name="dbStructVersionColName">版本列名</param>
        /// <returns>创建数据库表结构版本号表sql语句</returns>
        protected override string GetCreateDBVersionTableSql(string dbVersionTableName, string dbStructVersionColName)
        {
            return string.Format(@"CREATE TABLE {0} ({1} int)", dbVersionTableName, dbStructVersionColName);
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
                string sqlStr = @"select @@version";
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
            string sqlStr = @"select GETDATE()";
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.R))
            {
                return Convert.ToDateTime(this.InnerExecuteScalar(conInfo.Con, sqlStr));
            }
        }
    }
}
