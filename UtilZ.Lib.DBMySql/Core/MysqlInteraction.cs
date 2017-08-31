using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utilities.Lib.DBBase.Base;
using UtilZ.Lib.DBModel.Config;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBMySql.Core
{
    /// <summary>
    /// Mysql数据库交互类
    /// </summary>
    public class MysqlInteraction : DBInteractionBase
    {
        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public override IDbDataAdapter CreateDbDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <returns>数据库连接对象</returns>
        public override IDbConnection CreateConnection(DBConfigElement config)
        {
            return new MySqlConnection(this.GetDBConStr(config, DBVisitType.R));
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
                config.Port = 3306;
            }

            return string.Format(@"database={0};data source={1};Port={2};user id={3};password={4}", config.DatabaseName, config.Host, config.Port, config.Account, config.Password);
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

            return new MySqlParameter(parameter.ParameterName, value);
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="collection">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public override void SetParameter(IDbCommand cmd, NDbParameterCollection collection)
        {
            MySqlCommand mysqlCmd = cmd as MySqlCommand;
            if (mysqlCmd == null || collection == null || collection.Count == 0)
            {
                return;
            }

            object value;
            foreach (NDbParameter parameter in collection)
            {
                value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                mysqlCmd.Parameters.AddWithValue(parameter.ParameterName, value);
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
            MySqlCommand mysqlCmd = cmd as MySqlCommand;
            if (mysqlCmd == null || parameters == null || parameters.Count() == 0)
            {
                return;
            }

            object value;
            foreach (NDbParameter parameter in parameters)
            {
                value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                mysqlCmd.Parameters.AddWithValue(parameter.ParameterName, value);
            }
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraValues">参数名及值字典集合</param>
        public override void SetParameter(IDbCommand cmd, Dictionary<string, object> paraValues)
        {
            MySqlCommand mysqlCmd = cmd as MySqlCommand;
            if (mysqlCmd == null || paraValues == null || paraValues.Count == 0)
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

                mysqlCmd.Parameters.AddWithValue(kv.Key, value);
            }
        }
    }
}
