using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBBase.Core;
using UtilZ.Lib.DBModel.Model;

namespace UtilZ.Lib.DBBase.Factory
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    public class DbConnectionInfo : IDisposable
    {
        /// <summary>
        /// 数据库编号ID
        /// </summary>
        public int DBID { get; private set; }

        /// <summary>
        /// 数据库访问类型
        /// </summary>
        public DBVisitType VisitType { get; private set; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public IDbConnection Con { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="visitType">数据库访问类型</param>
        public DbConnectionInfo(int dbid, DBVisitType visitType)
        {
            this.DBID = dbid;
            this.VisitType = visitType;
            this.Con = DbConnectionPool.GetDbConnection(dbid, visitType);
        }

        #region IDispose接口实现
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="isDisposing">是否释放资源标识</param>
        protected virtual void Dispose(bool isDisposing)
        {
            DbConnectionPool.ReleaseDbConnection(this.DBID, this.Con, this.VisitType);
        }
        #endregion
    }
}
