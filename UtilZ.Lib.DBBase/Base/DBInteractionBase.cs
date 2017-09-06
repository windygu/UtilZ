using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Extend;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBModel.Config;
using UtilZ.Lib.DBModel.Constant;
using UtilZ.Lib.DBModel.Interface;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Base
{
    /// <summary>
    /// 数据库交互基类
    /// </summary>
    public abstract class DBInteractionBase : IDBInteraction
    {
        /// <summary>
        /// 数据库连接字符串字典集合[key:数据库编号ID;value:数据库连接字符串]
        /// </summary>
        private readonly ConcurrentDictionary<int, DBConStrInfo> _dicConStrs = new ConcurrentDictionary<int, DBConStrInfo>();

        /// <summary>
        /// 数据库连接字符串字典集合线程锁
        /// </summary>
        private readonly object _dicConStrsMonitor = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBInteractionBase()
        {

        }

        /// <summary>
        /// 创建数据库读连接对象
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接对象</returns>
        public abstract IDbConnection CreateConnection(DBConfigElement config, DBVisitType visitType);

        /// <summary>
        /// 创建DbDataAdapter
        /// </summary>
        /// <returns>创建好的DbDataAdapter</returns>
        public abstract IDbDataAdapter CreateDbDataAdapter();

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接字符串</returns>
        public string GetDBConStr(DBConfigElement config, DBVisitType visitType)
        {
            int dbid = config.DBID;
            DBConStrInfo dbConStrInfo;
            if (this._dicConStrs.ContainsKey(dbid))
            {
                if (!this._dicConStrs.TryGetValue(dbid, out dbConStrInfo))
                {
                    throw new ApplicationException(string.Format("获取数据库编号为{0}的数据库连接字符串失败", dbid));
                }
            }
            else
            {
                lock (this._dicConStrsMonitor)
                {
                    if (this._dicConStrs.ContainsKey(dbid))
                    {
                        if (!this._dicConStrs.TryGetValue(dbid, out dbConStrInfo))
                        {
                            throw new ApplicationException(string.Format("获取数据库编号为{0}的数据库连接字符串失败", dbid));
                        }
                    }
                    else
                    {
                        string decryptionType = config.Decryption;
                        string readConStr, writeConStr;
                        if (string.IsNullOrEmpty(decryptionType))
                        {
                            if (config.DBConInfoType == 0)
                            {
                                readConStr = config.ConStr;
                                writeConStr = config.ConStr;
                            }
                            else
                            {
                                readConStr = this.GenerateDBConStr(config, DBVisitType.R);
                                writeConStr = this.GenerateDBConStr(config, DBVisitType.W);
                            }
                        }
                        else
                        {
                            //调用数据库连接信息解密接口解密
                            IConStrDecryption decryption = NExtendActivator.CreateInstance(decryptionType) as IConStrDecryption;
                            if (decryption == null)
                            {
                                throw new ApplicationException(string.Format("创建数据库连接信息解密接口类型{0}失败", decryptionType));
                            }

                            readConStr = decryption.GetDBConStr(config, DBVisitType.R);
                            writeConStr = decryption.GetDBConStr(config, DBVisitType.W);
                        }

                        dbConStrInfo = new DBConStrInfo(readConStr, writeConStr);
                        this._dicConStrs.TryAdd(dbid, dbConStrInfo);
                    }
                }
            }

            string conStr;
            if (visitType == DBVisitType.R)
            {
                conStr = dbConStrInfo.ReadConStr;
            }
            else
            {
                conStr = dbConStrInfo.WriteConStr;
            }

            return conStr;
        }

        /// <summary>
        /// 生成数据库连接字符串
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <param name="visitType">访问类型</param>
        /// <returns>数据库连接字符串</returns>
        public abstract string GenerateDBConStr(DBConfigElement config, DBVisitType visitType);

        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="parameter">命令参数</param>
        /// <returns>创建好的命令参数</returns>
        public abstract IDbDataParameter CreateDbParameter(NDbParameter parameter);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmdParameter">命令参数</param>
        /// <param name="parameter">参数</param>
        public virtual void SetParameter(IDbDataParameter cmdParameter, NDbParameter parameter)
        {
            //各个数据库继承自IDbDataParameter的命令参数构造函数有如下几个:
            //public IDbDataParameter() { }
            //public IDbDataParameter(string parameterName, object value) { }
            //public IDbDataParameter(string parameterName, DbType dbType) { }
            //public IDbDataParameter(string parameterName, DbType type, int size) { }

            //调用无参数构造函数
            switch (parameter.ConstructedTypeID)
            {
                case 1:
                    cmdParameter.ParameterName = parameter.ParameterName;
                    cmdParameter.Direction = parameter.Direction;
                    cmdParameter.Size = parameter.Size;
                    break;
                case 2:
                    cmdParameter.ParameterName = parameter.ParameterName;
                    cmdParameter.Direction = parameter.Direction;
                    cmdParameter.Size = parameter.Size;
                    cmdParameter.DbType = parameter.DbType;
                    break;
                case 3:
                    cmdParameter.ParameterName = parameter.ParameterName;
                    cmdParameter.Value = parameter.Value;
                    cmdParameter.Direction = parameter.Direction;
                    break;
                case 4:
                    cmdParameter.ParameterName = parameter.ParameterName;
                    cmdParameter.DbType = parameter.DbType;
                    cmdParameter.Value = parameter.Value;
                    cmdParameter.Direction = parameter.Direction;
                    break;
                case 5:
                    cmdParameter.ParameterName = parameter.ParameterName;
                    cmdParameter.DbType = parameter.DbType;
                    cmdParameter.Size = parameter.Size;
                    cmdParameter.Value = parameter.Value;
                    cmdParameter.Direction = parameter.Direction;
                    break;
                case 6:
                    cmdParameter.ParameterName = parameter.ParameterName;
                    cmdParameter.Value = parameter.Value;
                    cmdParameter.DbType = parameter.DbType;
                    cmdParameter.Direction = parameter.Direction;
                    cmdParameter.Size = parameter.Size;
                    break;
                default:
                    throw new NotImplementedException(string.Format("未实现的命令参数构造函数类型{0}", parameter.ConstructedTypeID));
            }

            //var cmd2 = new System.Data.Common.DbCommand().CreateParameter();
            //cmdParameter.IsNullable = parameter.IsNullable;
            cmdParameter.SourceColumn = parameter.SourceColumn;
            //cmdParameter.SourceColumnNullMapping = parameter.SourceColumnNullMapping;
            cmdParameter.SourceVersion = parameter.SourceVersion;
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="collection">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public abstract void SetParameter(IDbCommand cmd, NDbParameterCollection collection);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>创建好的命令参数</returns>
        public abstract void SetParameter(IDbCommand cmd, IEnumerable<IDbDataParameter> parameters);

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraNames">参数名集合</param>
        /// <param name="values">参数值集合</param>
        public void SetParameter(IDbCommand cmd, IEnumerable<string> paraNames, IEnumerable<object> values)
        {
            if (paraNames == null || values == null)
            {
                return;
            }

            int paraCount = paraNames.Count();
            if (paraCount != values.Count())
            {
                throw new ApplicationException("参数个数与值个数不匹配");
            }

            Dictionary<string, object> paraValues = new Dictionary<string, object>();
            string colName;
            for (int i = 0; i < paraCount; i++)
            {
                colName = paraNames.ElementAt(i);
                if (string.IsNullOrWhiteSpace(colName))
                {
                    throw new ApplicationException("参数名不能为空或null或全空格");
                }

                paraValues.Add(colName, values.ElementAt(i));
            }

            this.SetParameter(cmd, paraValues);
        }

        /// <summary>
        /// 设置命令参数
        /// </summary>
        /// <param name="cmd">命令参数</param>
        /// <param name="paraValues">参数名及值字典集合</param>
        public abstract void SetParameter(IDbCommand cmd, Dictionary<string, object> paraValues);

        /// <summary>
        /// 生成可用的列参数名称
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="paraNames">已有的参数名集合</param>
        /// <param name="paraIndex">参数名索引</param>
        /// <returns>可用的列参数名称</returns>
        public string GenerateParaName(string colName, IEnumerable<string> paraNames, ref int paraIndex)
        {
            string tmpParaName = colName;
            while (paraNames.Contains(tmpParaName))
            {
                tmpParaName = string.Format("{0}{1}", tmpParaName, paraIndex++);
            }

            return tmpParaName;
        }

        /// <summary>
        /// 生成SQL插入语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paraSign">数据库参数字符</param>
        /// <param name="cols">列名集合</param>
        /// <returns>SQL插入语句</returns>
        public virtual string GenerateSqlInsert(string tableName, string paraSign, IEnumerable<string> cols)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            if (cols == null)
            {
                throw new ArgumentNullException("cols");
            }

            int colCount = cols.Count();
            if (colCount == 0)
            {
                throw new ArgumentException("列名集合不能为空");
            }

            foreach (var col in cols)
            {
                if (string.IsNullOrWhiteSpace(col))
                {
                    throw new ArgumentException("列名不能为null或空或全部空格");
                }
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("INSERT INTO ");
            sbSql.Append(tableName);
            sbSql.Append(" (");
            sbSql.Append(string.Join(",", cols));
            sbSql.Append(") VALUES (");
            sbSql.Append(paraSign);
            sbSql.Append(string.Join("," + paraSign, cols));
            sbSql.Append(")");
            return sbSql.ToString();
        }

        /// <summary>
        /// 生成SQL删除语句
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="tableName">表名</param>
        /// <param name="paraSign">数据库参数字符</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        public virtual void GenerateSqlDelete(IDbCommand cmd, string tableName, string paraSign, Dictionary<string, object> priKeyColValues)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(tableName);
            if (priKeyColValues != null && priKeyColValues.Count > 0)
            {
                IDbDataParameter parameter;
                sb.Append(" WHERE");
                string andStr = " AND";
                foreach (KeyValuePair<string, object> pryKeyColValue in priKeyColValues)
                {
                    if (string.IsNullOrEmpty(pryKeyColValue.Key))
                    {
                        throw new Exception("列名不能为null或空");
                    }

                    sb.Append(" ");
                    sb.Append(pryKeyColValue.Key);
                    sb.Append("=");
                    sb.Append(paraSign);
                    sb.Append(pryKeyColValue.Key);
                    sb.Append(andStr);
                    parameter = cmd.CreateParameter();
                    parameter.ParameterName = pryKeyColValue.Key;
                    parameter.Value = pryKeyColValue.Value;
                    cmd.Parameters.Add(parameter);
                }

                sb = sb.Remove(sb.Length - andStr.Length, andStr.Length);
            }

            cmd.CommandText = sb.ToString();
        }

        /// <summary>
        /// 生成SQL删除语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paraSign">数据库参数字符</param>
        /// <param name="priKeyCols">主键列名集合</param>
        public virtual string GenerateSqlDelete(string tableName, string paraSign, IEnumerable<string> priKeyCols)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(tableName);
            if (priKeyCols != null && priKeyCols.Count() > 0)
            {
                sb.Append(" WHERE");
                string andStr = " AND";
                foreach (var pryKeyColValue in priKeyCols)
                {
                    if (string.IsNullOrEmpty(pryKeyColValue))
                    {
                        throw new Exception("列名不能为null或空");
                    }

                    sb.Append(" ");
                    sb.Append(pryKeyColValue);
                    sb.Append("=");
                    sb.Append(paraSign);
                    sb.Append(pryKeyColValue);
                    sb.Append(andStr);
                }

                sb = sb.Remove(sb.Length - andStr.Length, andStr.Length);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成SQL更新语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paraSign">数据库参数字符</param>
        /// <param name="priKeyColValues">主键列名值映射字典</param>
        /// <param name="colValues">列名值字典</param>
        /// <param name="paraValues">参数值字典集合</param>
        /// <returns>SQL更新语句</returns>
        public virtual string GenerateSqlUpdate(string tableName, string paraSign, Dictionary<string, object> priKeyColValues, Dictionary<string, object> colValues, out Dictionary<string, object> paraValues)
        {
            paraValues = new Dictionary<string, object>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE ");
            sbSql.Append(tableName);
            sbSql.Append(" SET ");
            foreach (var colValue in colValues)
            {
                sbSql.Append(colValue.Key);
                sbSql.Append("=");
                sbSql.Append(paraSign);
                sbSql.Append(colValue.Key);
                sbSql.Append(",");
                paraValues.Add(colValue.Key, colValue.Value);
            }

            sbSql = sbSql.Remove(sbSql.Length - 1, 1);

            if (priKeyColValues != null && priKeyColValues.Count > 0)
            {
                sbSql.Append(" WHERE ");
                int paraIndex = 1;
                string paraName;
                foreach (var priKeyValue in priKeyColValues)
                {
                    paraName = this.GenerateParaName(priKeyValue.Key, paraValues.Keys, ref paraIndex);
                    sbSql.Append(paraName);
                    sbSql.Append("=");
                    sbSql.Append(paraSign);
                    sbSql.Append(paraName);
                    paraValues.Add(paraName, priKeyValue.Value);
                    sbSql.Append(",");
                }

                sbSql = sbSql.Remove(sbSql.Length - 1, 1);
            }

            return sbSql.ToString();
        }
    }

    /// <summary>
    /// 数据库连接字符串信息
    /// </summary>
    internal class DBConStrInfo
    {
        /// <summary>
        /// 读取连接字符串
        /// </summary>
        public string ReadConStr { get; private set; }

        /// <summary>
        /// 写取连接字符串
        /// </summary>
        public string WriteConStr { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="readConStr">读取连接字符串</param>
        /// <param name="writeConStr">写取连接字符串</param>
        public DBConStrInfo(string readConStr, string writeConStr)
        {
            this.ReadConStr = readConStr;
            this.WriteConStr = writeConStr;
        }
    }
}
