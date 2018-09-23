using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBIBase.DBModel.Model
{
    /// <summary>
    /// 表示System.Data.Common.DbCommand的参数
    /// </summary>
    [Serializable]
    public class NDbParameter
    {
        #region 
        /// <summary>
        /// 构造类型1
        /// </summary>
        private const byte StructType1 = 1;

        /// <summary>
        /// 构造类型2
        /// </summary>
        private const byte StructType2 = 2;

        /// <summary>
        /// 构造类型3
        /// </summary>
        private const byte StructType3 = 3;

        /// <summary>
        /// 构造类型4
        /// </summary>
        private const byte StructType4 = 4;

        /// <summary>
        /// 构造类型5
        /// </summary>
        private const byte StructType5 = 5;

        /// <summary>
        /// 构造类型6
        /// </summary>
        private const byte StructType6 = 6;
        #endregion

        /// <summary>
        /// 构造函数类型ID
        /// </summary>
        private readonly byte _constructedTypeID;

        /// <summary>
        /// 先有构造函数,生成hashcode
        /// </summary>
        private NDbParameter()
        {
            this._hashCode = Guid.NewGuid().ToString().GetHashCode();
        }

        /// <summary>
        /// 构造函数[此构造函数仅用于调用存储过程中的输出或返回值参数]
        /// </summary>
        /// <param name="size">参数的长度</param>
        /// <param name="parameterName">参数名称</param>        
        /// <param name="direction">参数方向</param>
        public NDbParameter(int size, string parameterName, ParameterDirection direction)
            : this()
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException("parameterName不能为空或null", "parameterName");
            }

            if (size <= 0)
            {
                throw new ArgumentException("参数的长度不能为0或负数", "size");
            }

            if (direction != ParameterDirection.Output
                && direction != ParameterDirection.InputOutput
                && direction != ParameterDirection.ReturnValue)
            {
                throw new ArgumentException("参数方向只能为输出或输入输出或返回值类型", "direction");
            }

            this.Size = size;
            this.ParameterName = parameterName;
            this.Direction = direction;
            this._constructedTypeID = StructType1;
        }

        /// <summary>
        /// 构造函数[此构造函数仅用于调用存储过程中的输出或返回值参数]
        /// </summary>
        /// <param name="size">参数的长度</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="direction">参数方向</param>
        /// <param name="dbType">参数类型</param>
        public NDbParameter(int size, string parameterName, ParameterDirection direction, DbType dbType)
            : this()
        {
            if (size <= 0)
            {
                throw new ArgumentException("参数的长度不能为0或负数", "size");
            }

            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException("parameterName不能为空或null", "parameterName");
            }

            if (direction != ParameterDirection.Output
                && direction != ParameterDirection.InputOutput
                && direction != ParameterDirection.ReturnValue)
            {
                throw new ArgumentException("参数方向只能为输出或输入输出或返回值类型", "direction");
            }

            this.Size = size;
            this.ParameterName = parameterName;
            this.DbType = dbType;
            this.Direction = direction;
            this._constructedTypeID = StructType2;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        public NDbParameter(string parameterName, object value)
            : this(parameterName, value, ParameterDirection.Input)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">值</param>
        /// <param name="direction">参数方向</param>
        public NDbParameter(string parameterName, object value, ParameterDirection direction)
            : this()
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException("parameterName不能为空或null", "parameterName");
            }

            this.ParameterName = parameterName;
            this.Value = value;
            this.Direction = direction;
            this._constructedTypeID = StructType3;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">值</param>
        public NDbParameter(string parameterName, DbType dbType, object value)
            : this(parameterName, dbType, value, ParameterDirection.Input)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="value">值</param>
        /// <param name="direction">参数方向</param>
        public NDbParameter(string parameterName, DbType dbType, object value, ParameterDirection direction)
            : this()
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException("parameterName不能为空或null", "parameterName");
            }

            this.ParameterName = parameterName;
            this.DbType = dbType;
            this.Value = value;
            this.Direction = direction;
            this._constructedTypeID = StructType4;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数的长度</param>
        /// <param name="value">值</param>
        public NDbParameter(string parameterName, DbType dbType, int size, object value)
            : this(parameterName, dbType, size, ParameterDirection.Input, value)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数的长度</param>
        /// <param name="direction">参数方向</param>
        /// <param name="value">值</param>
        public NDbParameter(string parameterName, DbType dbType, int size, ParameterDirection direction, object value)
            : this()
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentException("parameterName不能为空或null", "parameterName");
            }

            if (size <= 0)
            {
                throw new ArgumentException("参数的长度不能为0或负数", "size");
            }

            this.ParameterName = parameterName;
            this.DbType = dbType;
            this.Size = size;
            this.Value = value;
            this.Direction = direction;
            this._constructedTypeID = StructType5;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public NDbParameter(DbParameter parameter)
            : this()
        {
            if (parameter == null)
            {
                throw new ArgumentException("命令参数不能为null", "parameter");
            }

            this.DbType = parameter.DbType;
            this.Direction = parameter.Direction;
            //this.IsNullable = parameter.IsNullable;
            this.ParameterName = parameter.ParameterName;
            this.Size = parameter.Size;
            this.SourceColumn = parameter.SourceColumn;
            this.SourceColumnNullMapping = parameter.SourceColumnNullMapping;
            this.SourceVersion = parameter.SourceVersion;
            this.Value = parameter.Value;
            this._constructedTypeID = StructType6;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameter">命令参数</param>
        public NDbParameter(NDbParameter parameter)
            : this()
        {
            if (parameter == null)
            {
                throw new ArgumentException("命令参数不能为null", "parameter");
            }

            this.DbType = parameter.DbType;
            this.Direction = parameter.Direction;
            //this.IsNullable = parameter.IsNullable;
            this.ParameterName = parameter.ParameterName;
            this.Size = parameter.Size;
            this.SourceColumn = parameter.SourceColumn;
            this.SourceColumnNullMapping = parameter.SourceColumnNullMapping;
            this.SourceVersion = parameter.SourceVersion;
            this.Value = parameter.Value;
            this._constructedTypeID = 6;
        }

        /// <summary>
        /// 获取或设置参数的 DbType
        /// </summary>
        public System.Data.DbType DbType { get; private set; }

        /// <summary>
        /// 获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数
        /// </summary>
        public System.Data.ParameterDirection Direction { get; private set; }

        /// <summary>
        /// 获取或设置 DbParameter 的名称
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// 获取或设置列中数据的最大大小（以字节为单位）。
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 获取或设置该参数的值
        /// </summary>
        public object Value { get; set; }

        ///// <summary>
        ///// 指示参数是否接受空值
        ///// </summary>
        //private bool _isNullable = false;

        ///// <summary>
        ///// 获取或设置一个值，该值指示参数是否接受空值[默认值为false]
        ///// </summary>
        //public bool IsNullable
        //{
        //    get { return _isNullable; }
        //    set { _isNullable = value; }
        //}

        /// <summary>
        /// 源列的名称，该源列映射到 DataSet 并用于加载或返回 Value
        /// </summary>
        private string _sourceColumn = string.Empty;

        /// <summary>
        /// 获取或设置源列的名称，该源列映射到 DataSet 并用于加载或返回 Value[默认值为空字符串]
        /// </summary>
        public string SourceColumn
        {
            get { return _sourceColumn; }
            set { _sourceColumn = value; }
        }

        /// <summary>
        /// 设置或获取一个值，该值指示源列是否可以为 null。 这使得 DbCommandBuilder 能够正确地为可以为 null 的列生成 Update 语句
        /// </summary>
        public bool SourceColumnNullMapping { get; set; }

        /// <summary>
        /// 加载Value时使用的 DataRowVersion
        /// </summary>
        private System.Data.DataRowVersion _sourceVersion = DataRowVersion.Current;

        /// <summary>
        /// 获取或设置在加载Value时使用的 DataRowVersion[默认值为Current]
        /// </summary>
        public System.Data.DataRowVersion SourceVersion
        {
            get { return _sourceVersion; }
            set { _sourceVersion = value; }
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.ParameterName, this.Value);
        }

        private readonly int _hashCode = 0;

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return this._hashCode;
        }

        /// <summary>
        /// 将参数值设置到指定的IDbDataParameter中
        /// </summary>
        /// <param name="parameter">IDbDataParameter</param>
        public void SetValueToParameter(IDbDataParameter parameter)
        {
            switch (this._constructedTypeID)
            {
                case StructType1:
                    parameter.Size = this.Size;
                    parameter.ParameterName = this.ParameterName;
                    parameter.Direction = this.Direction;
                    break;
                case StructType2:
                    parameter.Size = this.Size;
                    parameter.ParameterName = this.ParameterName;
                    parameter.DbType = this.DbType;
                    parameter.Direction = this.Direction;
                    break;
                case StructType3:
                    parameter.ParameterName = this.ParameterName;
                    parameter.Value = this.Value;
                    parameter.Direction = this.Direction;
                    break;
                case StructType4:
                    parameter.ParameterName = this.ParameterName;
                    parameter.DbType = this.DbType;
                    parameter.Value = this.Value;
                    parameter.Direction = this.Direction;
                    break;
                case StructType5:
                    parameter.ParameterName = this.ParameterName;
                    parameter.DbType = this.DbType;
                    parameter.Size = this.Size;
                    parameter.Value = this.Value;
                    parameter.Direction = this.Direction;
                    break;
                case StructType6:
                    parameter.DbType = this.DbType;
                    parameter.Direction = this.Direction;
                    //parameter.IsNullable = this.IsNullable;
                    parameter.ParameterName = this.ParameterName;
                    parameter.Size = this.Size;
                    parameter.SourceColumn = this.SourceColumn;
                    //parameter.SourceColumnNullMapping = this.SourceColumnNullMapping;
                    parameter.SourceVersion = this.SourceVersion;
                    parameter.Value = this.Value;
                    break;
                default:
                    parameter.ParameterName = this.ParameterName;
                    parameter.Value = this.Value;
                    break;
            }
        }
    }
}
