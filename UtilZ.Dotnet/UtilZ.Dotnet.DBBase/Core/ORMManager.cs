using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.DBBase.Factory;
using UtilZ.Dotnet.DBModel.DBInfo;
using UtilZ.Dotnet.DBModel.DBObject;
using UtilZ.Dotnet.DBBase.Interface;
using UtilZ.Dotnet.Ex.Base;

namespace UtilZ.Dotnet.DBBase.Core
{
    /// <summary>
    /// 实体映射管理类
    /// </summary>
    public class ORMManager
    {
        #region 数据模型管理
        /// <summary>
        /// 属性Hashtable集合[key:模型名称;value:数据模型信息]
        /// </summary>
        private static readonly Hashtable _htTypePropertyInfos = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 转换接口名称
        /// </summary>
        private static readonly string _iDBModelValueConvertName = typeof(IDBModelValueConvert).Name;

        /// <summary>
        /// 线程锁
        /// </summary>
        private readonly static object _monitor = new object();

        /// <summary>
        /// 数据库表特性信息类型
        /// </summary>
        private static readonly Type _dbTableAttributeType = typeof(DBTableAttribute);

        /// <summary>
        /// 数据模型数据库字段特性信息类型
        /// </summary>
        private static readonly Type _dbColumnAttributeType = typeof(DBColumnAttribute);

        /// <summary>
        /// 注册数据模型
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="dbid">数据库编号ID</param>
        /// <param name="dataModelTableInfo">数据模型表信息</param>
        public static void RegisteDataModel<T>(int dbid, DataModelTableInfo dataModelTableInfo)
        {
            IDBAccess dbAccess = DBFactoryManager.GetDBFactory(dbid).GetDBAccess(dbid);
            RegisteDataModel<T>(dbAccess, dataModelTableInfo);
        }

        /// <summary>
        /// 注册数据模型
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="dbAccess">数据库访问对象</param>
        /// <param name="dataModelTableInfo">数据模型表信息</param>
        public static void RegisteDataModel<T>(IDBAccess dbAccess, DataModelTableInfo dataModelTableInfo)
        {
            if (dbAccess == null)
            {
                throw new ArgumentNullException("数据库访问对象不能为null", "dbAccess");
            }

            if (dataModelTableInfo == null)
            {
                throw new ArgumentNullException("数据模型表信息不能为null", "dataModelTableInfo");
            }

            if (dataModelTableInfo.DBTable == null)
            {
                throw new ArgumentNullException("数据模型表信息中的表字段不能为null", "dataModelTableInfo.DBTable");
            }

            if (string.IsNullOrEmpty(dataModelTableInfo.DBTable.Name))
            {
                throw new ArgumentNullException("数据模型表信息中的表字段中的表名不能为空或null", "dataModelTableInfo.DBTable.Name");
            }

            if (dataModelTableInfo.DataModelFieldInfos == null)
            {
                throw new ArgumentNullException("数据模型所有字段信息集合不能为null", "dataModelTableInfo.DataModelFieldInfos");
            }

            if (dataModelTableInfo.DataModelFieldInfos.Count == 0)
            {
                throw new ArgumentNullException("数据模型所有字段信息集合不能为空", "dataModelTableInfo.DataModelFieldInfos");
            }

            //数据模型中属性的值与数据库表中字段对应的值转换器
            IDBModelValueConvert dbModelValueConvert = null;
            string valueConvertor = dataModelTableInfo.DBTable.ValueConvertor;
            if (dbModelValueConvert == null && !string.IsNullOrWhiteSpace(valueConvertor))
            {
                dbModelValueConvert = ActivatorEx.CreateInstance(valueConvertor) as IDBModelValueConvert;
            }

            DBTableInfo dbTableInfo = dbAccess.GetTableInfo(dataModelTableInfo.DBTable.Name, true);
            //数据库主键列信息及对应的类的属性信息集合[key:列名;value:列信息]
            var dicPriKeyDBColumnProperties = new Dictionary<string, DBColumnPropertyInfo>();
            //数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
            var dicDBColumnProperties = new Dictionary<string, DBColumnPropertyInfo>();
            DBColumnPropertyInfo tmpItem = null;
            PropertyInfo propertyInfo = null;
            Type type = typeof(T);
            Dictionary<string, PropertyInfo> dicProperties = type.GetProperties().ToDictionary((pro) => { return pro.Name; });//模型属性集合

            foreach (var dataModelFieldInfo in dataModelTableInfo.DataModelFieldInfos)
            {
                if (!dicProperties.ContainsKey(dataModelFieldInfo.PropertyName))
                {
                    throw new Exception(string.Format("类型{0}中不包含名称为{1}的属性", type.FullName, dataModelFieldInfo.PropertyName));
                }

                propertyInfo = dicProperties[dataModelFieldInfo.PropertyName];
                tmpItem = new DBColumnPropertyInfo(dataModelFieldInfo, propertyInfo);

                //数据字段列
                dicDBColumnProperties.Add(tmpItem.DBColumn.ColumnName.ToUpper(), tmpItem);

                if (dataModelTableInfo.PriKeyDataModelFieldInfos != null && dataModelTableInfo.PriKeyDataModelFieldInfos.Contains(dataModelFieldInfo))
                {
                    //主键列
                    dicPriKeyDBColumnProperties.Add(tmpItem.DBColumn.ColumnName, tmpItem);
                }
            }

            //数据模型信息
            _htTypePropertyInfos[type] = new DataModelInfo(new DBTableAttribute(dataModelTableInfo.DBTable.Name), dicDBColumnProperties, dicPriKeyDBColumnProperties, dbModelValueConvert);
        }

