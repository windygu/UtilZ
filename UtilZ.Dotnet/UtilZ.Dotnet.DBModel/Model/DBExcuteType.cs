using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBModel.Model
{
    /// <summary>
    /// 执行类型
    /// </summary>
    public enum DBExcuteType
    {
        /// <summary>
        /// 执行语句,返回DataSet
        /// </summary>
        Query,

        /// <summary>
        /// 执行语句,返回受影响的行数
        /// </summary>
        NonQuery,

        /// <summary>
        /// 执行语句,返回执行结果的第一行第一列
        /// </summary>
        Scalar
    }
}
