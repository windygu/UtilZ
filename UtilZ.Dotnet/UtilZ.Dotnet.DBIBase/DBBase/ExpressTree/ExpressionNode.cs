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
    public class ExpressionNode
    {
        /// <summary>
        /// 比较运算符
        /// </summary>
        public CompareOperater Operater { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        public ExpressionNodeCollection<ExpressionNode> Children { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpressionNode()
        {

        }
    }
}
