using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UtilZ.Lib.DBModel.DBObject
{
    /// <summary>
    /// 数据库列信息及对应的类的属性信息
    /// </summary>
    [Serializable]
    public class DBColumnPropertyInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbColumn">列信息</param>
        /// <param name="propertyInfo">属性信息</param>
        public DBColumnPropertyInfo(DBColumnAttribute dbColumn, PropertyInfo propertyInfo)
        {
            this.DBColumn = dbColumn;
            this.PropertyInfo = propertyInfo;
        }

        /// <summary>
        /// 列信息
        /// </summary>
        public DBColumnAttribute DBColumn { get; private set; }

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }
    }
}
