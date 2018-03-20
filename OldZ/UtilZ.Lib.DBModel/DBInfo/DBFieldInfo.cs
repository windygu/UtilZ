using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.Common;

namespace UtilZ.Lib.DBModel.DBInfo
{
    /// <summary>
    /// 数据库表字段信息
    /// </summary>
    [Serializable]
    public class DBFieldInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbFieldInfo">数据库表字段信息</param>
        public DBFieldInfo(DBFieldInfo dbFieldInfo)
            : this(dbFieldInfo.OwerTableName, dbFieldInfo.Caption, dbFieldInfo.Description, dbFieldInfo.FiledName, dbFieldInfo.DbTypeName, dbFieldInfo.DataType, dbFieldInfo.Comments, dbFieldInfo.DefaultValue, dbFieldInfo.AllowNull, dbFieldInfo.FieldType, dbFieldInfo.IsPriKey)
        {
            //this.Caption = dbFieldInfo.Caption;
            //this.Description = dbFieldInfo.Description;
            //this.MapValues = dbFieldInfo.MapValues;
            //this.FiledName = dbFieldInfo.FiledName;
            //this.DbTypeName = dbFieldInfo.DbTypeName;
            //this.DataType = dbFieldInfo.DataType;
            //this.Comments = dbFieldInfo.Comments;
            //this.DefaultValue = dbFieldInfo.DefaultValue;
            //this.AllowNull = dbFieldInfo.AllowNull;
            //this.FieldType = dbFieldInfo.FieldType;
            //this.IsPriKey = dbFieldInfo.IsPriKey;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owerTableName">所属表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="dbTypeName">数据库数据类型名称</param>
        /// <param name="dataType">数据库字段对应于.net平台的托管类型</param>
        /// <param name="isPriKey">是否是主键</param>
        public DBFieldInfo(string owerTableName, string fieldName, string dbTypeName, Type dataType, bool isPriKey)
            : this(owerTableName, fieldName, null, fieldName, dbTypeName, dataType, null, null, true, DBHelper.GetDbClrFieldType(dataType), isPriKey)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owerTableName">所属表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="dbTypeName">数据库数据类型名称</param>
        /// <param name="dataType">数据库字段对应于.net平台的托管类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="allowNull">是否允许为空值</param>
        /// <param name="fieldType">运行时数据类型</param>
        /// <param name="isPriKey">是否是主键</param>
        public DBFieldInfo(string owerTableName, string fieldName, string dbTypeName, Type dataType, object defaultValue, bool allowNull, DBFieldType fieldType, bool isPriKey)
            : this(owerTableName, fieldName, null, fieldName, dbTypeName, dataType, null, defaultValue, allowNull, fieldType, isPriKey)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owerTableName">所属表名</param>
        /// <param name="caption">标题</param>
        /// <param name="description">描述</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="dbTypeName">数据类型</param>
        /// <param name="dataType">数据库字段对应于.net平台的托管类型</param>
        /// <param name="comments">注释</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="allowNull">是否允许为空值</param>
        /// <param name="fieldType">运行时数据类型</param>
        /// <param name="isPriKey">是否是主键</param>
        public DBFieldInfo(string owerTableName, string caption, string description, string fieldName, string dbTypeName, Type dataType, string comments, object defaultValue, bool allowNull, DBFieldType fieldType, bool isPriKey)
        {
            if (string.IsNullOrEmpty(owerTableName))
            {
                throw new ArgumentNullException("owerTableName");
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }

            if (string.IsNullOrEmpty(dbTypeName))
            {
                throw new ArgumentNullException("dbTypeName");
            }

            if (dataType == null)
            {
                throw new ArgumentNullException("clrType");
            }

            this.OwerTableName = owerTableName;
            if (string.IsNullOrEmpty(caption))
            {
                this.Caption = fieldName;
            }
            else
            {
                this.Caption = caption;
            }

            if (string.IsNullOrEmpty(description))
            {
                this.Description = comments;
            }
            else
            {
                this.Description = description;
            }

            this.FiledName = fieldName;
            this.DbTypeName = dbTypeName;
            this.DataType = dataType;
            this.Comments = comments;
            this.DefaultValue = defaultValue;
            this.AllowNull = allowNull;
            this.FieldType = fieldType;
            this.IsPriKey = isPriKey;
        }

        /// <summary>
        /// 所属表名
        /// </summary>
        [DisplayName("所属表名")]
        public string OwerTableName { get; private set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [DisplayName("字段名")]
        public string FiledName { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        public string Caption { get; private set; }

        /// <summary>
        /// 数据库类型[字段数据库中对应的数据类型名称]
        /// </summary>
        [DisplayName("数据库类型")]
        public string DbTypeName { get; private set; }

        /// <summary>
        /// 数据库字段对应于.net平台运行时数据类型
        /// </summary>
        [DisplayName("数据类型")]
        public Type DataType { get; private set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [DisplayName("字段类型")]
        public DBFieldType FieldType { get; private set; }

        /// <summary>
        /// 是否允许为空值
        /// </summary>
        [DisplayName("是否允许为空值")]
        public bool AllowNull { get; private set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [DisplayName("默认值")]
        public object DefaultValue { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Description { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Comments { get; private set; }

        /// <summary>
        /// 是否是主键字段[true:主键字段;false:非主键字段]
        /// </summary>
        [Browsable(false)]
        public bool IsPriKey { get; private set; }

        /// <summary>
        /// 是否是主键字段[Y:是;N:数据字段]
        /// </summary>
        [DisplayName("是否是主键字段")]
        public string IsPriKeyStr { get { return this.IsPriKey ? "Y" : "N"; } }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return string.Format("字段名称:{0};数据类型:{1};备注:{2}", this.FiledName, this.DbTypeName, this.Comments);
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 重写Equals方法[相等返回true;不等返回false]
        /// </summary>
        /// <param name="obj">要比较的对象</param>
        /// <returns>相等返回true;不等返回false</returns>
        public override bool Equals(object obj)
        {
            DBFieldInfo exObj = obj as DBFieldInfo;
            if (exObj == null)
            {
                return false;
            }

            if (!this.FiledName.Equals(exObj.FiledName))
            {
                return false;
            }

            if (!this.DbTypeName.Equals(exObj.DbTypeName))
            {
                return false;
            }

            return true;
        }
    }
}
