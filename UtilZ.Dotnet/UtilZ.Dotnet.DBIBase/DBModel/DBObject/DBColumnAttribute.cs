using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBIBase.DBModel.DBObject
{
    /// <summary>
    /// 数据模型数据库字段特性信息
    /// </summary>
    [Serializable]
    public class DBColumnAttribute : Attribute
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="colName">列名</param>
        public DBColumnAttribute(string colName)
            : this(colName, null, false, DBFieldDataAccessType.RIM)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="dataAccessType">[RIM:读写改;RM:读改;R:只读]</param>
        public DBColumnAttribute(string colName, DBFieldDataAccessType dataAccessType)
            : this(colName, null, false, dataAccessType)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="index">索引</param>
        public DBColumnAttribute(string colName, object[] index)
            : this(colName, index, false, DBFieldDataAccessType.RIM)
        { }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="colName">列名</param>        
        /// <param name="index">索引</param>
        /// <param name="isNeedConvert">数据模型中属性的值与数据库表中字段对应的值是否是否需要转换[true:需要转换;false:不需要转换]</param>
        public DBColumnAttribute(string colName, object[] index, bool isNeedConvert)
            : this(colName, index, false, DBFieldDataAccessType.RIM)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="isPriKey">是否是主键</param>
        /// <param name="dataAccessType">[RIM:读写改;RM:读改;R:只读]</param>
        public DBColumnAttribute(string colName, bool isPriKey, DBFieldDataAccessType dataAccessType)
            : this(colName, null, isPriKey, dataAccessType)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="index">索引</param>
        /// <param name="dataAccessType">[RIM:读写改;RM:读改;R:只读]</param>
        /// <param name="isPriKey">是否是主键</param>
        public DBColumnAttribute(string colName, object[] index, bool isPriKey, DBFieldDataAccessType dataAccessType)
        {
            if (string.IsNullOrEmpty(colName))
            {
                throw new ArgumentNullException("列名不能为空或null", "colName");
            }

            this.ColumnName = colName;
            this.Index = index;
            this.IsPriKey = isPriKey;
            this.DataAccessType = dataAccessType;
        }
        #endregion

        /// <summary>
        /// 是否是主键
        /// </summary>
        private bool _isPriKey = false;

        /// <summary>
        /// 是否是主键[true:主键;false:字段;默认值:false]
        /// </summary>
        public bool IsPriKey
        {
            get { return _isPriKey; }
            set { _isPriKey = value; }
        }

        /// <summary>
        /// 字段数据访问类型
        /// </summary>
        private DBFieldDataAccessType _dataAccessType = DBFieldDataAccessType.RIM;

        /// <summary>
        /// 字段数据访问类型[RIM:读写改;RM:读改;R:只读;默认值:RIM]
        /// </summary>
        public DBFieldDataAccessType DataAccessType
        {
            get { return _dataAccessType; }
            set { _dataAccessType = value; }
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 属性索引,默认为null
        /// </summary>
        public object[] Index { get; set; }       

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return this.ColumnName;
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
