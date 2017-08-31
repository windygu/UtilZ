using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Factory;
using UtilZ.Lib.DBModel.DBObject;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Core
{
    //数据库访问基类-删除
    public abstract partial class DBAccessBase
    {
        #region 单项删除
        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long Delete(string sqlStr, NDbParameterCollection collection = null)
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
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColName">主键名</param>
        /// <param name="value">主键值</param>
        /// <returns>返回受影响的行数</returns>
        public long Delete(string tableName, string priKeyColName, object value)
        {
            var priKeyColValues = new Dictionary<string, object>();
            priKeyColValues.Add(priKeyColName, value);
            return this.Delete(tableName, priKeyColValues);
        }

        /// <summary>
        /// 删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long Delete(string tableName, Dictionary<string, object> priKeyColValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                var cmd = this.CreateCommand(conInfo.Con);
                this.Interaction.GenerateSqlDelete(cmd, tableName, this.ParaSign, priKeyColValues);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="item">要删除的对象</param>
        /// <param name="con">数据库连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long DeleteT<T>(T item) where T : class
        {
            if (item == null)
            {
                return 0;
            }

            Type type = typeof(T);
            //数据模型信息
            DataModelInfo dataTableInfo = ORMManager.GetDataModelInfo(type);
            if (dataTableInfo.DicPriKeyDBColumnProperties == null || dataTableInfo.DicPriKeyDBColumnProperties.Count == 0)
            {
                throw new Exception(string.Format("类型:{0}没有标记为主键的字段,不能通过此方法删除该记录,请调用非泛型的方法对记录进行删除", type.FullName));
            }


            object value = null;
            //多主键遍历删除
            var priKeyColValues = new Dictionary<string, object>();
            priKeyColValues.Clear();
            foreach (var key in dataTableInfo.DicPriKeyDBColumnProperties)
            {
                value = key.Value.PropertyInfo.GetValue(item, key.Value.DBColumn.Index);
                if (value == null)
                {
                    throw new Exception("主键字段值不能为空或null");
                }

                priKeyColValues.Add(key.Key, value);
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                //删除记录
                var cmd = this.CreateCommand(conInfo.Con);
                this.Interaction.GenerateSqlDelete(cmd, dataTableInfo.DBTable.Name, this.ParaSign, priKeyColValues);
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStrs">Sql语句集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchDelete(IEnumerable<string> sqlStrs)
        {
            if (sqlStrs == null || sqlStrs.Count() == 0)
            {
                return 0L;
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                long effectCount = 0;
                using (IDbTransaction transaction = conInfo.Con.BeginTransaction())
                {
                    try
                    {
                        foreach (var sqlStr in sqlStrs)
                        {
                            effectCount += this.InnerExecuteNonQuery(conInfo.Con, sqlStr, null, transaction);
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
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchDelete(string tableName, IEnumerable<Dictionary<string, object>> priKeyColValues)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                long effectCount = 0;
                IDbCommand cmd;
                using (IDbTransaction transaction = conInfo.Con.BeginTransaction())
                {
                    try
                    {
                        if (this.ValidateParameterCollectionsIsSameType(priKeyColValues))
                        {
                            cmd = this.CreateCommand(conInfo.Con);
                            cmd.Transaction = transaction;
                            this.Interaction.GenerateSqlDelete(cmd, tableName, this.ParaSign, priKeyColValues.ElementAt(0));
                            cmd.Prepare();

                            foreach (var priKeyColValue in priKeyColValues)
                            {
                                this.Interaction.SetParameter(cmd, priKeyColValue);
                                effectCount += cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            foreach (var priKeyColValue in priKeyColValues)
                            {
                                cmd = this.CreateCommand(conInfo.Con);
                                cmd.Transaction = transaction;
                                this.Interaction.GenerateSqlDelete(cmd, tableName, this.ParaSign, priKeyColValue);
                                effectCount += cmd.ExecuteNonQuery();
                            }
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
        /// 批量删除记录,返回受影响的行数
        /// </summary>
        /// <param name="sqlStr">Sql语句</param>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchDelete(string sqlStr, IEnumerable<NDbParameterCollection> collections)
        {
            if (string.IsNullOrWhiteSpace(sqlStr))
            {
                throw new ArgumentNullException("sqlStr");
            }

            using (var conInfo = new DbConnectionInfo(this._dbid, DBVisitType.W))
            {
                using (IDbTransaction transaction = conInfo.Con.BeginTransaction())
                {
                    try
                    {
                        long effectCount = 0;
                        IDbCommand cmd = this.CreateCommand(conInfo.Con);
                        cmd.Transaction = transaction;
                        cmd.CommandText = sqlStr;
                        cmd.Prepare();

                        foreach (var collection in collections)
                        {
                            cmd.Parameters.Clear();
                            this.Interaction.SetParameter(cmd, collection);
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
        /// 批量删除泛型T记录,返回受影响的行数
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="items">要删除的集合</param>
        /// <returns>返回受影响的行数</returns>
        public virtual long BatchDeleteT<T>(IEnumerable<T> items) where T : class
        {
            if (items == null || items.Count() == 0)
            {
                return 0L;
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
                using (IDbTransaction transaction = conInfo.Con.BeginTransaction())
                {
                    try
                    {
                        object value = null;
                        long effectCount = 0;
                        IDbCommand cmd = this.CreateCommand(conInfo.Con);
                        cmd.Transaction = transaction;
                        cmd.CommandText = this.Interaction.GenerateSqlDelete(dataTableInfo.DBTable.Name, this.ParaSign, dataTableInfo.DicPriKeyDBColumnProperties.Keys);
                        cmd.Prepare();

                        //多主键遍历删除
                        var priKeyColValues = new NDbParameterCollection();
                        foreach (var item in items)
                        {
                            priKeyColValues.Clear();
                            foreach (var key in dataTableInfo.DicPriKeyDBColumnProperties)
                            {
                                value = key.Value.PropertyInfo.GetValue(item, key.Value.DBColumn.Index);
                                if (value == null)
                                {
                                    throw new Exception("主键字段值不能为空或null");
                                }

                                priKeyColValues.Add(key.Key, value);
                            }

                            //删除记录
                            cmd.Parameters.Clear();
                            this.Interaction.SetParameter(cmd, priKeyColValues);
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
        #endregion
    }
}
