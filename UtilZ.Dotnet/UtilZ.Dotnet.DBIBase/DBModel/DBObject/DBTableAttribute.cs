using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBIBase.DBModel.DBObject
{
    /// <summary>
    /// 数据库表信息特性
    /// </summary>
    [Serializable]
    public class DBTableAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">表名</param>
        public DBTableAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 数据模型中属性的值与数据库表中字段对应的值转换器,为空则该实体类不需要转换
        /// </summary>
        public string ValueConvertor { get; set; }
    }
}
