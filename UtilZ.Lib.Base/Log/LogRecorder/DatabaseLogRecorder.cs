using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.NLog.Config;
using UtilZ.Lib.Base.NLog.Config.Interface;
using UtilZ.Lib.Base.NLog.LogRecorderInterface;
using UtilZ.Lib.Base.NLog.Model;

namespace UtilZ.Lib.Base.NLog.LogRecorder
{
    /// <summary>
    /// 数据库日志记录基类
    /// </summary>
    public class DatabaseLogRecorder : BaseLogRecorder, IDatabaseLogRecorder
    {
        /// <summary>
        /// 配置
        /// </summary>
        private IDatabaseLogConfig _config = null;

        /// <summary>
        /// 配置
        /// </summary>
        public virtual IDatabaseLogConfig Config
        {
            get { return _config; }
            set
            {
                _config = value;

                try
                {
                    this._databaseType = this.GetDatabaseType(this._config.ConnectionType);
                    this._dbParaSign = this.GetDbParaSign(this._databaseType);

                    //检查日志表是否存在,如果不存在则创建
                    this.CheckLogTable();
                }
                catch (Exception ex)
                {
                    LogSysInnerLog.OnRaiseLog(this, ex);
                }
            }
        }

        /// <summary>
        /// 基础配置
        /// </summary>
        public override IConfig BaseConfig
        {
            get { return this.Config; }
            set { this.Config = value as IDatabaseLogConfig; }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        private int _databaseType = -1;

        /// <summary>
        /// 数据库参数字符串
        /// </summary>
        private string _dbParaSign = null;

        /// <summary>
        /// 默认数据库连接类型,SqlConnection名称
        /// </summary>
        private readonly string _sqlConnectionName = typeof(System.Data.SqlClient.SqlConnection).FullName;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DatabaseLogRecorder()
        {

        }

        /// <summary>
        /// 检查日志表是否存在,如果不存在则创建
        /// </summary>
        private void CheckLogTable()
        {
            //如果日志表不存在则创建表
            if (!this.IsExistTable(this._databaseType, this.Config.TableName))
            {
                var sqls = this.GetCreateLogTableSql(this._databaseType, this.Config.TableName);
                using (var con = this.CreateDbConnection())
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }

                    var cmd = con.CreateCommand();
                    foreach (var sql in sqls)
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="item">日志项</param>
        public override void WriteLog(LogItem item)
        {
            if (item == null)
            {
                return;
            }

            this.WriteLog(new List<LogItem> { item });
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="items">日志项集合</param>
        public override void WriteLog(List<LogItem> items)
        {
            if (this.Config == null || !this.Config.Enable || items == null || items.Count == 0)
            {
                return;
            }

            try
            {
                using (var con = this.CreateDbConnection())
                {
                    if (con == null)
                    {
                        return;
                    }

                    string tableName = this.Config.TableName;
                    DbCommand cmd = con.CreateCommand();
                    string dbParaSign = this._dbParaSign;
                    //string dbParaSign = this.Config.DbParaSign;
                    //if (this.Config.IsInsertID)
                    if (this._databaseType == LogDatabaseType.Oracle)
                    {
                        cmd.CommandText = string.Format(@"INSERT INTO {0}
                                           (ID
                                           ,TIME
                                           ,THREADID
                                           ,THREADNAME
                                           ,EVENTID
                                           ,LOGLEVEL
                                           ,LOGGER
                                           ,MESSAGE
                                           ,EXCEPTION)
                                     VALUES
                                           ({1}ID
                                           ,{1}TIME
                                           ,{1}THREADID
                                           ,{1}THREADNAME
                                           ,{1}EVENTID
                                           ,{1}LOGLEVEL
                                           ,{1}LOGGER
                                           ,{1}MESSAGE
                                           ,{1}EXCEPTION)", tableName, dbParaSign);
                    }
                    else
                    {
                        cmd.CommandText = string.Format(@"INSERT INTO {0}
                                           (TIME
                                           ,THREADID
                                           ,THREADNAME
                                           ,EVENTID
                                           ,LOGLEVEL
                                           ,LOGGER
                                           ,MESSAGE
                                           ,EXCEPTION)
                                     VALUES
                                           ({1}TIME
                                           ,{1}THREADID
                                           ,{1}THREADNAME
                                           ,{1}EVENTID
                                           ,{1}LOGLEVEL
                                           ,{1}LOGGER
                                           ,{1}MESSAGE
                                           ,{1}EXCEPTION)", tableName, dbParaSign);
                    }

                    // <THREADNAME, varchar(255),>
                    //,<LOGGER, varchar(255),>
                    //,<MESSAGE, varchar(4000),>
                    //,<EXCEPTION, varchar(4000),>)
                    string logRecorderName = this.Config.Name;
                    foreach (var item in items)
                    {
                        try
                        {
                            //过滤条件验证
                            if (!base.FilterValidate(item.Level))
                            {
                                continue;
                            }

                            //输出日志
                            this.OutputLog(logRecorderName, item);

                            cmd.Parameters.Clear();
                            if (this._databaseType == LogDatabaseType.Oracle)
                            {
                                this.AddParameter(cmd, "ID", Guid.NewGuid().ToString());
                            }

                            this.AddParameter(cmd, "TIME", item.Time);
                            this.AddParameter(cmd, "THREADID", item.ThreadID);
                            this.AddParameter(cmd, "THREADNAME", item.ThreadName);
                            this.AddParameter(cmd, "EVENTID", item.EventID);
                            this.AddParameter(cmd, "LOGLEVEL", item.Level.ToString());
                            this.AddParameter(cmd, "LOGGER", item.Logger);

                            string message = item.Message;
                            if (!string.IsNullOrEmpty(message) && message.Length > 4000)
                            {
                                message = message.Substring(0, 3999);
                            }

                            this.AddParameter(cmd, "MESSAGE", message);
                            string exception = item.Exception != null ? item.Exception.ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(exception) && exception.Length > 4000)
                            {
                                exception = exception.Substring(0, 3999);
                            }

                            this.AddParameter(cmd, "EXCEPTION", exception);
                            if (con.State == System.Data.ConnectionState.Closed)
                            {
                                con.Open();
                            }


                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception exi)
                        {
                            LogSysInnerLog.OnRaiseLog(this, exi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSysInnerLog.OnRaiseLog(this, ex);
            }

            //追加日志
            base.AppenderLog(items);
        }

        /// <summary>
        /// 添加命令参数
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        protected void AddParameter(DbCommand cmd, string parameterName, object value)
        {
            var dbParameter = cmd.CreateParameter();
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value == null ? DBNull.Value : value;
            cmd.Parameters.Add(dbParameter);
        }

        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        protected virtual DbConnection CreateDbConnection()
        {
            if (this.Config == null || string.IsNullOrEmpty(this.Config.ConnectionString))
            {
                return null;
            }

            DbConnection con;
            if (string.IsNullOrEmpty(this.Config.ConnectionType) || this._sqlConnectionName.Equals(this.Config.ConnectionType))
            {
                con = new System.Data.SqlClient.SqlConnection(this.Config.ConnectionString);
            }
            else
            {
                con = LogUtil.CreateInstance(this.Config.ConnectionType) as DbConnection;
                if (con != null)
                {
                    con.ConnectionString = this.Config.ConnectionString;
                }
            }

            return con;
        }

        /// <summary>
        /// 获取数据库连接类型
        /// </summary>
        /// <param name="connectionType">数据库连接类型字符串</param>
        /// <returns>数据库连接类型</returns>
        protected virtual int GetDatabaseType(string connectionType)
        {
            if (string.IsNullOrEmpty(connectionType))
            {
                throw new ArgumentNullException("数据库连接类型字符串不能为空或null");
            }

            int databaseType;
            if (string.IsNullOrEmpty(connectionType) || this._sqlConnectionName.Equals(connectionType))
            {
                databaseType = LogDatabaseType.SQLServer;
            }
            else
            {
                connectionType = connectionType.ToLower();
                if (connectionType.Contains("oracle"))
                {
                    databaseType = LogDatabaseType.Oracle;
                }
                else if (connectionType.Contains("sqlite"))
                {
                    databaseType = LogDatabaseType.SQLite;
                }
                else if (connectionType.Contains("mysql"))
                {
                    databaseType = LogDatabaseType.MySQL;
                }
                else
                {
                    throw new ArgumentException(string.Format("未识别的数据库连接类型{0}", connectionType));
                }
            }

            return databaseType;
        }

        /// <summary>
        /// 获取数据库参数字符串
        /// </summary>
        /// <param name="databaseType">数据库连接类型</param>
        /// <returns>数据库参数字符串</returns>
        protected virtual string GetDbParaSign(int databaseType)
        {
            string dbParaSign;
            switch (databaseType)
            {
                case LogDatabaseType.Oracle:
                    dbParaSign = ":";
                    break;
                case LogDatabaseType.SQLServer:
                case LogDatabaseType.SQLite:
                    dbParaSign = "@";
                    break;
                case LogDatabaseType.MySQL:
                    dbParaSign = "?";
                    break;
                default:
                    throw new ArgumentException(string.Format("未识别的数据库类型{0}", databaseType));
            }

            return dbParaSign;
        }

        /// <summary>
        /// 判断表是否存在[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="tableName">表名[表名区分大小写的数据库:Oracle,SQLite]</param>
        /// <returns>存在返回true,不存在返回false</returns>
        protected virtual bool IsExistTable(int dbType, string tableName)
        {
            using (var con = this.CreateDbConnection())
            {
                if (con == null)
                {
                    throw new ApplicationException("数据库连接对象创建失败");
                }

                DbCommand cmd = con.CreateCommand();
                switch (dbType)
                {
                    case LogDatabaseType.Oracle:
                        //select count(0) from tabs where table_name ='表名';
                        //select count(0) into tableExistedCount from user_tables t where t.table_name = upper('表名'); --从系统表中查询当表是否存在
                        //cmd.CommandText = string.Format(@"select count(0) into tableExistedCount from user_tables t where t.table_name = upper({0}TABLENAME)", dbParaSign);
                        cmd.CommandText = string.Format(@"select count(0) from tabs where TABLE_NAME ='{0}'", tableName.ToUpper());
                        break;
                    case LogDatabaseType.SQLServer:
                        //select COUNT(0) from sysobjects where id = object_id('表名') and type = 'u';
                        //select COUNT(0) from sys.tables where name='表名' and type = 'u';
                        cmd.CommandText = string.Format(@"select COUNT(0) from sys.tables where name='{0}' and type = 'u'", tableName);
                        break;
                    case LogDatabaseType.SQLite:
                        cmd.CommandText = string.Format(@"SELECT COUNT(0) FROM sqlite_master where type='table' and name='{0}'", tableName);
                        break;
                    case LogDatabaseType.MySQL:
                        //SHOW TABLES LIKE '表名';   -- 当前连接库中是否存在表
                        //select Count(0) from INFORMATION_SCHEMA.TABLES where TABLE_NAME='表名' AND TABLE_SCHEMA='数据库名';
                        cmd.CommandText = string.Format(@"select Count(0) from INFORMATION_SCHEMA.TABLES where TABLE_NAME='{0}' AND TABLE_SCHEMA='{1}'", tableName, con.Database);
                        break;
                    default:
                        throw new NotSupportedException("不支持的数据库类型");
                }

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                object value = cmd.ExecuteScalar();
                return Convert.ToInt32(value) > 0;
            }
        }

        /// <summary>
        /// 获取创建日志表sql
        /// </summary>
        /// <param name="databaseType">数据库类型</param>
        /// <param name="tableName">表名</param>
        /// <returns>创建日志表sql</returns>
        protected virtual List<string> GetCreateLogTableSql(int databaseType, string tableName)
        {
            List<string> sqlParas = new List<string>();
            string sqlStr = null;
            switch (databaseType)
            {
                case LogDatabaseType.SQLServer:
                    #region SQLServer
                    sqlStr = string.Format(@"CREATE TABLE [dbo].[{0}](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TIME] [datetime] NULL,
	[THREADID] [int] NULL,
	[THREADNAME] [varchar](255) NULL,
    [EVENTID] [int] NULL,
	[LOGLEVEL] [varchar](10) NULL,
	[LOGGER] [varchar](255) NULL,
	[MESSAGE] [varchar](4000) NULL,
	[EXCEPTION] [varchar](4000) NULL,
 CONSTRAINT [PK__{0}__3214EC27145C0A3F] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY];

SET ANSI_PADDING OFF;
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'ID';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'TIME';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线程ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'THREADID';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'线程名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'THREADNAME';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'EVENTID';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志级别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'LOGLEVEL';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志产生类名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'LOGGER';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'MESSAGE';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'异常信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'EXCEPTION';", tableName);
                    #endregion
                    sqlParas.Add(sqlStr);
                    break;
                case LogDatabaseType.MySQL:
                    #region MySQL
                    sqlStr = string.Format(@"CREATE TABLE `{0}` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `TIME` datetime DEFAULT NULL COMMENT '时间',
  `THREADID` int(11) DEFAULT NULL COMMENT '线程ID',
  `THREADNAME` varchar(255) COLLATE utf8_bin DEFAULT NULL COMMENT '线程名称',
  `EVENTID` int(11) DEFAULT NULL COMMENT '事件ID',
  `LOGLEVEL` varchar(10) DEFAULT NULL COMMENT '日志级别',
  `LOGGER` varchar(255) COLLATE utf8_bin DEFAULT NULL COMMENT '日志产生类名',
  `MESSAGE` varchar(4000) COLLATE utf8_bin DEFAULT NULL COMMENT '日志信息',
  `EXCEPTION` varchar(4000) COLLATE utf8_bin DEFAULT NULL COMMENT '异常信息',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;", tableName);
                    #endregion
                    sqlParas.Add(sqlStr);
                    break;
                case LogDatabaseType.Oracle:
                    #region Oracle
                    sqlStr = string.Format(@"CREATE TABLE {0}
        (
          ID         VARCHAR2(36),
          TIME       DATE,
          THREADID   NUMBER(11),
          THREADNAME VARCHAR2(255),
          EVENTID    NUMBER(11),
          LOGGER     VARCHAR2(255),
          MESSAGE    VARCHAR2(4000),
          EXCEPTION  VARCHAR2(4000),
          LOGLEVEL   VARCHAR2(10)
        )", tableName);
                    sqlParas.Add(sqlStr);
                    sqlParas.Add(string.Format(@"alter table {0} add constraint {0}_PKEY primary key (ID)", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.ID IS '主键'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.TIME IS '时间'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.THREADID IS '线程ID'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.THREADNAME IS '线程名称'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.EVENTID IS '事件ID'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.LOGGER IS '日志产生类名'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.MESSAGE IS '日志信息'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.EXCEPTION IS '异常信息'", tableName));
                    sqlParas.Add(string.Format(@"COMMENT ON COLUMN {0}.LOGLEVEL IS '日志级别'", tableName));
                    #endregion
                    break;
                case LogDatabaseType.SQLite:
                    #region SQLite
                    sqlStr = string.Format(@"CREATE TABLE ""{0}"" (
""ID""  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
""TIME""  DATETIME,
""THREADID""  INTEGER,
""THREADNAME""  TEXT(255),
""EVENTID""  INTEGER,
""LOGLEVEL""  INTEGER,
""LOGGER""  TEXT(255),
""MESSAGE""  TEXT(4000),
""EXCEPTION""  TEXT(4000)
);", tableName);
                    #endregion
                    sqlParas.Add(sqlStr);
                    break;
                default:
                    throw new NotSupportedException(string.Format("数据库类型:{0}不支持", databaseType));
            }

            return sqlParas;
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public class LogDatabaseType
    {
        /// <summary>
        /// SQLServer数据库
        /// </summary>
        public const int SQLServer = 1;

        /// <summary>
        /// Oracle数据库
        /// </summary>
        public const int Oracle = 2;

        /// <summary>
        /// MySQL数据库
        /// </summary>
        public const int MySQL = 3;

        /// <summary>
        /// SQLite数据库
        /// </summary>
        public const int SQLite = 5;
    }
}
