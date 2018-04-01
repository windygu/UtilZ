using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.ILEx.NDatabaseEx
{
    /// <summary>
    /// NDatabase扩展类
    /// </summary>
    public class NDatabaseExtend
    {
        /// <summary>
        /// 数据库文件路径
        /// </summary>
        private readonly string _dbFilePath = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbFilePath">数据库文件路径</param>
        public NDatabaseExtend(string dbFilePath)
        {
            if (string.IsNullOrEmpty(dbFilePath))
            {
                throw new ArgumentNullException("dbFilePath");
            }

            this._dbFilePath = dbFilePath;
        }

        /// <summary>
        /// 断言对象不为null
        /// </summary>
        /// <param name="value">要断言的对象</param>
        private void AssertValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
        }

        /// <summary>
        /// 存储对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="value">数据对象</param>
        public void Store<T>(T value) where T : class
        {
            this.AssertValue(value);

            using (var odb = NDatabase.OdbFactory.Open(this._dbFilePath))
            {
                odb.Store<T>(value);
            }
        }

        /// <summary>
        /// 存储对象集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        public void Store<T>(IEnumerable<T> values) where T : class
        {
            this.AssertValue(values);

            using (var odb = NDatabase.OdbFactory.Open(this._dbFilePath))
            {
                foreach (var value in values)
                {
                    this.AssertValue(value);
                    odb.Store<T>(value);
                }
            }
        }

        /// <summary>
        /// 存储对象,如果存在旧的数据文件,则先删除
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="value">数据对象</param>
        public void NewStore<T>(T value) where T : class
        {
            this.AssertValue(value);

            //删除已存在的数据文件,重新存入对象
            if (File.Exists(this._dbFilePath))
            {
                File.Delete(this._dbFilePath);
            }

            this.Store<T>(value);
        }

        /// <summary>
        /// 存储对象集合,如果存在旧的数据文件,则先删除
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        public void NewStore<T>(IEnumerable<T> values) where T : class
        {
            this.AssertValue(values);

            //删除已存在的数据文件,重新存入对象
            if (File.Exists(this._dbFilePath))
            {
                File.Delete(this._dbFilePath);
            }

            if (values.Count() == 0)
            {
                return;
            }

            using (var odb = NDatabase.OdbFactory.Open(this._dbFilePath))
            {
                foreach (var value in values)
                {
                    odb.Store<T>(value);
                }
            }
        }

        /// <summary>
        /// 删除数据对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="value">数据对象</param>
        public void Delete<T>(T value) where T : class
        {
            this.AssertValue(value);

            using (var odb = NDatabase.OdbFactory.Open(this._dbFilePath))
            {
                odb.Delete(value);
            }
        }

        /// <summary>
        /// 删除数据对象集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="values">数据集合</param>
        public void Delete<T>(IEnumerable<T> values) where T : class
        {
            this.AssertValue(values);

            if (values.Count() == 0)
            {
                return;
            }

            using (var odb = NDatabase.OdbFactory.Open(this._dbFilePath))
            {
                foreach (var value in values)
                {
                    odb.Delete(value);
                }
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>数据集合</returns>
        public IEnumerable<T> Query<T>() where T : class
        {
            using (var odb = NDatabase.OdbFactory.Open(this._dbFilePath))
            {
                return odb.QueryAndExecute<T>();
            }
        }
    }
}
