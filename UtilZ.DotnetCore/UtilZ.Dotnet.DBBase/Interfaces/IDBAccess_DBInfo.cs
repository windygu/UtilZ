using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;

namespace UtilZ.Dotnet.DBBase.Interfaces
{
    //数据库访问接口-数据库信息相关
    public partial interface IDBAccess
    {
        /// <summary>
        /// 判断表是否存在[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名[表名区分大小写的数据库:Oracle,SQLite]</param>
        /// <returns>存在返回true,不存在返回false</returns>
        bool IsExistTable(string tableName);

        /// <summary>
        /// 判断表中是否存在字段[存在返回true,不存在返回false]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        bool IsExistField(string tableName, string fieldName);

        /// <summary>
        /// 获取表二进制字段名称集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        List<string> GetTableBinaryFieldInfo(string tableName);

        #region 获取表的字段信息
        /// <summary>
        /// 获取表的字段信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>字段信息集合</returns>
        List<DBFieldInfo> GetTableFieldInfos(string tableName);

        /// <summary>
        /// 获取字段的公共语言运行时类型字典集合[tableName参数只是为重写此方法提供.基方法内不使用]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列集合</param>
        /// <returns>字段的公共语言运行时类型字典集合</returns>
        Dictionary<string, DBFieldType> GetFieldDbClrFieldType(string tableName, DataColumnCollection cols);
        #endregion

        #region 获取表信息
        /// <summary>
        /// 获取当前用户有权限的所有表集合
        /// </summary>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>当前用户有权限的所有表集合</returns>
        List<DBTableInfo> GetTableInfos(bool isGetFieldInfo = false);

        /// <summary>
        /// 查询主键列名集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>主键列名集合</returns>
        List<string> QueryPrikeyColumns(string tableName);

        /// <summary>
        /// 获取表信息[表不存在返回null]
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="isGetFieldInfo">是否获取字段信息[true:获取字段信息;false:不获取;默认不获取]</param>
        /// <returns>表信息</returns>
        DBTableInfo GetTableInfo(string tableName, bool isGetFieldInfo = false);
        #endregion

        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns>数据库版本信息</returns>
        string GetDataBaseVersion();

        /// <summary>
        /// 获取数据库系统时间
        /// </summary>
        /// <returns>数据库系统时间</returns>
        DateTime GetDataBaseSysTime();
    }
}
