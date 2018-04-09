using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Factory;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBInfo;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBObject;
using UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.Model;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBBase.Core
{
    //数据库访问基类-插入
    public abstract partial class DBAccessBase
    {
        #region 单项插入
        /// <summary>
        /// 插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long Insert(string sqlStr, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                return this.InnerExecuteNonQuery(conInfo.Con, sqlStr, collection);
            }
        }

        /// <summary>
        /// 插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">插入项</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long InsertT<T>(T item) where T : class
        {
            if (item == null)
            {
                return 0;
            }


            Type type = typeof(T);
            //数据模型信息
            DataModelInfo dataTableInfo = ORMManager.GetDataModelInfo(type);
            IDBModelValueConvert dbModelValueConvert = dataTableInfo.ModelValueConvert;
            string tableName = dataTableInfo.DBTable.Name;
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                DBTableInfo dbTableInfo = CacheManager.GetDBTableInfo(conInfo.Con, this, tableName);
                //数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
                Dictionary<string, DBColumnPropertyInfo> dicDBColumnProperties = dataTableInfo.DicDBColumnProperties;
                DBColumnPropertyInfo dbColumnPropertyInfo = null;
                var paraCollection = new NDbParameterCollection();
                string dbParaSign = this.ParaSign;
                StringBuilder sbPara = new StringBuilder();//参数
                StringBuilder sbSql = new StringBuilder();//值

                sbSql.Append("insert into ");
                sbSql.Append(dataTableInfo.DBTable.Name);
                sbSql.Append(" (");

                DBFieldDataAccessType dataAccessType;
                foreach (var key in dicDBColumnProperties.Keys)
                {
                    dbColumnPropertyInfo = dicDBColumnProperties[key];
                    dataAccessType = dbColumnPropertyInfo.DBColumn.DataAccessType;
                    //如果该字段是为只读或读改则不需要插入
                    if (dataAccessType == DBFieldDataAccessType.R || dataAccessType == DBFieldDataAccessType.RM)
                    {
                        continue;
                    }

                    //sql
                    sbSql.Append(key);
                    sbSql.Append(",");
                    sbPara.Append(dbParaSign);
                    sbPara.Append(key);
                    sbPara.Append(",");

                    //参数
                    paraCollection.Add(key, null);
                }

                sbSql.Remove(sbSql.Length - 1, 1);
                sbPara.Remove(sbPara.Length - 1, 1);
                sbSql.Append(") values(");
                sbSql.Append(sbPara.ToString());
                sbSql.Append(")");

                string insertSql = sbSql.ToString();
                var paraKeys = paraCollection.ParameterNames;//列名集合
                object tmpValue = null;
                DBFieldInfo dbFieldInfo = null;
                //参数
                foreach (var key in paraKeys)
                {
                    dbColumnPropertyInfo = dicDBColumnProperties[key];
                    tmpValue = dbColumnPropertyInfo.PropertyInfo.GetValue(item, dbColumnPropertyInfo.DBColumn.Index);
                    if (dbModelValueConvert != null)//如果该值需要转换为数据库表中字段对应的值,则转换值
                    {
                        tmpValue = dbModelValueConvert.ModelToDB(type, this.DatabaseTypeName, dbColumnPropertyInfo.PropertyInfo.Name, tmpValue);
                    }

                    dbFieldInfo = dbTableInfo.DbFieldInfos[key];
                    if (tmpValue == null)
                    {
                        if (dbFieldInfo.AllowNull)
                        {
                            tmpValue = DBNull.Value;
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("表{0}中字段{1}不允许为null值", tableName, key));
                        }
                    }

                    paraCollection[key].Value = tmpValue;
                }

                //执行sql语句
                return this.InnerExecuteNonQuery(conInfo.Con, insertSql, paraCollection);
            }
        }
        #endregion

        #region 批量插入
        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">DataTable</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchInsert(string tableName, DataTable dt)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (dt == null)
            {
                throw new ArgumentNullException("dt");
            }

            if (dt.Rows.Count == 0)
            {
                return 0L;
            }

            List<object[]> data = new List<object[]>();
            foreach (DataRow row in dt.Rows)
            {
                data.Add(row.ItemArray);
            }

            List<string> cols = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                cols.Add(col.ColumnName);
            }

            return this.BatchInsert(tableName, cols, data);
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>返回受影响的行数</returns>
        public long BatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                return this.InnerBatchInsert(conInfo.Con, tableName, cols, data);
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
        protected virtual long InnerBatchInsert(IDbConnection con, string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
        {
            if (cols == null || cols.Count() == 0 || data == null || data.Count() == 0)
            {
                return 0L;
            }


            //验证批量插入数据参数
            this.ValidateBatchInsert(tableName, cols, data);

            using (IDbTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    IDbCommand cmd = this.CreateCommand(con);
                    cmd.Transaction = transaction;
                    cmd.CommandText = this._interaction.GenerateSqlInsert(tableName, this.ParaSign, cols);
                    cmd.Prepare();
                    long effectCount = 0;

                    foreach (var arr in data)
                    {
                        this._interaction.SetParameter(cmd, cols, arr);
                        effectCount += cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return effectCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchInsert(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                using (IDbTransaction transaction = conInfo.Con.BeginTransaction())
                {
                    try
                    {
                        IDbCommand cmd = this.CreateCommand(conInfo.Con);
                        cmd.Transaction = transaction;
                        cmd.CommandText = sqlStr;
                        cmd.Prepare();
                        long effectCount = 0;
                        foreach (var collection in collections)
                        {
                            cmd.Parameters.Clear();
                            this._interaction.SetParameter(cmd, collection);
                            effectCount += cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return effectCount;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 转换泛型类型T为批量插入的值类型
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="con">数据库连接对象</param>
        /// <param name="items">泛型类型集合</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>插入表名</returns>
        protected string ConvertTToBatchValue<T>(IDbConnection con, IEnumerable<T> items, out List<string> cols, out List<object[]> data) where T : class
        {
            //数据模型信息
            Type type = typeof(T);
            DataModelInfo dataTableInfo = ORMManager.GetDataModelInfo(type);
            IDBModelValueConvert dbModelValueConvert = dataTableInfo.ModelValueConvert;
            string tableName = dataTableInfo.DBTable.Name;
            DBTableInfo dbTableInfo = CacheManager.GetDBTableInfo(con, this, tableName);
            //数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
            Dictionary<string, DBColumnPropertyInfo> dicDBColumnProperties = dataTableInfo.DicDBColumnProperties;
            DBColumnPropertyInfo dbColumnPropertyInfo = null;
            cols = new List<string>();
            data = new List<object[]>();
            DBFieldDataAccessType dataAccessType;
            foreach (var key in dicDBColumnProperties.Keys)
            {
                dbColumnPropertyInfo = dicDBColumnProperties[key];
                dataAccessType = dbColumnPropertyInfo.DBColumn.DataAccessType;
                //如果该字段是为只读或读改则不需要插入
                if (dataAccessType == DBFieldDataAccessType.R || dataAccessType == DBFieldDataAccessType.RM)
                {
                    continue;
                }

                cols.Add(key);
            }

            object tmpValue = null;
            DBFieldInfo dbFieldInfo = null;
            object[] value;
            foreach (T item in items)
            {
                value = new object[cols.Count];
                //参数
                for (int i = 0; i < cols.Count; i++)
                {
                    var key = cols[i];
                    dbColumnPropertyInfo = dicDBColumnProperties[key];
                    tmpValue = dbColumnPropertyInfo.PropertyInfo.GetValue(item, dbColumnPropertyInfo.DBColumn.Index);
                    if (dbModelValueConvert != null)//如果该值需要转换为数据库表中字段对应的值,则转换值
                    {
                        tmpValue = dbModelValueConvert.ModelToDB(type, this.DatabaseTypeName, dbColumnPropertyInfo.PropertyInfo.Name, tmpValue);
                    }

                    dbFieldInfo = dbTableInfo.DbFieldInfos[key];
                    if (tmpValue == null)
                    {
                        if (dbFieldInfo.AllowNull)
                        {
                            tmpValue = DBNull.Value;
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("表{0}中字段{1}不允许为null值", tableName, key));
                        }
                    }

                    value[i] = tmpValue;
                }

                data.Add(value);
            }

            return tableName;
        }

        /// <summary>
        /// 批量插入泛型T类型对象数据到数据到库,返回受影响的行数,仅支持单表
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">插入项集合</param>
        /// <returns>返回受影响的行数</returns>
        public long BatchInsertT<T>(IEnumerable<T> items) where T : class
        {
            if (items == null || items.Count() == 0)
            {
                return 0;
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                List<string> cols;
                List<object[]> data;
                string tableName = this.ConvertTToBatchValue(conInfo.Con, items, out cols, out data);
                return this.InnerBatchInsert(conInfo.Con, tableName, cols, data);
            }
        }
        #endregion
    }
}