        /// <summary>
        /// 注册数据模型
        /// </summary>
        /// <typeparam name="T">数据模型</typeparam>
        public static void RegisteDataModel<T>()
        {
            RegisteDataModel(typeof(T));
        }

        /// <summary>
        /// 注册数据模型
        /// </summary>
        /// <param name="type">数据模型</param>
        /// <returns>注册成功后的数据模型信息</returns>
        private static DataModelInfo RegisteDataModel(Type type)
        {
            DataModelInfo dataTableInfo;
            object[] attrs = null;
            attrs = type.GetCustomAttributes(_dbTableAttributeType, false);
            if (attrs.Length == 0)
            {
                throw new Exception(string.Format("类型:{0}上未标记{1}特性;可通过修改类型{2}定义或调用RegisteDBModel方法注册数据模型", type.FullName, _dbTableAttributeType.FullName, type.FullName));
            }

            DBTableAttribute dbTable = (DBTableAttribute)attrs[0];
            if (string.IsNullOrEmpty(dbTable.Name))
            {
                throw new Exception(string.Format("类型:{0}表名不能为空", type.FullName));
            }

            IDBModelValueConvert dbModelValueConvert = null;
            //数据模型中属性的值与数据库表中字段对应的值是否是否需要转换
            if (!string.IsNullOrWhiteSpace(dbTable.ValueConvertor))
            {
                dbModelValueConvert = ActivatorEx.CreateInstance(dbTable.ValueConvertor) as IDBModelValueConvert;
            }

            //数据库主键列信息及对应的类的属性信息集合[key:列名;value:列信息]
            var dicPriKeyDBColumnProperties = new Dictionary<string, DBColumnPropertyInfo>();
            //数据库列信息及对应的类的属性信息集合[key:列名;value:列信息]
            var dicDBColumnProperties = new Dictionary<string, DBColumnPropertyInfo>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            DBColumnAttribute dbColumnAttribute = null;
            DBColumnPropertyInfo tmpItem = null;

            foreach (var propertyInfo in propertyInfos)
            {
                attrs = propertyInfo.GetCustomAttributes(_dbColumnAttributeType, false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                if (!propertyInfo.CanWrite)
                {
                    throw new Exception(string.Format("类型:{0}中的字段:{1}不支持读或不支持写", type.FullName, propertyInfo.Name));
                }

                dbColumnAttribute = (DBColumnAttribute)attrs[0];
                if (string.IsNullOrEmpty(dbColumnAttribute.ColumnName))
                {
                    throw new Exception(string.Format("类型:{0}中的字段:{1}列名不能为空", type.FullName, propertyInfo.Name));
                }

                tmpItem = new DBColumnPropertyInfo(dbColumnAttribute, propertyInfo);
                if (dicDBColumnProperties.ContainsKey(dbColumnAttribute.ColumnName))
                {
                    throw new Exception(string.Format("类型:{0}中存在相同的列名:{1}", type.FullName, dbColumnAttribute.ColumnName));
                }

                dicDBColumnProperties.Add(dbColumnAttribute.ColumnName.ToUpper(), tmpItem);

                //如果是主键则记录
                if (dbColumnAttribute.IsPriKey)
                {
                    dicPriKeyDBColumnProperties.Add(dbColumnAttribute.ColumnName, tmpItem);
                }
            }

            //数据模型信息
            dataTableInfo = new DataModelInfo(dbTable, dicDBColumnProperties, dicPriKeyDBColumnProperties, dbModelValueConvert);
            _htTypePropertyInfos[type.FullName] = dataTableInfo;
            return dataTableInfo;
        }

        /// <summary>
        /// 获取数据模型信息
        /// </summary>
        /// <param name="type">数据模型类型</param>
        /// <returns>数据模型信息</returns>
        public static DataModelInfo GetDataModelInfo(Type type)
        {
            DataModelInfo dataTableInfo = _htTypePropertyInfos[type] as DataModelInfo;
            if (dataTableInfo != null)
            {
                return dataTableInfo;
            }

            lock (_monitor)
            {
                dataTableInfo = _htTypePropertyInfos[type] as DataModelInfo;
                if (dataTableInfo != null)
                {
                    return dataTableInfo;
                }

                return RegisteDataModel(type);
            }
        }

        /// <summary>
        /// 移除移除已注册数据模型
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        public static void RemoveDataModel<T>()
        {
            RemoveDataModel(typeof(T));
        }

        /// <summary>
        /// 移除移除已注册数据模型
        /// </summary>
        /// <param name="type">数据模型类型</param>
        public static void RemoveDataModel(Type type)
        {
            _htTypePropertyInfos.Remove(type);
        }
        #endregion

        #region 数据转换为数据模型列表
        /// <summary>
        /// 转换DataTable为List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <param name="dbAccess">数据库访问对象</param>
        /// <returns>数据集合</returns>
        public static List<T> ConvertData<T>(DataTable dt, IDBAccess dbAccess) where T : class, new()
        {
            return ConvertData<T>(dt, typeof(T), dbAccess);
        }

        /// <summary>
        /// 转换DataTable为List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <param name="tType">模型类型</param>
        /// <param name="dbAccess">数据库访问对象</param>
        /// <returns>数据集合</returns>
        public static List<T> ConvertData<T>(DataTable dt, Type tType, IDBAccess dbAccess) where T : class, new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<T>();
            }

