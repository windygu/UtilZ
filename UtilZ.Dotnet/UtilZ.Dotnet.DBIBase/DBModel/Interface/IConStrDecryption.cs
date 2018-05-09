using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.Config;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBIBase.DBModel.Interface
{
    /// <summary>
    /// 数据库连接信息解密接口
    /// </summary>
    public interface IConStrDecryption
    {
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <returns>数据库连接字符串</returns>
        string GetDBConStr(DBConfigElement config);
    }
}
