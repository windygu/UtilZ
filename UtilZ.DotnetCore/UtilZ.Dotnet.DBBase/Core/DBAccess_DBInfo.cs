using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBBase.Model;
using UtilZ.Dotnet.DBIBase.DBModel.Common;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBBase.Core
{
    //数据库访问基类-DBInfo
    public abstract partial class DBAccessBase
    {
        /// <summary>
        /// 判断表是否存在[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名[表名区分大小写的数据库:Oracle,SQLite]</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public abstract bool IsExistTable(string tableName);

        /// <summary>
        /// 判断表中是否存在字段[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public abstract bool IsExistField(string tableName, string fieldName);

        /// <summary>
        /// 获取表二进制字段名称集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        public List<string> GetTableBinaryFieldInfo(string tableName)
        {
            string sqlStr = string.Format("select * from {0} where 0=1", tableName);
            List<string> binaryCols = new List<string>();
            DataTable dt = this.QueryData(sqlStr);
            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == ClrSystemType.BytesType)
                {
                    binaryCols.Add(col.ColumnName);
                }
            }

            return binaryCols;
        }

        #region 获取表的字段信息
        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        public List<DBFieldInfo> GetTableFieldInfos(string tableName)
        {
            return this.InnerGetTableFieldInfos(null, tableName);
        }

        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        protected abstract List<DBFieldInfo> InnerGetTableFieldInfos(IDbConnection con, string tableName);

        /// <summary>
        /// 获取字段的公共语言运行时类型字典集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列集合</param>
        /// <returns>字段的公共语言运行时类型字典集合</returns>
        public Dictionary<string, DBFieldType> GetFieldDbClrFieldType(string tableName, DataColumnCollection cols)
        {
            Dictionary<string, DBFieldType> dicFieldDbClrFieldType = new Dictionary<string, DBFieldType>();
            foreach (DataColumn col in cols)
            {
                dicFieldDbClrFieldType.Add(col.ColumnName, DBHelper.GetDbClrFieldType(col.DataType));
            }

            return dicFieldDbClrFieldType;
        }
        #endregion

        #region 获取表信息
        /// <summary>
        /// 获取当前用户有权限的所有表集合
        /// </summary>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>当前用户有权限的所有表集合</returns>
        public abstract List<DBTableInfo> GetTableInfos(bool isGetFieldInfo = false);

        /// <summary>
        /// 查询主键列名集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主键列名集合</returns>
        public List<string> QueryPrikeyColumns(string tableName)
        {
            return this.InnerQueryPrikeyColumns(null, tableName);
        }

        /// <summary>
        /// 查询主键列名集合
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>主键列名集合</returns>
        protected abstract List<string> InnerQueryPrikeyColumns(IDbConnection con, string tableName);

        /// <summary>
        /// 获取表信息[表不存在返回null]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>表信息</returns>
        public DBTableInfo GetTableInfo(string tableName, bool isGetFieldInfo = false)
        {
            return this.InnerGetTableInfo(null, tableName, isGetFieldInfo);
        }

        /// <summary>
        /// 获取表信息[表不存在返回null]
        /// </summary>
        /// <param name="con">数据库连接对象</param>
        /// <param name="tableName">表名</param>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>表信息</returns>
        internal protected abstract DBTableInfo InnerGetTableInfo(IDbConnection con, string tableName, bool isGetFieldInfo = false);
        #endregion

        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns>数据库版本信息</returns>
        public abstract string GetDataBaseVersion();

        /// <summary>
        /// 获取数据库系统时间
        /// </summary>
        /// <returns>数据库系统时间</returns>
        public abstract DateTime GetDataBaseSysTime();
    }
}
