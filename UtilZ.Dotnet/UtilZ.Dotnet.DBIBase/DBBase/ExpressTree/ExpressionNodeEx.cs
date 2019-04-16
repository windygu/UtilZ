using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;

namespace UtilZ.Dotnet.DBIBase.DBBase.ExpressTree
{
    /// <summary>
    /// 表达式节点
    /// </summary>
    [Serializable]
    public class ExpressionNodeEx
    {
        /// <summary>
        /// 比较运算符
        /// </summary>
        public CompareOperater Operater { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public DBTableInfo TableInfo { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public DBFieldInfo FieldInfo { get; set; }

        /// <summary>
        /// 参数值组合字符串
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        public ExpressionNodeCollection<ExpressionNodeEx> Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpressionNodeEx()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="node">外部表达式树</param>
        public ExpressionNodeEx(ExpressionNode rootNode)
        {

        }
    }
}
