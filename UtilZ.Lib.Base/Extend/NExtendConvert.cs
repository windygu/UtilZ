using UtilZ.Lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// Convert类型扩展方法类
    /// </summary>
    public static class NExtendConvert
    {
        /// <summary>
        /// 转换数据到
        /// </summary>
        /// <param name="targetValueType">要待转换的目标类型</param>
        /// <param name="value">要转换的值</param>
        /// <returns>转换后的值,存放在object中,如果转换失败为目标类型的默认值</returns>
        public static object ToObject(Type targetValueType, object value)
        {
            object resultValue = null;
            try
            {
                resultValue = Convert.ChangeType(value, targetValueType);
            }
            catch (Exception ex)
            {
                if (targetValueType.IsEnum)
                {
                    resultValue = Enum.Parse(targetValueType, value.ToString());
                }
                else
                {
                    Console.WriteLine(ex.ToString());
                    //默认不转换
                    resultValue = value;
                }
            }

            return resultValue;
        }

        /// <summary>
        /// 转换数据到
        /// </summary>
        /// <param name="targetValueType">要待转换的目标类型</param>
        /// <param name="value">要转换的值</param>
        /// <param name="fromBase">值中数字基数,必须是2,8,10,16</param>
        /// <returns>转换后的值,存放在object中,如果转换失败为目标类型的默认值</returns>
        public static object ToObject(Type targetValueType, string value, byte fromBase)
        {
            object resultValue = null;
            TypeCode code = Type.GetTypeCode(targetValueType);
            switch (code)
            {
                case TypeCode.Byte:
                    resultValue = Convert.ToByte(value, fromBase);
                    break;
                case TypeCode.Int16:
                    resultValue = Convert.ToInt16(value, fromBase);
                    break;
                case TypeCode.Int32:
                    resultValue = Convert.ToInt32(value, fromBase);
                    break;
                case TypeCode.Int64:
                    resultValue = Convert.ToInt64(value, fromBase);
                    break;
                case TypeCode.SByte:
                    resultValue = Convert.ToSByte(value, fromBase);
                    break;
                case TypeCode.UInt16:
                    resultValue = Convert.ToUInt16(value, fromBase);
                    break;
                case TypeCode.UInt32:
                    resultValue = Convert.ToUInt32(value, fromBase);
                    break;
                case TypeCode.UInt64:
                    resultValue = Convert.ToUInt64(value, fromBase);
                    break;
                default:
                    //默认为输入的字符串
                    resultValue = value;
                    break;
            }

            return resultValue;
        }

        /// <summary>
        /// 尝试转换数据类型到指定类型[转换成功返回true,转换失败返回false]
        /// </summary>
        /// <param name="targetValueType">要待转换的目标类型</param>
        /// <param name="value">要转换的值</param>
        /// <param name="targetValue">输出目标值</param>
        /// <returns>转换成功返回true,转换失败返回false</returns>
        public static bool TryParse(Type targetValueType, string value, out IFormattable targetValue)
        {
            bool result = false;
            targetValue = null;
            TypeCode code = Type.GetTypeCode(targetValueType);
            switch (code)
            {
                case TypeCode.Byte:
                    byte bValue;
                    result = byte.TryParse(value, out bValue);
                    targetValue = bValue;
                    break;
                case TypeCode.Int16:
                    Int16 i16Value;
                    result = Int16.TryParse(value, out i16Value);
                    targetValue = i16Value;
                    break;
                case TypeCode.Int32:
                    Int32 i32Value;
                    result = Int32.TryParse(value, out i32Value);
                    targetValue = i32Value;
                    break;
                case TypeCode.Int64:
                    Int64 i64Value;
                    result = Int64.TryParse(value, out i64Value);
                    targetValue = i64Value;
                    break;
                case TypeCode.SByte:
                    sbyte sbValue;
                    result = sbyte.TryParse(value, out sbValue);
                    targetValue = sbValue;
                    break;
                case TypeCode.UInt16:
                    UInt16 ui16Value;
                    result = UInt16.TryParse(value, out ui16Value);
                    targetValue = ui16Value;
                    break;
                case TypeCode.UInt32:
                    UInt32 ui32Value;
                    result = UInt32.TryParse(value, out ui32Value);
                    targetValue = ui32Value;
                    break;
                case TypeCode.UInt64:
                    UInt64 ui64Value;
                    result = UInt64.TryParse(value, out ui64Value);
                    targetValue = ui64Value;
                    break;
                case TypeCode.Decimal:
                    decimal dValue;
                    result = decimal.TryParse(value, out dValue);
                    targetValue = dValue;
                    break;
                case TypeCode.Double:
                    double dbValue;
                    result = double.TryParse(value, out dbValue);
                    targetValue = dbValue;
                    break;
                case TypeCode.Single:
                    float fValuel;
                    result = float.TryParse(value, out fValuel);
                    targetValue = fValuel;
                    break;
                default:
                    //默认为输入的字符串
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获得类型默认值
        /// </summary>
        /// <param name="targetValueType">要获取默认值的目标类型</param>
        /// <returns>类型默认值</returns>
        public static object GetTypeDefaultValue(Type targetValueType)
        {
            object defaultValue;
            if (targetValueType.IsValueType)
            {
                defaultValue = Activator.CreateInstance(targetValueType);
            }
            else
            {
                defaultValue = null;
            }

            return defaultValue;
        }
    }
}
