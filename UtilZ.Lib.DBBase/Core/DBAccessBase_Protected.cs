using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Common;
using UtilZ.Lib.DBModel.Constant;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Core
{
    //数据库访问基类-保护的方法
    public abstract partial class DBAccessBase
    {
        #region protect method
        #region ADO.NET执行辅助方法
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="con">连接对象</param>
        /// <returns>命令</returns>
        protected virtual IDbCommand CreateCommand(IDbConnection con)
        {
            var cmd = con.CreateCommand();
            if (this.Config.CommandTimeout != DBConstant.CommandTimeout)
            {
                cmd.CommandTimeout = this.Config.CommandTimeout;
            }

            return cmd;
        }

        /// <summary>
        /// 是否初始化读写连接池
        /// </summary>
        protected bool _isInitReadWriteConPool = false;

        /// <summary>
        /// 是否初始化读写连接池线程锁
        /// </summary>
        protected readonly object _isInitReadWriteConPoolMonitor = new object();

        /// <summary>
        /// 初始化读写连接池
        /// </summary>
        public virtual void InitConPool()
        {
            if (this._isInitReadWriteConPool)
            {
                return;
            }

            lock (this._isInitReadWriteConPoolMonitor)
            {
                if (this._isInitReadWriteConPool)
                {
                    return;
                }

                DbConnectionPool.AddDbConnectionPool(this.Config, this.Interaction);
                this._isInitReadWriteConPool = true;
            }
        }
        #endregion

        /// <summary>
        /// 执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="transaction">事务参数</param>
        /// <returns>返回执行结果</returns>
        protected object InnerExecuteScalar(IDbConnection con, string sqlStr, NDbParameterCollection collection = null, IDbTransaction transaction = null)
        {
            IDbCommand cmd = this.CreateCommand(con);
            cmd.Transaction = transaction;
            cmd.CommandText = sqlStr;
            this.Interaction.SetParameter(cmd, collection);
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// 执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="transaction">事务参数</param>
        /// <returns>返回执行结果</returns>
        protected int InnerExecuteNonQuery(IDbConnection con, string sqlStr, NDbParameterCollection collection = null, IDbTransaction transaction = null)
        {
            IDbCommand cmd = this.CreateCommand(con);
            cmd.Transaction = transaction;
            cmd.CommandText = sqlStr;
            this.Interaction.SetParameter(cmd, collection);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 验证批量插入数据参数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <param name="con">数据库连接对象</param>
        protected void ValidateBatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data, IDbConnection con)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName");
            }

            if (cols == null || cols.Count() == 0)
            {
                throw new ArgumentException("cols");
            }

            int colCount = cols.Count();
            if (colCount < 1)
            {
                throw new ArgumentException("列不能为空", "cols");
            }

            if (data.ElementAt(0).Length != colCount)
            {
                throw new ArgumentException("列数与值中列数不匹配");
            }
        }

        /// <summary>
        /// 验证参数集合是否适用于同一SQL语句[返回true:适用;false:不适用]
        /// </summary>
        /// <param name="colValues">列名值映射字典</param>
        /// <returns>返回true:适用;false:不适用</returns>
        protected bool ValidateParameterCollectionsIsSameType(IEnumerable<Dictionary<string, object>> colValues)
        {
            if (colValues == null || colValues.Count() == 0)
            {
                return true;
            }

            int collectionCount = colValues.Count();
            var colValue = colValues.ElementAt(0);
            int parameterCount = colValue.Count;
            var parameterNames = colValue.Keys;
            for (int i = 1; i < collectionCount; i++)
            {
                colValue = colValues.ElementAt(i);
                if (colValue.Count != collectionCount)
                {
                    return false;
                }

                foreach (var subPara in colValue)
                {
                    if (!parameterNames.Contains(subPara.Key))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 验证参数集合是否适用于同一SQL语句[返回true:适用;false:不适用]
        /// </summary>
        /// <param name="collections">命令的参数集合</param>
        /// <returns>返回true:适用;false:不适用</returns>
        protected bool ValidateParameterCollectionsIsSameType(IEnumerable<NDbParameterCollection> collections)
        {
            if (collections == null || collections.Count() == 0)
            {
                return true;
            }

            int collectionCount = collections.Count();
            NDbParameterCollection collection = collections.ElementAt(0);
            int parameterCount = collection.Count;
            var parameterNames = (from para in collection select para.ParameterName);
            for (int i = 1; i < collectionCount; i++)
            {
                collection = collections.ElementAt(i);
                if (collection.Count != collectionCount)
                {
                    return false;
                }

                foreach (var subPara in collection)
                {
                    if (!parameterNames.Contains(subPara.ParameterName))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
