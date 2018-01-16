using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace UtilZ.Lib.Base.Extend
{
    /// <summary>
    /// Object扩展方法类
    /// </summary>
    public static class NExtendObject
    {
        /// <summary>
        /// 对象的深拷贝
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="obj">原始对象</param>
        /// <returns>新实例</returns>
        public static T DeepCopy<T>(this object obj)
        {
            Func<T> function = () =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, obj);
                    memoryStream.Position = 0;
                    T newInstance = (T)formatter.Deserialize(memoryStream);
                    return newInstance;
                }
            };

            return AssemblyHelper.ExcuteFuction<T>(function);
        }

        /// <summary>
        /// 可序列化对象序列化为byte数组
        /// </summary>
        /// <param name="obj">可序列化对象</param>
        /// <returns>byte数组</returns>
        public static byte[] ToBytes(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                return memoryStream.GetBuffer();
            }
        }

        /// <summary>
        /// 二进制转换为可序列化的对象
        /// </summary>
        /// <typeparam name="T">可序列化的类型 </typeparam>
        /// <param name="buffer">byte数组</param>
        /// <returns>可序列化的类型实例</returns>
        public static T ToObject<T>(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return default(T);
            }

            using (var memoryStream = new MemoryStream(buffer))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                memoryStream.Position = 0;
                return (T)formatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// 获取对象类型定义程序集所在目录
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>程序集所在目录</returns>
        public static string GetAssemblyDirectory(this object obj)
        {
            return NExtendObject.GetAssemblyDirectory(obj.GetType());
        }

        /// <summary>
        /// 获取T类型定义程序集所在目录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>程序集所在目录</returns>
        public static string GetAssemblyDirectory<T>()
        {
            return NExtendObject.GetAssemblyDirectory(typeof(T));
        }

        /// <summary>
        /// 获取类型定义程序集所在目录
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>程序集所在目录</returns>
        public static string GetAssemblyDirectory(Type type)
        {
            return Path.GetDirectoryName(type.Assembly.Location);
        }

        /// <summary>
        /// 获取变量名称
        /// </summary>
        /// <param name="varPara">变量参数</param>
        /// <param name="exp">变量表达式</param>
        /// <returns>变量名</returns>
        public static string GetVarName(this object varPara, System.Linq.Expressions.Expression<Func<object, object>> exp)
        {
            return GetVarName(exp);
        }

        /// <summary>
        /// 获取变量名称
        /// </summary>
        /// <param name="exp">变量表达式</param>
        /// <returns>变量名</returns>
        public static string GetVarName(System.Linq.Expressions.Expression<Func<object, object>> exp)
        {
            string varName = null;
            if (exp.Body is System.Linq.Expressions.UnaryExpression)
            {
                varName = ((System.Linq.Expressions.MemberExpression)(((System.Linq.Expressions.UnaryExpression)exp.Body).Operand)).Member.Name;
            }
            else if (exp.Body is System.Linq.Expressions.MemberExpression)
            {
                varName = ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;
            }
            else if (exp.Body is System.Linq.Expressions.ParameterExpression)
            {
                varName = ((System.Linq.Expressions.ParameterExpression)exp.Body).Type.Name;
            }

            return varName;
        }

        /// <summary>
        /// 无序遍历集合
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="items">集合</param>
        /// <param name="func">集合项要做的处理委托[返回值:true:继续遍历其它项;false:循环结束]</param>
        public static void DisorderLoop<T>(IEnumerable<T> items, Func<T, bool> func)
        {
            if (items == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(xx => items));
            }

            if (func == null)
            {
                throw new ArgumentNullException(NExtendObject.GetVarName(xx => func));
            }

            int count = items.Count();
            //空集合直接返回
            if (count == 0)
            {
                return;
            }

            int length = count / 2;
            if (count % 2 != 0)
            {
                length += 1;
            }

            /*************************************************
             *    fp              pl      pr              rp
             * |-->|              |<--|-->|              |<--|
             * |______________________|______________________|
             **************************************************/
            int fp = 0;
            int pl = length - 1;
            int pr = 0;
            int rp = count - 1;
            T tmpItem;

            //fp,pl,pr,rp为四个方向移动的索引指针
            for (fp = 0, pr = length; fp < length; fp++, pr++)
            {
                if (fp <= pl)
                {
                    tmpItem = items.ElementAt(fp);
                    if (!func(tmpItem))
                    {
                        break;
                    }
                }

                if (pl > fp)
                {
                    tmpItem = items.ElementAt(pl);
                    if (!func(tmpItem))
                    {
                        break;
                    }

                    pl--;
                }

                if (pr <= rp)
                {
                    tmpItem = items.ElementAt(pr);
                    if (!func(tmpItem))
                    {
                        break;
                    }
                }

                if (rp > pr)
                {
                    tmpItem = items.ElementAt(rp);
                    if (!func(tmpItem))
                    {
                        break;
                    }

                    rp--;
                }
            }
        }
    }
}
