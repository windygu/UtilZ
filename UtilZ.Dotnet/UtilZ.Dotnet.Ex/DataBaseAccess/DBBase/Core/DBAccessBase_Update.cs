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
    //数据库访问基类-更新
    public abstract partial class DBAccessBase
    {
        #region 单项更新
        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColName">主键名</param>
        /// <param name="priKeyValue">主键值</param>
        /// <param name="colName">修改列名</param>
        /// <param name="value">修改列值</param>
        /// <returns>返回受影响的行数</returns>
        public long Update(string tableName, string priKeyColName, object priKeyValue, string colName, object value)
        {
            Dictionary<string, object> colValues = new Dictionary<string, object>();
            colValues.Add(colName, value);
            return this.Update(tableName, priKeyColName, priKeyValue, colValues);
        }

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColName">主键名</param>
        /// <param name="priKeyValue">主键值</param>
        /// <param name="colValues">列名值字典</param>
        /// <returns>返回受影响的行数</returns>
        public long Update(string tableName, string priKeyColName, object priKeyValue, Dictionary<string, object> colValues)
        {
            Dictionary<string, object> priKeyColValues = new Dictionary<string, object>();
            priKeyColValues.Add(priKeyColName, priKeyValue);
            return this.Update(tableName, priKeyColValues, colValues);
        }

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="colValues">修改列名值字典</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long Update(string tableName, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                Dictionary<string, object> paraValues;
                IDbCommand cmd = this.CreateCommand(conInfo.Con);
                cmd.CommandText = this._interaction.GenerateSqlUpdate(tableName, this.ParaSign, priKeyColValues, colValues, out paraValues);
                this._interaction.SetParameter(cmd, paraValues);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long Update(string sqlStr, NDbParameterCollection collection = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                var cmd = this.CreateCommand(conInfo.Con);
                cmd.CommandText = sqlStr;
                this._interaction.SetParameter(cmd, collection);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">对象</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long UpdateT<T>(T item, IEnumerable<string> updateProperties = null) where T : class
        {
            if (item == null)
            {
                return 0;
            }

            if (updateProperties != null && updateProperties.Count() == 0)
            {
                throw new ArgumentException("要更新到表的属性名称集合当不为null时元素不能为空", "updateProperties");
            }

            Type type = typeof(T);
            //数据模型信息
            DataModelInfo dataTableInfo = ORMManager.GetDataModelInfo(type);
            if (dataTableInfo.DicPriKeyDBColumnProperties == null || dataTableInfo.DicPriKeyDBColumnProperties.Count == 0)
            {
                throw new Exception(string.Format("类型:{0}没有标记为主键的字段,不能通过此方法删除该记录,请调用非泛型的方法对记录进行删除", type.FullName));
            }

            string tableName = dataTableInfo.DBTable.Name;
            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                var dbTableInfo = CacheManager.GetDBTableInfo(conInfo.Con, this, tableName);
                //要更新到数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
                Dictionary<string, DBColumnPropertyInfo> dicDBColumnProperties;
                if (updateProperties == null)
                {
                    //找出可更新值的列
                    dicDBColumnProperties = (from dbcol in dataTableInfo.DicDBColumnProperties.Values
                                             where dbcol.DBColumn.DataAccessType == DBFieldDataAccessType.IM ||
                                             dbcol.DBColumn.DataAccessType == DBFieldDataAccessType.RIM ||
                                             dbcol.DBColumn.DataAccessType == DBFieldDataAccessType.RM
                                             select dbcol).ToDictionary((dbItem) => { return dbItem.DBColumn.ColumnName.ToUpper(); });
                }
                else
                {
                    //字段列
                    dicDBColumnProperties = new Dictionary<string, DBColumnPropertyInfo>();
                    DBFieldDataAccessType dataAccessType;
                    foreach (var kv in dataTableInfo.DicDBColumnProperties)
                    {
                        dataAccessType = kv.Value.DBColumn.DataAccessType;
                        //找出可更新值的列且属性要更新值的列
                        if (updateProperties.Contains(kv.Value.PropertyInfo.Name))
                        {
                            if ((dataAccessType == DBFieldDataAccessType.IM || dataAccessType == DBFieldDataAccessType.RIM || dataAccessType == DBFieldDataAccessType.RM))
                            {
                                dicDBColumnProperties.Add(kv.Key.ToUpper(), kv.Value);
                            }
                            else
                            {
                                throw new ArgumentException(string.Format("属性{0}对应的表{1}中字段{2}的值不允许修改", kv.Value.PropertyInfo.Name, dataTableInfo.DBTable.Name, kv.Value.DBColumn.ColumnName));
                            }
                        }
                    }
                }

                var paraCollection = new NDbParameterCollection();//更新参数值字典集合
                string dbParaSign = this.ParaSign;//数据库参数符号            
                DBColumnPropertyInfo dbColumnPropertyInfo = null;
                StringBuilder sbSql = new StringBuilder();//更新值语句            
                sbSql.Append("UPDATE ");
                sbSql.Append(dataTableInfo.DBTable.Name);
                sbSql.Append(" SET ");
                foreach (var key in dicDBColumnProperties.Keys.ToArray())
                {
                    dbColumnPropertyInfo = dicDBColumnProperties[key];
                    //sql
                    sbSql.Append(key);
                    sbSql.Append("=");
                    sbSql.Append(dbParaSign);
                    sbSql.Append(key);
                    sbSql.Append(",");
                    paraCollection.Add(key, null);
                }

                var whereParaNameValues = new Dictionary<string, object>();//条件参数值字典集合
                string and = "AND";
                StringBuilder sbWhereSql = new StringBuilder();//条件语句
                var dicPrikeyDBColumnProperties = dataTableInfo.DicPriKeyDBColumnProperties;
                foreach (var priKey in dicPrikeyDBColumnProperties)
                {
                    //如果该字段是主键列
                    sbWhereSql.Append(priKey.Key);
                    sbWhereSql.Append("=");
                    sbWhereSql.Append(dbParaSign);
                    sbWhereSql.Append(priKey.Key);
                    //参数
                    whereParaNameValues.Add(priKey.Key, null);
                    sbWhereSql.Append(and);
                }

                //移除结尾
                sbSql.Remove(sbSql.Length - 1, 1);
                sbWhereSql.Remove(sbWhereSql.Length - and.Length, and.Length);

                //合并SQL
                sbSql.Append(" WHERE ");
                sbSql.Append(sbWhereSql.ToString());
                //合并参数
                foreach (var pv in whereParaNameValues)
                {
                    paraCollection.Add(pv.Key, pv.Value);
                }

                string updateSql = sbSql.ToString();//更新SQL语句
                object tmpValue = null;//临时参数值
                List<string> keys = paraCollection.ParameterNames;
                DBFieldInfo dbFieldInfo = null;//字段信息

                foreach (var key in keys)
                {
                    dbColumnPropertyInfo = dataTableInfo.DicDBColumnProperties[key];
                    tmpValue = dbColumnPropertyInfo.PropertyInfo.GetValue(item, dbColumnPropertyInfo.DBColumn.Index);
                    if (tmpValue == null)
                    {
                        if (whereParaNameValues.ContainsKey(key))
                        {
                            throw new Exception("主键字段的值不能为空或null");
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
                                throw new ApplicationException(string.Format("表{0}中字段{1}不能为空值", tableName, key));
                            }
                        }
                    }

                    paraCollection[key].Value = tmpValue;
                }

                //更新记录
                return this.InnerExecuteNonQuery(conInfo.Con, updateSql, paraCollection);
            }
        }
        #endregion

        #region 批量更新
        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchUpdate(string sqlStr, IEnumerable<NDbParameterCollection> collections = null)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                var cmd = this.CreateCommand(conInfo.Con);
                cmd.CommandText = sqlStr;
                cmd.Prepare();
                long effectCount = 0;

                foreach (var collection in collections)
                {
                    cmd.Parameters.Clear();
                    this._interaction.SetParameter(cmd, collection);
                    effectCount += cmd.ExecuteNonQuery();
                }

                return effectCount;
            }
        }

        /// <summary>
        /// 批量更新记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchUpdate(IEnumerable<string> sqlStrs)
        {
            if (sqlStrs == null || sqlStrs.Count() == 0)
            {
                return 0L;
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                var cmd = this.CreateCommand(conInfo.Con);
                long effectCount = 0;
                foreach (var sqlStr in sqlStrs)
                {
                    cmd.CommandText = sqlStr;
                    effectCount += cmd.ExecuteNonQuery();
                }

                return effectCount;
            }
        }

        /// <summary>
        /// 批量更新泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要更新的对象集合</param>
        /// <param name="updateProperties">要更新到数据库表的属性名称集合,为null时全部更新</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchUpdateT<T>(IEnumerable<T> items, IEnumerable<string> updateProperties = null) where T : class
        {
            if (items == null || items.Count() == 0)
            {
                return 0L;
            }

            if (updateProperties != null && updateProperties.Count() == 0)
            {
                throw new ArgumentException("要更新到表的属性名称集合当不为null时元素不能为空", "updateProperties");
            }

            Type type = typeof(T);
            //数据模型信息
            DataModelInfo dataTableInfo = ORMManager.GetDataModelInfo(type);
            if (dataTableInfo.DicPriKeyDBColumnProperties == null || dataTableInfo.DicPriKeyDBColumnProperties.Count == 0)
            {
                throw new Exception(string.Format("类型:{0}没有标记为主键的字段,不能通过此方法删除该记录,请调用非泛型的方法对记录进行删除", type.FullName));
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                string tableName = dataTableInfo.DBTable.Name;
                var dbTableInfo = CacheManager.GetDBTableInfo(conInfo.Con, this, tableName);
                //要更新到数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
                Dictionary<string, DBColumnPropertyInfo> dicDBColumnProperties;
                if (updateProperties == null)
                {
                    //找出可更新值的列
                    dicDBColumnProperties = (from dbcol in dataTableInfo.DicDBColumnProperties.Values
                                             where dbcol.DBColumn.DataAccessType == DBFieldDataAccessType.IM ||
                                             dbcol.DBColumn.DataAccessType == DBFieldDataAccessType.RIM ||
                                             dbcol.DBColumn.DataAccessType == DBFieldDataAccessType.RM
                                             select dbcol).ToDictionary((dbItem) => { return dbItem.DBColumn.ColumnName.ToUpper(); });
                }
                else
                {
                    //字段列
                    dicDBColumnProperties = new Dictionary<string, DBColumnPropertyInfo>();
                    DBFieldDataAccessType dataAccessType;
                    foreach (var kv in dataTableInfo.DicDBColumnProperties)
                    {
                        dataAccessType = kv.Value.DBColumn.DataAccessType;
                        //找出可更新值的列且属性要更新值的列
                        if (updateProperties.Contains(kv.Value.PropertyInfo.Name))
                        {
                            if ((dataAccessType == DBFieldDataAccessType.IM || dataAccessType == DBFieldDataAccessType.RIM || dataAccessType == DBFieldDataAccessType.RM))
                            {
                                dicDBColumnProperties.Add(kv.Key.ToUpper(), kv.Value);
                            }
                            else
                            {
                                throw new ArgumentException(string.Format("属性{0}对应的表{1}中字段{2}的值不允许修改", kv.Value.PropertyInfo.Name, dataTableInfo.DBTable.Name, kv.Value.DBColumn.ColumnName));
                            }
                        }
                    }
                }

                var paraCollection = new NDbParameterCollection();//更新参数值字典集合
                string dbParaSign = this.ParaSign;//数据库参数符号
                DBColumnPropertyInfo dbColumnPropertyInfo = null;
                StringBuilder sbSql = new StringBuilder();//更新值语句
                sbSql.Append("UPDATE ");
                sbSql.Append(dataTableInfo.DBTable.Name);
                sbSql.Append(" SET ");
                foreach (var key in dicDBColumnProperties.Keys.ToArray())
                {
                    dbColumnPropertyInfo = dicDBColumnProperties[key];
                    //sql
                    sbSql.Append(key);
                    sbSql.Append("=");
                    sbSql.Append(dbParaSign);
                    sbSql.Append(key);
                    sbSql.Append(",");
                    paraCollection.Add(key, null);
                }

                var whereParaNameValues = new Dictionary<string, object>();//条件参数值字典集合
                string and = "AND";
                StringBuilder sbWhereSql = new StringBuilder();//条件语句
                var dicPrikeyDBColumnProperties = dataTableInfo.DicPriKeyDBColumnProperties;
                foreach (var priKey in dicPrikeyDBColumnProperties)
                {
                    //如果该字段是主键列
                    sbWhereSql.Append(priKey.Key);
                    sbWhereSql.Append("=");
                    sbWhereSql.Append(dbParaSign);
                    sbWhereSql.Append(priKey.Key);
                    //参数
                    whereParaNameValues.Add(priKey.Key, null);
                    sbWhereSql.Append(and);
                }

                //移除结尾
                sbSql.Remove(sbSql.Length - 1, 1);
                sbWhereSql.Remove(sbWhereSql.Length - and.Length, and.Length);

                //合并SQL
                sbSql.Append(" WHERE ");
                sbSql.Append(sbWhereSql.ToString());
                //合并参数
                foreach (var pv in whereParaNameValues)
                {
                    paraCollection.Add(pv.Key, pv.Value);
                }

                string updateSql = sbSql.ToString();//更新SQL语句
                long effectCount = 0;//受影响的记录行数
                object tmpValue = null;//临时参数值
                List<string> keys = paraCollection.ParameterNames;
                DBFieldInfo dbFieldInfo = null;//字段信息


                var cmd = this.CreateCommand(conInfo.Con);
                cmd.CommandText = updateSql;
                cmd.Prepare();

                foreach (var item in items)
                {
                    foreach (var key in keys)
                    {
                        dbColumnPropertyInfo = dataTableInfo.DicDBColumnProperties[key];
                        tmpValue = dbColumnPropertyInfo.PropertyInfo.GetValue(item, dbColumnPropertyInfo.DBColumn.Index);
                        if (tmpValue == null)
                        {
                            if (whereParaNameValues.ContainsKey(key))
                            {
                                throw new Exception("主键字段的值不能为空或null");
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
                                    throw new ApplicationException(string.Format("表{0}中字段{1}不能为空值", tableName, key));
                                }
                            }
                        }

                        paraCollection[key].Value = tmpValue;
                    }

                    //更新记录
                    cmd.Parameters.Clear();
                    this._interaction.SetParameter(cmd, paraCollection);
                    effectCount += cmd.ExecuteNonQuery();
                }

                //返回受影响的记录行数
                return effectCount;
            }
        }
        #endregion
    }
}
