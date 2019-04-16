using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.DBIBase.DBBase.ExpressTree
{
    /// <summary>
    /// 比较运算符
    /// </summary>
    public enum CompareOperater
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 1,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 2,

        /// <summary>
        /// 小于
        /// </summary>
        Less = 3,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual = 4,

        /// <summary>
        /// 大于
        /// </summary>
        Greater = 5,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual = 6,

        /// <summary>
        /// 存在[eg:=>field in (1,2,3)]
        /// </summary>
        In = 7,

        /// <summary>
        /// 不存在[eg:=>field not in (1,2,3)]
        /// </summary>
        NotIn = 8,

        /// <summary>
        /// 字符串包含[eg:字符串包含abc=>"%abc%"]
        /// </summary>
        Like = 9,

        /// <summary>
        /// 字符串左侧包含[eg:字符串左侧包含abc=>"%abc"]
        /// </summary>
        LeftLike = 10,

        /// <summary>
        /// 字符串右侧包含[eg:字符串右侧包含abc=>"abc%"]
        /// </summary>
        RightLike = 11,

        /// <summary>
        /// 不在某个范围[eg:=>field<=20 or filed>=100]
        /// </summary>
        NotInRange = 12,
    }
}
