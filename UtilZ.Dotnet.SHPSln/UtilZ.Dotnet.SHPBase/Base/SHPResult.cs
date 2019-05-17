using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.SHPBase.Base
{
    /// <summary>
    /// 运控命令执行结果
    /// </summary>
    [Serializable]
    public class SHPResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public SHPCommandExcuteResult Result { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHPResult()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        public SHPResult(SHPCommandExcuteResult result, byte[] data)
        {
            this.Result = result;
            this.Data = data;
        }

        /// <summary>
        /// 获取异常二进制数据
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>二进制数据</returns>
        public static byte[] GetBytes(Exception ex)
        {
            return Encoding.UTF8.GetBytes(ex.ToString());
        }

        /// <summary>
        /// 获取异常
        /// </summary>
        /// <param name="byte[]">二进制数据</param>
        /// <returns>异常</returns>
        public static Exception GetException(byte[] data)
        {
            return new Exception(Encoding.UTF8.GetString(data));
        }

        /// <summary>
        /// 获取字符串二进制数据
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>二进制数据</returns>
        public static byte[] GetBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 二进制数据转字符串
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <returns>字符串</returns>
        public static string GetString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public byte[] ToBytes()
        {
            return SerializeEx.BinarySerialize(this);
        }

        public override string ToString()
        {
            string info = null;
            switch (this.Result)
            {
                case SHPCommandExcuteResult.Sucess:
                    info = "成功";
                    break;
                case SHPCommandExcuteResult.Fail:
                    info = "失败";
                    break;
                case SHPCommandExcuteResult.Timeout:
                    info = "超时";
                    break;
                case SHPCommandExcuteResult.Exception:
                    info = "异常";
                    break;
                case SHPCommandExcuteResult.NotExistPlugin:
                    info = "没有对应处理的插件";
                    break;
                case SHPCommandExcuteResult.NotExistCommand:
                    info = "没有对应的命令";
                    break;
                default:
                    info = "未知";
                    break;
            }

            //var data = this.Data;
            //if (data != null && data.Length > 0)
            //{
            //    var error = Encoding.UTF8.GetString(data);
            //    info = $"{info},{error}";
            //}

            return info;
        }
    }
}
