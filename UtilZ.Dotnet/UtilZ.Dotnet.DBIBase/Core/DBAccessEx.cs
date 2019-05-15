﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.Config;
using UtilZ.Dotnet.DBIBase.Interface;
using UtilZ.Dotnet.DBIBase.Model;

namespace UtilZ.Dotnet.DBIBase.Core
{
    /// <summary>
    /// 扩展方法类DBAccessEx
    /// </summary>
    public static class DBAccessEx
    {
        /// <summary>
        /// 设置命令配置参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="config">配置</param>
        public static void SetCommandPara(IDbCommand cmd, DatabaseConfig config)
        {
            if (config.CommandTimeout != DBConstant.CommandTimeout)
            {
                cmd.CommandTimeout = config.CommandTimeout;
            }
        }

        /// <summary>
        /// 快速创建命令
        /// </summary>
        /// <param name="dbid">数据库ID</param>
        /// <param name="con">连接对象</param>
        /// <returns>命令</returns>
        public static IDbCommand CreateCommand(int dbid, IDbConnection con)
        {
            var config = DatabaseConfigManager.GetConfig(dbid);
            var cmd = con.CreateCommand();
            SetCommandPara(cmd, config);
            return cmd;
        }

        /// <summary>
        /// 快速创建命令
        /// </summary>
        /// <param name="dbAccess">IDBAccess</param>
        /// <param name="con">连接对象</param>
        /// <param name="sqlParameterDic">参数字典集合</param>
        /// <returns>命令</returns>
        public static IDbCommand CreateCommand(this IDBAccess dbAccess, IDbConnection con, Dictionary<string, object> sqlParameterDic = null)
        {
            var cmd = con.CreateCommand();
            if (sqlParameterDic != null && sqlParameterDic.Count > 0)
            {
                foreach (var kv in sqlParameterDic)
                {
                    AddParameter(cmd, kv.Key, kv.Value);
                }
            }

            var config = dbAccess.Config;
            SetCommandPara(cmd, config);
            return cmd;
        }

        /// <summary>
        /// 快速创建命令
        /// </summary>
        /// <param name="dbAccess">IDBAccess</param>
        /// <param name="con">连接对象</param>
        /// <param name="sqlStr">SQL语句</param>
        /// <param name="sqlParameterDic">命令参数字典集合[key:参数名;value:参数值]</param>
        /// <param name="transaction">事务对象</param>
        /// <returns>命令</returns>
        public static IDbCommand CreateCommand(this IDBAccess dbAccess, IDbConnection con, string sqlStr, Dictionary<string, object> sqlParameterDic = null, IDbTransaction transaction = null)
        {
            IDbCommand cmd = CreateCommand(dbAccess, con, sqlParameterDic);
            cmd.Transaction = transaction;
            cmd.CommandText = sqlStr;
            return cmd;
        }

        /// <summary>
        /// 扩展方法添加命令参数
        /// </summary>
        /// <param name="cmd">目标命令</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        public static void AddParameter(this IDbCommand cmd, string parameterName, object value)
        {
            var parameter = cmd.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            cmd.Parameters.Add(parameter);
        }
    }
}