            DataModelInfo dataTableInfo = GetDataModelInfo(tType);
            //列名及模型属性字典集合[key:列名,value:属性信息]
            Dictionary<string, DBColumnPropertyInfo> dicProperties = dataTableInfo.DicDBColumnProperties;
            //如果数据中存在但不包含在模型中的列,则该列数据无视,移除该列
            List<string> removeColNames = new List<string>();
            Dictionary<string, string> colNamesMap = new Dictionary<string, string>();
            string colName;
            foreach (DataColumn col in dt.Columns)
            {
                colName = col.ColumnName.ToUpper();
                colNamesMap.Add(col.ColumnName, colName);
                if (!dicProperties.ContainsKey(colName))
                {
                    removeColNames.Add(col.ColumnName);
                }
            }

            //从DT中移除数据中存在但不包含在模型中的列
            foreach (string removeColName in removeColNames)
            {
                dt.Columns.Remove(removeColName);
            }

            //转换查询的DT为List<T>
            List<T> items = new List<T>();
            object value = null;
            DBColumnPropertyInfo dbColumnPropertyInfo = null;
            //遍历数据行
            foreach (DataRow row in dt.Rows)
            {
                T item = new T();
                foreach (DataColumn col in dt.Columns)
                {
                    colName = colNamesMap[col.ColumnName];
                    value = row[colName];
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }

                    dbColumnPropertyInfo = dicProperties[colName];
                    if (dataTableInfo.ModelValueConvert != null)
                    {
                        //如果该值需要转换为数据库表中字段对应的值,则转换值
                        value = dataTableInfo.ModelValueConvert.DBToModel(tType, dbAccess.DatabaseTypeName, dbColumnPropertyInfo.PropertyInfo.Name, value);
                    }
                    else//否则如果类型不匹配则转换为类型匹配的数据
                    {
                        if (col.DataType != dbColumnPropertyInfo.PropertyInfo.PropertyType)
                        {
                            value = ConvertEx.ToObject(dbColumnPropertyInfo.PropertyInfo.PropertyType, value);
                        }
                    }

                    //设置值
                    dbColumnPropertyInfo.PropertyInfo.SetValue(item, value, dbColumnPropertyInfo.DBColumn.Index);
                }

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 缓存20分钟
        /// </summary>
        private const int PropertiesCacheTime = 1000 * 60 * 20;

        /// <summary>
        /// 获取属性字典集合[key:属性名;value:PropertyInfo]
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>属性字典集合</returns>
        public static Dictionary<string, PropertyInfo> GetProperties<T>()
        {
            Type type = typeof(T);
            var propertyInfos = MemoryCacheEx.Get(type.FullName) as Dictionary<string, PropertyInfo>;
            if (propertyInfos == null)
            {
                propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).ToDictionary((item) => { return item.Name.ToUpper(); });
                MemoryCacheEx.Set(type.FullName, propertyInfos, PropertiesCacheTime);
            }

