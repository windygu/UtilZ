using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBObject
{
    /// <summary>
    /// 数据模型表信息
    /// </summary>
    [Serializable]
    public class DataModelTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbTable">表信息</param>
        /// <param name="dataModelFieldInfos">数据模型所有字段信息集合</param>
        /// <param name="priKeyDataModelFieldInfos">数据模型主键字段信息集合</param>
        public DataModelTableInfo(DBTableAttribute dbTable, DataModelFieldInfoCollection dataModelFieldInfos, DataModelFieldInfoCollection priKeyDataModelFieldInfos)
        {
            this.DBTable = dbTable;
            this.DataModelFieldInfos = dataModelFieldInfos;
            this.PriKeyDataModelFieldInfos = priKeyDataModelFieldInfos;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        public DBTableAttribute DBTable { get; private set; }

        /// <summary>
        /// 获取数据模型所有字段信息集合
        /// </summary>
        public DataModelFieldInfoCollection DataModelFieldInfos { get; private set; }

        /// <summary>
        /// 获取数据模型主键字段信息集合
        /// </summary>
        public DataModelFieldInfoCollection PriKeyDataModelFieldInfos { get; private set; }
    }
}
