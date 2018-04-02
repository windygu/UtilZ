using System;
using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.DBModel.Model
{
    /// <summary>
    /// 排序信息
    /// </summary>
    [Serializable]
    public class DBOrderInfo : BaseModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="orderFlag">排序标识[true:升序;false:降序]</param>
        public DBOrderInfo(string fieldName, bool orderFlag)
        {
            this.FieldName = fieldName;
            this.OrderFlag = orderFlag;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        [DisplayNameExAttribute("字段名称")]
        public string FieldName { get; private set; }

        /// <summary>
        /// 排序标识[true:升序;false:降序]
        /// </summary>
        [DisplayNameExAttribute("排序标识")]
        public bool OrderFlag { get; private set; }
    }
}
