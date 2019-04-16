using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.Ex.Attributes;

namespace UtilZ.Dotnet.DBIBase.DBModel.Model
{
    /// <summary>
    /// 逻辑运算符枚举
    /// </summary>
    public enum LogicOperaters
    {
        /// <summary>
        /// 且
        /// </summary>
        [DisplayNameExAttribute("且")]
        And = 1,

        /// <summary>
        /// 或
        /// </summary>
        [DisplayNameExAttribute("或")]
        Or = 2
    }
}
