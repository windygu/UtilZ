using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBBase.Core
{
    //数据库访问基类-保护的方法
    public abstract partial class DBAccessBase
    {
        #region protect method
        protected void SetParameter(IDbCommand cmd, DbParameterCollection collection)
        {
            if (collection != null && collection.Count > 0)
            {
                foreach (var para in collection)
                {
                    cmd.Parameters.Add(para);
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回执行结果的第一行第一列；
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="collection">命令的参数集合</param>
        /// <param name="transaction">事务参数</param>
        /// <returns>返回执行结果</returns>
        protected object InnerExecuteScalar(IDbConnection con, string sqlStr, DbParameterCollection collection = null, IDbTransaction transaction = null)
        {
            IDbCommand cmd = con.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = sqlStr;
            this.SetParameter(cmd, collection);
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
        protected int InnerExecuteNonQuery(IDbConnection con, string sqlStr, DbParameterCollection collection = null, IDbTransaction transaction = null)
        {
            IDbCommand cmd = con.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = sqlStr;
            this.SetParameter(cmd, collection);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 验证批量插入数据参数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        protected void ValidateBatchInsert(string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
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
        #endregion
    }
}
