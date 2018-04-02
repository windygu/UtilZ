using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.DBModel.Model
{
    /// <summary>
    /// 存储过程调用结果
    /// </summary>
    [Serializable]
    public class StoredProcedureResult
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public StoredProcedureResult()
        {
            this.ParaValue = new Dictionary<NDbParameter, object>();
        }

        /// <summary>
        /// 返回值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public Dictionary<NDbParameter, object> ParaValue { get; private set; }
    }
}
