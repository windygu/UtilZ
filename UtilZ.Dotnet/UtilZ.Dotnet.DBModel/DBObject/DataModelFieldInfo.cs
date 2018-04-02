using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBModel.DBObject
{
    /// <summary>
    /// 数据模型字段信息
    /// </summary>
    [Serializable]
    public class DataModelFieldInfo : DBColumnAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="colName">列名</param>
        public DataModelFieldInfo(string propertyName, string colName)
            : this(propertyName, colName, null, false, DBFieldDataAccessType.RIM)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="colName">列名</param>
        /// <param name="dataAccessType">字段数据访问类型</param>
        public DataModelFieldInfo(string propertyName, string colName, DBFieldDataAccessType dataAccessType)
            : this(propertyName, colName, null, false, dataAccessType)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="colName">列名</param>
        /// <param name="index">索引</param>
        public DataModelFieldInfo(string propertyName, string colName, object[] index)
            : this(propertyName, colName, index, false, DBFieldDataAccessType.RIM)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="colName">列名</param>
        /// <param name="isPriKey">是否是主键</param>
        /// <param name="dataAccessType">[RIM:读写改;RM:读改;R:只读]</param>
        public DataModelFieldInfo(string propertyName, string colName, bool isPriKey, DBFieldDataAccessType dataAccessType)
            : this(propertyName, colName, null, isPriKey, dataAccessType)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="colName">列名</param>
        /// <param name="index">索引</param>
        /// <param name="isPriKey">是否是主键</param>
        /// <param name="dataAccessType">[RIM:读写改;RM:读改;R:只读]</param>
        public DataModelFieldInfo(string propertyName, string colName, object[] index, bool isPriKey, DBFieldDataAccessType dataAccessType)
            : base(colName, index, isPriKey, dataAccessType)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("属性名不能为空或null", "propertyName");
            }

            this.PropertyName = propertyName;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; private set; }
    }
}
