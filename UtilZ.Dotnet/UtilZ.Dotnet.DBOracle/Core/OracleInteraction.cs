using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBBase.Base;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBOracle.Core
{
    /// <summary>
    /// Oracle数据库交互类
    /// </summary>
    public class OracleInteraction : DBInteractioBase
    {
        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public override IDbDataAdapter CreateDbDataAdapter()
        {
            return new OracleDataAdapter();
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

            IDbDataParameter oracleParameter = new OracleParameter();
            oracleParameter.ParameterName = parameter.ParameterName;
            oracleParameter.Direction = parameter.Direction;
            oracleParameter.DbType = parameter.DbType;
            if (parameter.Direction == ParameterDirection.Input || parameter.Direction == ParameterDirection.InputOutput)
            {
                object value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                oracleParameter.Value = value;
            }

            return oracleParameter;
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="collection">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public override void SetParameter(IDbCommand cmd, NDbParameterCollection collection)
        {
            OracleCommand oracleCmd = cmd as OracleCommand;
            if (oracleCmd == null || collection == null || collection.Count == 0)
            {
                return;
            }

            OracleParameter oracleParameter;
            object value;
            foreach (NDbParameter parameter in collection)
            {
                value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                if (oracleCmd.Parameters.Contains(parameter.ParameterName))
                {
                    oracleParameter = oracleCmd.Parameters[parameter.ParameterName];
                    oracleParameter.Value = value;
                }
                else
                {
                    oracleParameter = new OracleParameter(parameter.ParameterName, value);
                    oracleCmd.Parameters.Add(oracleParameter);
                }
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
            OracleCommand oracleCmd = cmd as OracleCommand;
            if (oracleCmd == null || parameters == null || parameters.Count() == 0)
            {
                return;
            }

            OracleParameter oracleParameter;
            object value;
            foreach (NDbParameter parameter in parameters)
            {
                value = parameter.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                if (oracleCmd.Parameters.Contains(parameter.ParameterName))
                {
                    oracleParameter = oracleCmd.Parameters[parameter.ParameterName];
                    oracleParameter.Value = value;
                }
                else
                {
                    oracleParameter = new OracleParameter(parameter.ParameterName, value);
                    oracleCmd.Parameters.Add(oracleParameter);
                }
            }
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraValues">参数名及值字典集合</param>
        public override void SetParameter(IDbCommand cmd, Dictionary<string, object> paraValues)
        {
            OracleCommand oracleCmd = cmd as OracleCommand;
            if (oracleCmd == null || paraValues == null || paraValues.Count == 0)
            {
                return;
            }

            OracleParameter oracleParameter;
            object value;
            foreach (var kv in paraValues)
            {
                value = kv.Value;
                if (value == null)
                {
                    value = DBNull.Value;
                }

                if (oracleCmd.Parameters.Contains(kv.Key))
                {
                    oracleParameter = oracleCmd.Parameters[kv.Key];
                    oracleParameter.Value = value;
                }
                else
                {
                    oracleParameter = new OracleParameter(kv.Key, value);
                    oracleCmd.Parameters.Add(oracleParameter);
                }
            }
        }

        /// <summary>
        /// 创建数据库读连接对象
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接对象</returns>
        public override IDbConnection CreateConnection(DBConfigElement config, DBVisitType visitType)
        {
            return new OracleConnection(this.GetDBConStr(config, visitType));
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
                config.Port = 1521;
            }

            return string.Format(@"User Id={0};Password={1};Data Source={2}:{3}/{4}", config.Account, config.Password, config.Host, config.Port, config.DatabaseName);
            //return string.Format(@"User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})))",
            //config.Account, config.Password, config.Host, config.Port, config.DatabaseName);
        }
    }
}
