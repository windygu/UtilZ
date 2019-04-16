using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.Dotnet.DBIBase.DBModel.DBInfo;
using UtilZ.Dotnet.DBIBase.DBModel.Model;

namespace UtilZ.Dotnet.DBIBase.DBBase.ExpressTree
{
    /// <summary>
    /// 单值条件生成器
    /// </summary>
    public class SingleValueConditionValueGenerator : AbsCompareOperaterWhereGenerator
    {
        public SingleValueConditionValueGenerator()
            : base()
        {

        }

        /// <summary>
        /// 生成条件值
        /// </summary>
        /// <param name="node">表达式节点</param>
        /// <param name="parameters">命令参数集合</param>
        /// <param name="paraSign">数据库参数字符</param>
        /// <returns>条件值</returns>
        public override string Generate(ExpressionNode node, NDbParameterCollection parameters, string paraSign)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(node.TableInfo.Name);
            //sb.Append('.');
            //sb.Append(node.FieldInfo.FiledName);
            //CompareOperaterAttribute compareOperaterAttribute = CompareOperaterHelper.GetCompareOperaterAttributeByCompareOperater(node.Operater);
            //sb.Append(compareOperaterAttribute.Operater);

            //string paraName;

            //switch (node.FieldInfo.FieldType)
            //{
            //    case DBFieldType.Binary:

            //        break;
            //    case DBFieldType.DateTime:
            //        paraName = string.Format("{0}{1}", node.FieldInfo.FiledName, parameters.Count);
            //        sb.Append(paraSign);
            //        sb.Append(paraName);
            //        parameters.Add(paraName, node.Value);
            //        break;
            //    case DBFieldType.Number:
            //        break;
            //    case DBFieldType.String:
            //        break;
            //    case DBFieldType.Other:
            //        break;
            //    default:
            //        throw new NotImplementedException($"未实现的数据库字段类型[{filedAttribute.FiledType.ToString()}]");
            //}

            return sb.ToString();
        }
    }
}
