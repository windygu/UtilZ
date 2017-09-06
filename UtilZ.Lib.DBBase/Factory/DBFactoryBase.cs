using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base.Extend;
using UtilZ.Lib.DBBase.Base;
using UtilZ.Lib.DBBase.Interface;
using UtilZ.Lib.DBModel.Config;

namespace UtilZ.Lib.DBBase.Factory
{
    /// <summary>
    /// 数据访问工厂基类
    /// </summary>
    public abstract class DBFactoryBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DBFactoryBase()
        {

        }

        /// <summary>
        /// 获取数据库交互实例
        /// </summary>
        /// <param name="config">数据库配置</param>
        /// <returns>数据库交互实例</returns>
        public abstract DBInteractionBase GetDBInteraction(DBConfigElement config);

        /// <summary>
        /// 获取数据库访问实例
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <returns>数据库访问实例</returns>
        public abstract IDBAccess GetDBAccess(int dbid);
    }
}
