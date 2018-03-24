using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Log.Config.Interface
{
    /// <summary>
    /// 数据库配置接口
    /// </summary>
    public interface IDatabaseLogConfig : IConfig
    {
        /// <summary>
        /// 数据库编号ID
        /// </summary>
        int DBID { get; set; }

        /// <summary>
        /// 是否使用数据库编号[true:使用数据编号;false:使用数据库配置]
        /// </summary>
        bool IsUseDBID { get; set; }

        /// <summary>
        /// 日志表名
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// 数据连接对象类型字符串
        /// </summary>
        string ConnectionType { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /*
        /// <summary>
        /// 数据库参数符号
        /// </summary>
        /// <returns>数据库参数符号</returns>
        string DbParaSign { get; set; }

        /// <summary>
        /// 是否插入主键[true:插入;false:不插入]
        /// </summary>
        bool IsInsertID { get; set; }

        /// <summary>
        /// 时间值是否要转换为整形值[true:转换;false:不转换]
        /// </summary>
        bool IsConvertTimeValue { get; set; }
         * */
    }
}
