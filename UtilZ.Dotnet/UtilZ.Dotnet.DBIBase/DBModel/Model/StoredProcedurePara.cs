using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBIBase.DBModel.Model
{
    /// <summary>
    /// 存储过程调用参数
    /// </summary>
    [Serializable]
    public class StoredProcedurePara : NDbParameterCollection
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public StoredProcedurePara()
        {
            this._excuteType = DBExcuteType.Query;
            this.VisitType = DBVisitType.R;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection">参数集合</param>
        public StoredProcedurePara(NDbParameterCollection collection)
            : base(collection)
        {
            this.VisitType = DBVisitType.R;

            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 数据库访问类型,默认为读
        /// </summary>
        public DBVisitType VisitType { get; set; }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string StoredProcedureName { get; set; }

        /// <summary>
        /// 执行类型
        /// </summary>
        private DBExcuteType _excuteType;

        /// <summary>
        /// 执行类型[默认为查询]
        /// </summary>
        public DBExcuteType ExcuteType
        {
            get { return _excuteType; }
            set { _excuteType = value; }
        }

        /// <summary>
        /// 将某项添加到集合中
        /// </summary>
        /// <param name="item">要添加到集合的对象</param>
        public override void Add(NDbParameter item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("要添加到集合的对象不能为null", "item");
            }

            //检查返回值参数
            if (item.Direction == System.Data.ParameterDirection.ReturnValue && this.Count > 0)
            {
                //是否存在返回值参数
                bool isExistReturnPara = (from tmpItem in this where tmpItem.Direction == System.Data.ParameterDirection.ReturnValue select tmpItem).Count() > 0;
                if (isExistReturnPara)
                {
                    throw new ArgumentException(string.Format("已存在一个返回值类型的参数{0},一个存储过程参数中只能存在最多一个返回值类型的参数", item.ParameterName), "item");
                }
            }

            base.Add(item);
        }
    }
}
