using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.Foundation;
using UtilZ.Lib.Base.Log;
using UtilZ.Lib.DBModel.DBInfo;

namespace UtilZ.Lib.DBModel.Common
{
    /// <summary>
    /// 数据库辅助类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 静态构造函数,初始化
        /// </summary>
        static DBHelper()
        {
            //初始化数据库字段对应的公共运行时类型枚举集合
            DBHelper.InitFieldClrDbTypes();
        }

        /// <summary>
        /// 数据库字段对应的公共运行时类型枚举集合[key:数据库字段对应的公共运行时类型枚举;value:该类型对应的CLR类型集合]
        /// </summary>
        private static readonly Dictionary<DBFieldType, List<Type>> _dicFieldClrDbTypes = new Dictionary<DBFieldType, List<Type>>();

        /// <summary>
        /// 初始化数据库字段对应的公共运行时类型枚举集合
        /// </summary>
        private static void InitFieldClrDbTypes()
        {
            /**********************************************
             *SQLServer:有如下几种类型不支持
             Microsoft.SqlServer.Types.SqlGeography
             Microsoft.SqlServer.Types.SqlGeometry
             Microsoft.SqlServer.Types.SqlHierarchyId
            ***********************************************/

            //数值类型
            List<Type> numTypes = new List<Type>();
            numTypes.Add(ClrSystemType.BoolType);
            numTypes.Add(ClrSystemType.ByteType);
            numTypes.Add(ClrSystemType.DecimalType);
            numTypes.Add(ClrSystemType.DoubleType);
            numTypes.Add(ClrSystemType.Int16Type);
            numTypes.Add(ClrSystemType.Int32Type);
            numTypes.Add(ClrSystemType.Int64Type);
            numTypes.Add(ClrSystemType.SbyteType);
            numTypes.Add(ClrSystemType.FloatType);
            numTypes.Add(ClrSystemType.UInt16Type);
            numTypes.Add(ClrSystemType.UInt32Type);
            numTypes.Add(ClrSystemType.UInt64Type);
            DBHelper._dicFieldClrDbTypes.Add(DBFieldType.Number, numTypes);

            //二进制类型
            List<Type> binaryTypes = new List<Type>();
            binaryTypes.Add(ClrSystemType.BytesType);
            DBHelper._dicFieldClrDbTypes.Add(DBFieldType.Binary, binaryTypes);

            //日期时间类型
            List<Type> dateTypes = new List<Type>();
            dateTypes.Add(ClrSystemType.DateTimeType);
            dateTypes.Add(ClrSystemType.TimeSpanType);
            dateTypes.Add(ClrSystemType.DateTimeOffsetType);
            DBHelper._dicFieldClrDbTypes.Add(DBFieldType.DateTime, dateTypes);

            //字符串类型
            List<Type> stringTypes = new List<Type>();
            stringTypes.Add(ClrSystemType.StringType);
            stringTypes.Add(ClrSystemType.GuidType);
            DBHelper._dicFieldClrDbTypes.Add(DBFieldType.String, stringTypes);
        }

        /// <summary>
        /// 获取数据库字段对应的公共运行时类型[默认为DbClrFieldType.Other]
        /// </summary>
        /// <param name="fieldDataType">字段数据类型</param>
        /// <returns>数据库字段对应的公共运行时类型</returns>
        public static DBFieldType GetDbClrFieldType(Type fieldDataType)
        {
            foreach (var kv in DBHelper._dicFieldClrDbTypes)
            {
                if (kv.Value.Contains(fieldDataType))
                {
                    return kv.Key;
                }
            }

            return DBFieldType.Other;
        }

