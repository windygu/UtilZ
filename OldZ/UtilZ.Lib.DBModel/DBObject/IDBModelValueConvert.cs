using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.DBModel.DBObject
{
    /// <summary>
    /// 数据模型值转换器
    /// </summary>
    public interface IDBModelValueConvert
    {
        /// <summary>
        /// 从数据库库字段中的值转换为数据模型字段的值
        /// </summary>
        /// <param name="type">模型编号类型</param>
        /// <param name="databaseTypeName">数据库类型名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">数据库的值</param>
        /// <returns>数据模型字段的值</returns>
        object DBToModel(Type type, string databaseTypeName, string propertyName, object value);

        /// <summary>
        /// 从数据模型字段的值转换为数据库库字段中的值
        /// </summary>
        /// <param name="type">模型编号类型</param>
        /// <param name="databaseTypeName">数据库类型名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>数据库库字段中的值</returns>
        object ModelToDB(Type type, string databaseTypeName, string propertyName, object value);
    }
}
