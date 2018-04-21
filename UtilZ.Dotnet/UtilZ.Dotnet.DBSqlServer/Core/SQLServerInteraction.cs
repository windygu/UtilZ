using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Base;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBSqlServer.Core
{
    /// <summary>
    /// SQLServer数据库交互类
    /// </summary>
    public class SQLServerInteraction : DBInteractioBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SQLServerInteraction()
            : base()
        {

        }

        /// <summary>
        /// 创建数据库读连接对象
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接对象</returns>
        public override IDbConnection CreateConnection(DBConfigElement config, DBVisitType visitType)
        {
            return new SqlConnection(this.GetDBConStr(config, visitType));
        }

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public override IDbDataAdapter CreateDbDataAdapter()
        {
            return new SqlDataAdapter();
        }

        /// <summary>
        /// 生成数据库连接字符串
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接字符串</returns>
        public override string GenerateDBConStr(DBConfigElement config, DBVisitType visitType)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (config.Port == 0)
            {
                config.Port = 1433;
            }

            return string.Format(@"data source={0},{1};initial catalog={2};user id={3};password={4}", config.Host, config.Port, config.DatabaseName, config.Account, config.Password);
        }

        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns>创建好的命令参数</returns>
        public override IDbDataParameter CreateDbParameter(NDbParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            object value = parameter.Value;
            if (value == null)
            {
                value = DBNull.Value;
            }

            return new SqlParameter(parameter.ParameterName, value);
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="collection">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public override void SetParameter(IDbCommand cmd, NDbParameterCollection collection)
        {
            SqlCommand sqlCmd = cmd as SqlCommand;
            if (sqlCmd == null || collection == null || collection.Count == 0)
            {
                return;
            }

            object value;
            foreach (var parameter in collection)
            {
                value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                sqlCmd.Parameters.AddWithValue(parameter.ParameterName, value);
            }
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public override void SetParameter(IDbCommand cmd, IEnumerable<IDbDataParameter> parameters)
        {
            SqlCommand sqlCmd = cmd as SqlCommand;
            if (sqlCmd == null || parameters == null || parameters.Count() == 0)
            {
                return;
            }

            object value;
            foreach (var parameter in parameters)
            {
                value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                sqlCmd.Parameters.AddWithValue(parameter.ParameterName, value);
            }
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraValues">参数名及值字典集合</param>
        public override void SetParameter(IDbCommand cmd, Dictionary<string, object> paraValues)
        {
            SqlCommand sqlCmd = cmd as SqlCommand;
            if (sqlCmd == null || paraValues == null || paraValues.Count == 0)
            {
                return;
            }

            object value;
            foreach (var kv in paraValues)
            {
                value = kv.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                sqlCmd.Parameters.AddWithValue(kv.Key, value);
            }
        }
    }
}
