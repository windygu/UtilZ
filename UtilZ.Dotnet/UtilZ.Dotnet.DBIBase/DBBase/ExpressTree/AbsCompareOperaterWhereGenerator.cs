using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBIBase.DBBase.ExpressTree
{
    /// <summary>
    /// 单个节点条件值生成器接口
    /// </summary>
    public abstract class AbsCompareOperaterWhereGenerator : ICompareOperaterWhereGenerator
    {
        public AbsCompareOperaterWhereGenerator()
        {

        }

        /// <summary>
        /// 生成条件值
        /// </summary>
        /// <param name="node">表达式节点</param>
        /// <param name="parameters">命令参数集合</param>
        /// <param name="paraSign">数据库参数字符</param>
        /// <returns>条件值</returns>
        public abstract string Generate(ExpressionNode node, NDbParameterCollection parameters, string paraSign);
    }
}
