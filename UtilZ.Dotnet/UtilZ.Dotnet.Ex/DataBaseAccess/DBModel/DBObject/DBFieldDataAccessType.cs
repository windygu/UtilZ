using UtilZ.Dotnet.Ex.Model;

namespace UtilZ.Dotnet.Ex.DataBaseAccess.DBModel.DBObject
{
    /// <summary>
    /// 字段数据访问类型
    /// </summary>
    public enum DBFieldDataAccessType
    {
        //R:读;I:插入;M:修改

        /// <summary>
        /// 读写改
        /// </summary>
        [DisplayNameExAttribute("读写改")]
        RIM,

        /// <summary>
        /// 读改
        /// </summary>
        [DisplayNameExAttribute("读改")]
        RM,

        /// <summary>
        /// 只读
        /// </summary>
        [DisplayNameExAttribute("只读")]
        R,

        /// <summary>
        /// 插入读
        /// </summary>
        [DisplayNameExAttribute("插入读")]
        IR,

        /// <summary>
        /// 只写
        /// </summary>
        [DisplayNameExAttribute("只写")]
        I,

        /// <summary>
        /// 改写
        /// </summary>
        [DisplayNameExAttribute("改写")]
        IM
    }
}
