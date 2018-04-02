using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBModel.DBInfo
{
    /// <summary>
    /// 表信息
    /// </summary>
    [Serializable]
    public class DBTableInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="comments">注释</param>
        /// <param name="dbFieldInfos">表字段集合</param>
        /// <param name="priKeyFieldInfos">主键字段集合</param>
        public DBTableInfo(string tableName, string comments, DBFieldInfoCollection dbFieldInfos, DBFieldInfoCollection priKeyFieldInfos)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("表名不能为空或null", "tableName");
            }

            if (dbFieldInfos == null)
            {
                throw new ArgumentNullException("表字段集合不能为null", "dbFieldInfos");
            }

            this.Name = tableName;
            this.Comments = comments;
            this.DbFieldInfos = dbFieldInfos;
            this.PriKeyFieldInfos = priKeyFieldInfos;
        }

        /// <summary>
        /// 表名
        /// </summary>
        [DisplayName("表名")]
        public string Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Comments { get; private set; }

        /// <summary>
        /// 表字段集合
        /// </summary>
        [Browsable(false)]
        public DBFieldInfoCollection DbFieldInfos { get; private set; }

        /// <summary>
        /// 主键字段集合
        /// </summary>
        [Browsable(false)]
        public DBFieldInfoCollection PriKeyFieldInfos { get; private set; }

        /// <summary>
        /// 获取字段数
        /// </summary>
        [DisplayName("字段数")]
        public int Count
        {
            get { return this.DbFieldInfos.Count; }
        }

        /// <summary>
        /// 获取主键列数
        /// </summary>
        [DisplayName("主键字段数")]
        public int PriKeyCount
        {
            get { return this.PriKeyFieldInfos == null ? 0 : this.PriKeyFieldInfos.Count; }
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
