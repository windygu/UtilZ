using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Common;
using UtilZ.Lib.DBModel.DBInfo;
using UtilZ.Lib.DBModel.Model;
using UtilZ.Lib.Base;

namespace UtilZ.Lib.DBBase.Core
{
    //数据库访问基类-DBInfo
    public abstract partial class DBAccessBase
    {
        /// <summary>
        /// 判断表是否存在[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名[表名区分大小写的数据库:Oracle,SQLite]</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public abstract bool IsExistTable(string tableName);

        /// <summary>
        /// 判断表中是否存在字段[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public abstract bool IsExistField(string tableName, string fieldName);

        /// <summary>
        /// 获取表二进制字段名称集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        public List<string> GetTableBinaryFieldInfo(string tableName)
        {
            string sqlStr = string.Format("select * from {0} where 0=1", tableName);
            List<string> binaryCols = new List<string>();
            DataTable dt = this.QueryData(sqlStr);
            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == NClrSystemType.BytesType)
                {
                    binaryCols.Add(col.ColumnName);
                }
            }

            return binaryCols;
        }

        #region 获取表的字段信息
        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        public List<DBFieldInfo> GetTableFieldInfos(string tableName)
        {
            return this.InnerGetTableFieldInfos(null, tableName);
        }

        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        protected abstract List<DBFieldInfo> InnerGetTableFieldInfos(IDbConnection con, string tableName);

        /// <summary>
        /// 获取字段的公共语言运行时类型字典集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列集合</param>
        /// <returns>字段的公共语言运行时类型字典集合</returns>
        public Dictionary<string, DBFieldType> GetFieldDbClrFieldType(string tableName, DataColumnCollection cols)
        {
            Dictionary<string, DBFieldType> dicFieldDbClrFieldType = new Dictionary<string, DBFieldType>();
            foreach (DataColumn col in cols)
            {
                dicFieldDbClrFieldType.Add(col.ColumnName, DBHelper.GetDbClrFieldType(col.DataType));
            }

            return dicFieldDbClrFieldType;
        }
        #endregion

        #region 获取表信息
        /// <summary>
        /// 获取当前用户有权限的所有表集合
        /// </summary>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>当前用户有权限的所有表集合</returns>
        public abstract List<DBTableInfo> GetTableInfos(bool isGetFieldInfo = false);

        /// <summary>
        /// 查询主键列名集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主键列名集合</returns>
        public List<string> QueryPrikeyColumns(string tableName)
        {
            return this.InnerQueryPrikeyColumns(null, tableName);
        }

        /// <summary>
        /// 查询主键列名集合
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>主键列名集合</returns>
        protected abstract List<string> InnerQueryPrikeyColumns(IDbConnection con, string tableName);

        /// <summary>
        /// 获取表信息[表不存在返回null]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>表信息</returns>
        public DBTableInfo GetTableInfo(string tableName, bool isGetFieldInfo = false)
        {
            return this.InnerGetTableInfo(null, tableName, isGetFieldInfo);
        }

        /// <summary>
        /// 获取表信息[表不存在返回null]
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>表信息</returns>
        internal protected abstract DBTableInfo InnerGetTableInfo(IDbConnection con, string tableName, bool isGetFieldInfo = false);
        #endregion

        #region 数据库表结构版本管理
        /// <summary>
        /// 获取或设置数据库表结构版本号表名
        /// </summary>
        private string _dbVersionTableName = "DBStructVersion";

        /// <summary>
        /// 获取或设置数据库表结构版本号表名
        /// </summary>
        public string DBStructVersionTableName
        {
            get { return _dbVersionTableName; }
            set { _dbVersionTableName = value; }
        }

        /// <summary>
        /// 数据库表结构版本号表名
        /// </summary>
        private readonly string _dbStructVersionColName = "Version";

        /// <summary>
        /// 获取当前数据库表结构版本号,如果该表不存在则返回-1
        /// </summary>
        /// <returns>当前数据库表结构版本号</returns>
        public int GetDBVersion()
        {
            string dbVersionTableName = this.DBStructVersionTableName;
            if (this.IsExistTable(dbVersionTableName))
            {
                string sqlStr = string.Format(@"select {0} from {1}", this._dbStructVersionColName, dbVersionTableName);
                object obj = this.ExecuteScalar(sqlStr, DBVisitType.R);
                return Convert.ToInt32(obj);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 更新当前数据库表结构版本号[如果表存在，则更新为新版本号值,否则创建表,并设置版本号为新版本号值]
        /// </summary>
        public void UpdateDBVersion(int newVersion)
        {
            string dbVersionTableName = this.DBStructVersionTableName;
            string dbStructVersionColName = this._dbStructVersionColName;
            if (this.IsExistTable(dbVersionTableName))
            {
                //表存在，则更新为新版本号值
                string sqlStr = string.Format(@"update {0} set {1} = {2}", dbVersionTableName, dbStructVersionColName, newVersion);
                this.ExecuteScalar(sqlStr, DBVisitType.W);
            }
            else
            {
                //表不存在，创建表,并设置版本号为新版本号值
                dynamic para = new System.Dynamic.ExpandoObject();
                para.DBVersionTableName = dbVersionTableName;
                para.DBStructVersionColName = dbStructVersionColName;
                para.DefaultVersion = newVersion;
                this.ExcuteAdoNetTransaction(para, new Func<IDbConnection, IDbTransaction, object, object>(this.TransactionCreateDBVersionTable));
            }
        }

        /// <summary>
        /// 事务创建数据库表结构版本号表
        /// </summary>
        /// <param name="con">数据库写连接</param>
        /// <param name="transaction">事务对象</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        private object TransactionCreateDBVersionTable(IDbConnection con, IDbTransaction transaction, object p)
        {
            dynamic para = p;
            //创建表
            IDbCommand cmdCreateTable = this.CreateCommand(con);
            cmdCreateTable.Transaction = transaction;
            cmdCreateTable.CommandText = this.GetCreateDBVersionTableSql(para.DBVersionTableName, para.DBStructVersionColName);
            cmdCreateTable.ExecuteNonQuery();

            //插入版本数据
            IDbCommand cmdInsertDefaultVersion = this.CreateCommand(con);
            cmdInsertDefaultVersion.Transaction = transaction;
            cmdInsertDefaultVersion.CommandText = string.Format(@"insert into {0} ({1}) values ({2})", para.DBVersionTableName, para.DBStructVersionColName, para.DefaultVersion);
            cmdInsertDefaultVersion.ExecuteNonQuery();
            return null;
        }

        /// <summary>
        /// 获取创建数据库表结构版本号表sql语句
        /// </summary>
        /// <param name="dbVersionTableName">表名</param>
        /// <param name="dbStructVersionColName">版本列名</param>
        /// <returns>创建数据库表结构版本号表sql语句</returns>
        protected abstract string GetCreateDBVersionTableSql(string dbVersionTableName, string dbStructVersionColName);
        #endregion

        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns>数据库版本信息</returns>
        public abstract string GetDataBaseVersion();

        /// <summary>
        /// 获取数据库系统时间
        /// </summary>
        /// <returns>数据库系统时间</returns>
        public abstract DateTime GetDataBaseSysTime();
    }
}
