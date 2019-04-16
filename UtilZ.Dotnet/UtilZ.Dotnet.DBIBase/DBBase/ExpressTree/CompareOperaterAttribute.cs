using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilZ.Dotnet.DBIBase.DBBase.ExpressTree
{
    /// <summary>
    /// 比较运算答特性
    /// </summary>
    public class CompareOperaterAttribute : Attribute
    {
        /// <summary>
        /// 比较运算符
        /// </summary>
        public string Operater { get; private set; }

        /// <summary>
        /// 条件值生成器
        /// </summary>
        public ICompareOperaterWhereGenerator ConditionValueGenerator { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operater">比较运算符</param>
        /// <param name="compareOperaterWhereGeneratorType">条件值生成器类型</param>
        public CompareOperaterAttribute(string operater, Type compareOperaterWhereGeneratorType)
        {
            this.Operater = operater;
            this.ConditionValueGenerator = (ICompareOperaterWhereGenerator)Activator.CreateInstance(compareOperaterWhereGeneratorType);
        }
    }
}
