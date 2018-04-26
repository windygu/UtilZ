using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Factory;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBMySql.Core
{
    //Mysql数据库访问类-插入泛型数据
    public partial class MySqlDBAccess
    {
        /// <summary>
        /// 批量插入数据,返回受影响的行数
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合</param>
        /// <param name="data">数据</param>
        /// <returns>返回受影响的行数</returns>
        protected override long InnerBatchInsert(IDbConnection con, string tableName, IEnumerable<string> cols, IEnumerable<object[]> data)
        {
            if (cols == null || cols.Count() == 0 || data == null || data.Count() == 0)
            {
                return 0L;
            }

            //DBTableInfo dbTableInfo = CacheManager.GetDBTableInfo(this, con, tableName);
            char comma = ',';
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ");
            sb.Append(tableName);
            sb.Append(" (");
            foreach (var col in cols)
            {
                sb.Append(col);
                sb.Append(comma);
            }

            sb = sb.Remove(sb.Length - 1, 1);//去掉最后一个逗号
            sb.Append(") VALUES ");

            string insertPreSqlStr = sb.ToString();
            var collection = new NDbParameterCollection();
            var sqlMaxLength = this.SqlMaxLength;
            StringBuilder sbInsert = new StringBuilder(insertPreSqlStr);
            StringBuilder sbTmpValue = new StringBuilder();
            int paraIndex = 1;
            string paraSign = this.ParaSign;
            string paraName;

            using (IDbTransaction transaction = con.BeginTransaction())
            {
                try
                {
                    long effectRowCount = 0;
                    //INSERT INTO tbl_name (a,b,c) VALUES(1,2,3),(4,5,6),(7,8,9),(10,11,12)...; 
                    foreach (var dataRow in data)
                    {
                        sbTmpValue.Append('(');
                        foreach (var colValue in dataRow)
                        {
                            paraName = string.Format("{0}para{1}", paraSign, paraIndex++);
                            sbTmpValue.Append(paraName);
                            sbTmpValue.Append(comma);
                            collection.Add(paraName, colValue);
                        }

                        sbTmpValue = sbTmpValue.Remove(sbTmpValue.Length - 1, 1);//去掉最后一个逗号
                        sbTmpValue.Append("),");

                        if (sbInsert.Length > 0 && sbTmpValue.Length + sbInsert.Length >= sqlMaxLength)
                        {
                            if (sbTmpValue[sbTmpValue.Length - 1] == comma)
                            {
                                sbTmpValue = sbTmpValue.Remove(sbTmpValue.Length - 1, 1);//去掉最后一个逗号
                            }

                            //执行
                            sbInsert.Append(sbTmpValue.ToString());
                            sbTmpValue.Clear();
                            effectRowCount += this.InnerExecuteNonQuery(con, sbInsert.ToString(), collection);

                            //清空执行过的语句
                            collection.Clear();
                            sbInsert = sbInsert.Clear();
                            sbInsert.Append(insertPreSqlStr);
                        }
                    }

                    if (sbTmpValue.Length > 0)
                    {
                        //执行最后一次
                        if (sbTmpValue[sbTmpValue.Length - 1] == comma)
                        {
                            sbTmpValue = sbTmpValue.Remove(sbTmpValue.Length - 1, 1);//去掉最后一个逗号
                        }

                        sbInsert.Append(sbTmpValue.ToString());
                        //执行
                        effectRowCount += this.InnerExecuteNonQuery(con, sbInsert.ToString(), collection, transaction);
                    }

                    return effectRowCount;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