        #region 解析备注
        /// <summary>
        /// 解析备注
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="comments">字段备注</param>
        /// <param name="caption">标题</param>
        /// <param name="description">描述</param>
        /// <param name="mapValues">映射值</param>
        /// <returns>标题</returns>
        private static void ParseComments_bk(string fieldName, string comments, out string caption, out string description, out Dictionary<string, string> mapValues)
        {
            /************************************************
             * 示例字段描述:[T:C极化][D:C极化（ 0 左 1 右 2 未知）][M:0:左;1:右;2:未知]
             * T:标题;D:描述;M:映射集合
             * [T:C极化]
             * [D:C极化（ 0 左 1 右 2 未知）]
             * [M:0:左;1:右;2:未知]
             * 
             * 备注规则:
             * * \:为转义符,如果字符串中有]符号,则需要转义;如果映射值中有;则需要转义
             * 有T必须存在D,M可选;如果D不存在,则默认将整个备注信息用于描述
             * 无T,则无D和M,默认字段名称为标题和描述
             * 
             * 解析规则:
             * 如果字段备注为空或null,则标题和描述皆为字段名;
             * 如果没有找到[T:,则全部为标题和描述;
             * 找[T:或[D:或[M:位置,两找到之后的第一个非转义的],自此为一对.以此从中解析出TDM值.
             * M和D可选,即可能没有
             ************************************************/
            caption = null;
            description = null;
            mapValues = null;

            //如果字段备注为空或null,则标题和描述皆为字段名
            if (string.IsNullOrEmpty(comments))
            {
                caption = fieldName;
                description = fieldName;
                return;
            }

            string strT = @"[T:";
            string strD = @"[D:";
            string strM = @"[M:";
            int startIndex = comments.IndexOf(strT);
            //如果没有找到[T:,则全部为标题和描述
            if (startIndex == -1)
            {
                caption = comments;
                description = comments;
                return;
            }

            try
            {
                //找[T:或[D:或[M:位置,两找到之后的第一个非转义的],自此为一对.以此从中解析出TDM值

                //解析[T
                caption = DBHelper.CutParseStr(startIndex + strT.Length, comments);

                //解析[D
                startIndex = comments.IndexOf(strD);
                if (startIndex >= 0)
                {
                    description = DBHelper.CutParseStr(startIndex + strD.Length, comments);
                    //Replace转义回本身表达的含义
                    description = description.Replace(@"\]", "]");
                }
                else
                {
                    description = comments;
                }

                //解析[M
                startIndex = comments.IndexOf(strM);
                if (startIndex >= 0)
                {
                    string mapValueStr = DBHelper.CutParseStr(startIndex + strM.Length, comments);
                    if (!mapValueStr.EndsWith(";"))
                    {
                        mapValueStr = mapValueStr + ";";
                    }

                    mapValues = new Dictionary<string, string>();
                    string tmpStr = null;
                    int beginIndex = 0;
                    int colonIndex = -1;
                    string key, value;
                    //[M:0:左;1:右;2:未知]
                    for (int i = beginIndex; i < mapValueStr.Length; i++)
                    {
                        if (mapValueStr[i] == ';' && mapValueStr[i - 1] != '\\')
                        {
                            tmpStr = mapValueStr.Substring(beginIndex, i - beginIndex);
                            //如果是两个相邻的字符都是;,则略过
                            if (string.IsNullOrEmpty(tmpStr))
                            {
                                continue;
                            }

                            colonIndex = tmpStr.IndexOf(':');
                            //如果不包含:号,则此映射值段字符串视为无效串
                            if (colonIndex < 0)
                            {
                                continue;
                            }

                            //Replace转义回本身表达的含义
                            key = tmpStr.Substring(0, colonIndex).Replace(@"\;", ";");
                            value = tmpStr.Substring(colonIndex + 1).Replace(@"\;", ";");
                            mapValues.Add(key, value);

                            //此处++是为了略过分号;
                            beginIndex = ++i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                caption = fieldName;
                description = fieldName;
                Loger.Debug(ex);
            }
        }

        /// <summary>
        /// 解析备注
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="comments">字段备注</param>
        /// <param name="caption">标题</param>
        /// <param name="description">描述</param>
        /// <returns>标题</returns>
        public static void ParseComments(string fieldName, string comments, out string caption, out string description)
        {
            /************************************************
             * 示例字段描述:[T:C极化][D:C极化（ 0 左 1 右 2 未知）]
             * T:标题;D:描述;M:映射集合
             * [T:C极化]
             * [D:C极化（ 0 左 1 右 2 未知）]
             * [M:0:左;1:右;2:未知]
             * 
             * 备注规则:
             * * \:为转义符,如果字符串中有]符号,则需要转义;如果映射值中有;则需要转义
             * 有T必须存在D,M可选;如果D不存在,则默认将整个备注信息用于描述
             * 无T,则无D和M,默认字段名称为标题和描述
             * 
             * 解析规则:
             * 如果字段备注为空或null,则标题和描述皆为字段名;
             * 如果没有找到[T:,则全部为标题和描述;
             * 找[T:或[D:或[M:位置,两找到之后的第一个非转义的],自此为一对.以此从中解析出TDM值.
             * M和D可选,即可能没有
             ************************************************/
            caption = null;
            description = null;

            //如果字段备注为空或null,则标题和描述皆为字段名
            if (string.IsNullOrEmpty(comments))
            {
                caption = fieldName;
                description = fieldName;
                return;
            }

            string strT = @"[T:";
            string strD = @"[D:";
            int startIndex = comments.IndexOf(strT);
            //如果没有找到[T:,则全部为标题和描述
            if (startIndex == -1)
            {
                caption = comments;
                description = comments;
                return;
            }

            try
            {
                //找[T:或[D:或[M:位置,两找到之后的第一个非转义的],自此为一对.以此从中解析出TDM值

                //解析[T
                caption = DBHelper.CutParseStr(startIndex + strT.Length, comments);

                //解析[D
                startIndex = comments.IndexOf(strD);
                if (startIndex >= 0)
                {
                    description = DBHelper.CutParseStr(startIndex + strD.Length, comments);
                    //Replace转义回本身表达的含义
                    description = description.Replace(@"\]", "]");
                }
                else
                {
                    description = comments;
                }
            }
            catch (Exception ex)
            {
                caption = fieldName;
                description = fieldName;
                Loger.Debug(ex);
            }
        }

        /// <summary>
        /// 截取解析的字符串
        /// </summary>
        /// <param name="startIndex">起始索引</param>
        /// <param name="comments">备注字符串</param>
        /// <returns>截取到的结果字符串</returns>
        private static string CutParseStr(int startIndex, string comments)
        {
            string targetStr = null;
            if (startIndex >= 0)
            {
                for (int i = startIndex; i < comments.Length; i++)
                {
                    if (comments[i] == ']' && comments[i - 1] != '\\')
                    {
                        targetStr = comments.Substring(startIndex, i - startIndex);
                        break;
                    }
                }

                if (string.IsNullOrEmpty(targetStr))
                {
                    targetStr = comments.Substring(startIndex);
                }
            }
            else
            {
                targetStr = comments;
            }

            return targetStr;
        }
        #endregion

        /*
        #region 生成条件SQL
        /// <summary>
        /// 生成Oracle无参数SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paraSign">参数符号</param>
        /// <param name="conditionGroupCollection">条件组集合</param>
        /// <param name="queryFields">要查询的字段名称集合,为null或空时全查</param>
        /// <returns>Oracle无参数SQL语句</returns>
        public static string GenerateNoParaOracleSql(string tableName, string paraSign, ConditionGroupCollection conditionGroupCollection, List<string> queryFields = null)
        {
            var conditionGenerator = new OracleConditionGenerator(tableName, paraSign, conditionGroupCollection, 0, queryFields);
            return conditionGenerator.GenerateNoParaConditionOracleSql();
        }

        /// <summary>
        /// 生成Oracle无参数SQL语句
        /// </summary>
        /// <param name="conditionGroupCollection">条件组集合</param>
        /// <returns>Oracle无参数SQL语句</returns>
        public static string GenerateNoParaOracleWhere(ConditionGroupCollection conditionGroupCollection)
        {
            var conditionGenerator = new OracleConditionGenerator(null, null, conditionGroupCollection, 0, null);
            return conditionGenerator.GenerateNoParaConditionOracleWhere();
        }

        /// <summary>
        /// 生成Oracle带参数SQL语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paraSign">参数符号</param>
        /// <param name="conditionGroupCollection">条件组集合</param>
        /// <param name="paraIndex">参数索引</param>
        /// <param name="queryFields">要查询的字段名称集合,为null或空时全查</param>
        /// <returns>Oracle带参数SQL语句对象</returns>
        public static DBSqlInfo GenerateParaSql(string tableName, string paraSign, ConditionGroupCollection conditionGroupCollection, IEnumerable<string> queryFields = null)
        {
            var conditionGenerator = new OracleConditionGenerator(tableName, paraSign, conditionGroupCollection, 1, queryFields);
            return conditionGenerator.GenerateParaSql();
        }

        /// <summary>
        /// 生成Oracle带参数SQL语句
        /// </summary>
        /// <param name="paraSign">参数符号</param>
        /// <param name="conditionGroupCollection">条件组集合</param>
        /// <param name="paraIndex">参数索引</param>
        /// <returns>Oracle带参数SQL语句对象</returns>
        public static DBSqlInfo GenerateParaWhere(string paraSign, ConditionGroupCollection conditionGroupCollection, ref int paraIndex)
        {
            var conditionGenerator = new OracleConditionGenerator(null, paraSign, conditionGroupCollection, paraIndex, null);
            DBSqlInfo dbWhereInfo = conditionGenerator.GenerateParaWhere();
            paraIndex = conditionGenerator.ParaIndex;
            return dbWhereInfo;
        }

        /// <summary>
        /// 生成Oracle带参数SQL语句
        /// </summary>
        /// <param name="paraSign">参数符号</param>
        /// <param name="conditionGroupCollection">条件组集合</param>
        /// <returns>Oracle带参数SQL语句对象</returns>
        public static DBSqlInfo GenerateParaWhere(string paraSign, ConditionGroupCollection conditionGroupCollection)
        {
            var conditionGenerator = new OracleConditionGenerator(null, paraSign, conditionGroupCollection, 1, null);
            DBSqlInfo dbWhereInfo = conditionGenerator.GenerateParaWhere();
            return dbWhereInfo;
        }
        #endregion
    */
    }
}
