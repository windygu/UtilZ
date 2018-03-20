using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Lib.BaseEx.NDatabaseExtend
{
    /// <summary>
    /// NDatabase辅助类
    /// </summary>
    public static class NDatabaseHelper
    {
        /// <summary>
        /// 存储对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <param name="value">数据对象</param>
        public static void Store<T>(string filePath, T value) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            databaseExtend.Store<T>(value);
        }

        /// <summary>
        /// 存储对象集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <param name="values">数据集合</param>
        public static void Store<T>(string filePath, IEnumerable<T> values) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            databaseExtend.Store<T>(values);
        }

        /// <summary>
        /// 存储对象,如果存在旧的数据文件,则先删除
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <param name="value">数据对象</param>
        public static void NewStore<T>(string filePath, T value) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            databaseExtend.NewStore<T>(value);
        }

        /// <summary>
        /// 存储对象集合,如果存在旧的数据文件,则先删除
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <param name="values">数据集合</param>
        public static void NewStore<T>(string filePath, IEnumerable<T> values) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            databaseExtend.NewStore<T>(values);
        }

        /// <summary>
        /// 删除数据对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <param name="value">数据对象</param>
        public static void Delete<T>(string filePath, T value) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            databaseExtend.Delete<T>(value);
        }

        /// <summary>
        /// 删除数据对象集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <param name="values">数据集合</param>
        public static void Delete<T>(string filePath, IEnumerable<T> values) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            databaseExtend.Delete<T>(values);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filePath">数据文件路径</param>
        /// <returns>数据集合</returns>
        public static IEnumerable<T> Query<T>(string filePath) where T : class
        {
            NDatabaseExtend databaseExtend = new NDatabaseExtend(filePath);
            return databaseExtend.Query<T>();
        }
    }
}
