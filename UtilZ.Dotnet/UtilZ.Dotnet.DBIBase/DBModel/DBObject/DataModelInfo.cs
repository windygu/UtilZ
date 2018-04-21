using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.DBIBase.DBModel.Interface;

namespace UtilZ.Dotnet.DBIBase.DBModel.DBObject
{
    /// <summary>
    /// 数据模型信息
    /// </summary>
    [Serializable]
    public class DataModelInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbTable">表信息</param>
        /// <param name="dicDBColumnProperties">数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]</param>
        /// <param name="dicPriKeyDBColumnProperties">数据库主键列信息及对应的类的属性信息集合[key:列名;value:列信息]</param>
        /// <param name="modelValueConvert">数据模型值转换器</param>
        public DataModelInfo(DBTableAttribute dbTable, Dictionary<string, DBColumnPropertyInfo> dicDBColumnProperties, Dictionary<string, DBColumnPropertyInfo> dicPriKeyDBColumnProperties, IDBModelValueConvert modelValueConvert)
        {
            this.DBTable = dbTable;
            this.DicDBColumnProperties = dicDBColumnProperties;
            this.DicPriKeyDBColumnProperties = dicPriKeyDBColumnProperties;
            this.ModelValueConvert = modelValueConvert;

            //属性名称与列名映射字典集合[key:属性名称;value:列名]
            var propertyNameColNameMapDic = new Dictionary<string, string>();
            foreach (var kv in dicDBColumnProperties)
            {
                propertyNameColNameMapDic.Add(kv.Value.PropertyInfo.Name, kv.Key);
            }

            this.PropertyNameColNameMapDic = propertyNameColNameMapDic;
        }

        /// <summary>
        /// 表信息
        /// </summary>
        public DBTableAttribute DBTable { get; private set; }

        /// <summary>
        /// 数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
        /// </summary>
        public Dictionary<string, DBColumnPropertyInfo> DicDBColumnProperties { get; private set; }

        /// <summary>
        /// 数据库主键列信息及对应的类的属性信息集合[key:列名;value:列信息]
        /// </summary>
        public Dictionary<string, DBColumnPropertyInfo> DicPriKeyDBColumnProperties { get; private set; }

        /// <summary>
        /// 数据模型值转换器
        /// </summary>
        public IDBModelValueConvert ModelValueConvert { get; private set; }

        /// <summary>
        /// 属性名称与列名映射字典集合[key:属性名称;value:列名]
        /// </summary>
        public Dictionary<string, string> PropertyNameColNameMapDic { get; private set; }
    }
}