            return propertyInfos;
        }

        /// <summary>
        /// 转换DataTable为List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns>数据集合</returns>
        public static List<T> ConvertData<T>(DataTable dt) where T : class, new()
        {
            Dictionary<string, string> colNameMap = new Dictionary<string, string>();
            foreach (DataColumn col in dt.Columns)
            {
                colNameMap.Add(col.ColumnName, col.ColumnName.ToUpper());
            }

            List<T> items = new List<T>();
            var propertyInfos = GetProperties<T>();
            PropertyInfo propertyInfo;
            object value;

            //遍历数据行
            string colName;
            foreach (DataRow row in dt.Rows)
            {
                T item = new T();
                foreach (DataColumn col in dt.Columns)
                {
                    colName = colNameMap[col.ColumnName];
                    if (!propertyInfos.ContainsKey(colName))
                    {
                        continue;
                    }

                    propertyInfo = propertyInfos[colName];
                    value = row[col];
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    else if (col.DataType != propertyInfo.PropertyType)
                    {
                        value = ConvertEx.ToObject(propertyInfo.PropertyType, value);
                    }

                    propertyInfo.SetValue(item, value, null);
                }

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 转换IDataReader为List泛型集合
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="reader">数据表</param>
        /// <returns>数据集合</returns>
        public static List<T> ConvertData<T>(IDataReader reader) where T : class, new()
        {
            Dictionary<string, string> colNameMap = null;
            List<T> items = new List<T>();
            var propertyInfos = GetProperties<T>();
            PropertyInfo propertyInfo;
            string colName;
            object value;
            //遍历数据行
            while (reader.Read())
            {
                if (colNameMap == null)
                {
                    colNameMap = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        colName = reader.GetName(i);
                        colNameMap.Add(colName, colName.ToUpper());
                    }
                }

                T item = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    colName = colNameMap[reader.GetName(i)];
                    if (!propertyInfos.ContainsKey(colName))
                    {
                        continue;
                    }

                    propertyInfo = propertyInfos[colName];
                    value = reader.GetValue(i);
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    else if (reader.GetFieldType(i) != propertyInfo.PropertyType)
                    {
                        value = Convert.ChangeType(value, propertyInfo.PropertyType);
                    }

                    propertyInfo.SetValue(item, value, null);
                }

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 转换IDataReader第一项数据为T类型对象
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <param name="propertyInfos">属性字典集合[key:属性名;value:PropertyInfo]</param>
        /// <returns>T类型对象</returns>
        public static T ConvertData<T>(IDataReader reader, Dictionary<string, PropertyInfo> propertyInfos) where T : class, new()
        {
            if (propertyInfos == null)
            {
                propertyInfos = GetProperties<T>();
            }

            PropertyInfo propertyInfo;
            string colName;
            object value;
            T item = null;
            //遍历数据行
            if (reader.Read())
            {
                item = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    colName = reader.GetName(i).ToUpper();
                    if (!propertyInfos.ContainsKey(colName))
                    {
                        continue;
                    }

                    propertyInfo = propertyInfos[colName];
                    value = reader.GetValue(i);
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    else if (reader.GetFieldType(i) != propertyInfo.PropertyType)
                    {
                        value = Convert.ChangeType(value, propertyInfo.PropertyType);
                    }

                    propertyInfo.SetValue(item, value, null);
                }
            }

            return item;
        }

        /// <summary>
        /// 转换DataRow为T类型对象
        /// </summary>
        /// <typeparam name="T">数据模型类型</typeparam>
        /// <param name="row">数据行</param>
        /// <param name="propertyInfos">属性字典集合[key:属性名;value:PropertyInfo]</param>
        /// <returns>T类型对象</returns>
        public static T ConvertData<T>(DataRow row, Dictionary<string, PropertyInfo> propertyInfos) where T : class, new()
        {
            if (propertyInfos == null)
            {
                propertyInfos = GetProperties<T>();
            }

            T item = null;
            PropertyInfo propertyInfo;
            object value;
            string colName;
            //遍历数据行
            if (row != null)
            {
                item = new T();
                foreach (DataColumn col in row.Table.Columns)
                {
                    colName = col.ColumnName.ToUpper();
                    if (!propertyInfos.ContainsKey(colName))
                    {
                        continue;
                    }

                    propertyInfo = propertyInfos[colName];
                    value = row[col];
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    else if (col.DataType != propertyInfo.PropertyType)
                    {
                        value = Convert.ChangeType(value, propertyInfo.PropertyType);
                    }

                    propertyInfo.SetValue(item, value, null);
                }
            }

            return item;
        }
        #endregion
    }
}
