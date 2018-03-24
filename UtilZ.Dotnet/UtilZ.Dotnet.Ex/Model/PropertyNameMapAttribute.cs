using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UtilZ.Dotnet.Ex.Base;
using UtilZ.Dotnet.Ex.MemoryCache;

namespace UtilZ.Dotnet.Ex.Model
{
    /// <summary>
    /// 属性名称映射属性
    /// </summary>
    [Serializable]
    public class PropertyNameMapAttribute : Attribute
    {
        /// <summary>
        /// 映射源对象类型,为null作用任意对象
        /// </summary>
        public Type SourceType { get; set; }

        /// <summary>
        /// 与源对象映射属性名称
        /// </summary>
        public string SourcePropertyName { get; set; }

        /// <summary>
        /// 属性值转换类型
        /// </summary>
        public PropertyValueConvertType ConvertType { get; set; }

        /// <summary>
        /// 与源对象映射属性获取值索引
        /// </summary>
        public object[] SourceIndex { get; set; }

        /// <summary>
        /// 目标属性设置值索引
        /// </summary>
        public object[] TargetIndex { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourcePropertyName">与源对象映射属性名称</param>
        public PropertyNameMapAttribute(string sourcePropertyName) : this(sourcePropertyName, PropertyValueConvertType.ConvertChangeType)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourcePropertyName">与源对象映射属性名称</param>
        /// <param name="convertType">属性值转换类型</param>
        public PropertyNameMapAttribute(string sourcePropertyName, PropertyValueConvertType convertType)
        {
            this.SourcePropertyName = sourcePropertyName;
            this.ConvertType = convertType;
        }
    }

    /// <summary>
    /// 属性值转换类型
    /// </summary>
    public enum PropertyValueConvertType
    {
        /// <summary>
        /// 不转换
        /// </summary>
        None = 1,

        /// <summary>
        /// 如果源对象中映射的属性值与目标属性类型不一致时,调用Convert.ChangeType强制转换,转换失败抛出异常
        /// </summary>
        ConvertChangeType = 2,

        /// <summary>
        /// 如果源对象中映射的属性值与目标属性类型不一致时,自定义转换
        /// </summary>
        Custom = 3
    }

    /// <summary>
    /// 属性值转换接口
    /// </summary>
    public interface IPropertyValueConverter
    {
        /// <summary>
        /// 转换函数
        /// </summary>
        /// <param name="sourceType">源对象类型</param>
        /// <param name="sourcePropertyName">源属性名称</param>
        /// <param name="targetPropertyName">目标属性名称</param>
        /// <param name="sourceValue">源对象中属性关联的值</param>
        /// <returns>转换后的目标值</returns>
        object Convert(Type sourceType, string sourcePropertyName, string targetPropertyName, object sourceValue);
    }

    /// <summary>
    /// 属性映射对象辅助类
    /// </summary>
    public class PropertyMapObjectHelper
    {
        /// <summary>
        /// 缓存项有效时间,单位/毫秒(5分钟)
        /// </summary>
        private static readonly int _expiration = 5 * 60 * 1000;

        /// <summary>
        /// 更新关联对象映射值
        /// </summary>
        /// <typeparam name="T">源对象类型</typeparam>
        /// <typeparam name="W">目标对象类型</typeparam>
        /// <param name="sourceObj">源对象</param>
        /// <param name="targetObj">目标对象</param>
        /// <param name="updateProperties">目标对象要更新的属性名称列表</param>
        public static void UpdateValue<T, W>(T sourceObj, W targetObj, IEnumerable<string> updateProperties = null)
            where T : class
            where W : class
        {
            if (sourceObj == null || targetObj == null)
            {
                return;
            }

            //获取映射信息
            Type sourceType = sourceObj.GetType();
            string key = sourceType.FullName;
            PropertyMapObjectInfo mapObjectInfo = MemoryCacheEx.Get(key) as PropertyMapObjectInfo;
            if (mapObjectInfo == null)
            {
                Hashtable htTargetObjProperty = new Hashtable();
                Hashtable htSourceObjProperty = new Hashtable();
                Type pnmType = typeof(PropertyNameMapAttribute);
                object[] pnmAttri;
                PropertyNameMapAttribute pnm;
                var proInfos = targetObj.GetType().GetProperties().Where(p => p.GetCustomAttributes(pnmType, false) != null);
                foreach (var proInfo in targetObj.GetType().GetProperties())
                {
                    pnmAttri = proInfo.GetCustomAttributes(pnmType, false);
                    if (pnmAttri == null || pnmAttri.Length == 0)
                    {
                        continue;
                    }

                    pnm = null;
                    foreach (PropertyNameMapAttribute pnmAttriItem in pnmAttri)
                    {
                        if (pnmAttriItem.SourceType != null && pnmAttriItem.SourceType == sourceType)
                        {
                            pnm = pnmAttriItem;
                            break;
                        }
                    }

                    if (pnm == null)
                    {
                        continue;
                    }

                    htTargetObjProperty.Add(proInfo.Name, new PropertyMapInfo(proInfo, pnm));
                }

                mapObjectInfo = new PropertyMapObjectInfo(htTargetObjProperty, htSourceObjProperty);
                MemoryCacheEx.Set(key, mapObjectInfo, _expiration);
            }

            //获取目标对象要设置的属性列表
            Hashtable htTargetAllProperty = mapObjectInfo.HTTargetObjectProperty;
            List<PropertyMapInfo> propertyMapItems = new List<PropertyMapInfo>();
            if (updateProperties != null && updateProperties.Count() > 0)
            {
                foreach (string propertyName in htTargetAllProperty.Keys)
                {
                    if (updateProperties.Contains(propertyName))
                    {
                        propertyMapItems.Add((PropertyMapInfo)htTargetAllProperty[propertyName]);
                    }
                }
            }
            else
            {
                foreach (string propertyName in htTargetAllProperty.Keys)
                {
                    propertyMapItems.Add((PropertyMapInfo)htTargetAllProperty[propertyName]);
                }
            }

            //更新值
            Hashtable htSourceObjectProperty = mapObjectInfo.HTSourceObjectProperty;
            object targetValue;
            string sourcePropertyName;
            PropertyInfo propertyInfo;
            foreach (var propertyMapItem in propertyMapItems)
            {
                sourcePropertyName = propertyMapItem.Map.SourcePropertyName;
                if (!htSourceObjectProperty.ContainsKey(sourcePropertyName))
                {
                    throw new Exception(string.Format("源类型:{0}中找不到属性:{1}", sourceType.FullName, sourcePropertyName));
                }

                targetValue = ((PropertyInfo)htSourceObjectProperty[sourcePropertyName]).GetValue(sourceObj, propertyMapItem.Map.SourceIndex);
                propertyInfo = propertyMapItem.PropertyInfo;
                if (targetValue == null)
                {
                    if (!propertyInfo.PropertyType.IsClass)
                    {
                        switch (propertyMapItem.Map.ConvertType)
                        {
                            case PropertyValueConvertType.None:
                            case PropertyValueConvertType.ConvertChangeType:
                                targetValue = Convert.ChangeType(targetValue, propertyInfo.PropertyType);
                                break;
                            case PropertyValueConvertType.Custom:
                                if (sourceType.GetInterface(typeof(IPropertyValueConverter).FullName) != null)
                                {
                                    targetValue = ((IPropertyValueConverter)targetObj).Convert(sourceType, propertyMapItem.Map.SourcePropertyName, propertyInfo.Name, targetValue);
                                }
                                else
                                {
                                    targetValue = Convert.ChangeType(targetValue, propertyInfo.PropertyType);
                                }
                                break;
                            default:
                                throw new NotImplementedException("未实现的转换类型:" + propertyMapItem.Map.ConvertType.ToString());
                        }
                    }
                }
                else
                {
                    if (propertyInfo.PropertyType != targetValue.GetType())
                    {
                        switch (propertyMapItem.Map.ConvertType)
                        {
                            case PropertyValueConvertType.None:
                                break;
                            case PropertyValueConvertType.ConvertChangeType:
                                targetValue = Convert.ChangeType(targetValue, propertyInfo.PropertyType);
                                break;
                            case PropertyValueConvertType.Custom:
                                if (sourceType.GetInterface(typeof(IPropertyValueConverter).FullName) != null)
                                {
                                    targetValue = ((IPropertyValueConverter)targetObj).Convert(sourceType, propertyMapItem.Map.SourcePropertyName, propertyInfo.Name, targetValue);
                                }
                                else
                                {
                                    targetValue = Convert.ChangeType(targetValue, propertyInfo.PropertyType);
                                }
                                break;
                            default:
                                throw new NotImplementedException("未实现的转换类型:" + propertyMapItem.Map.ConvertType.ToString());
                        }
                    }
                }
            }
        }
    }

    internal class PropertyMapObjectInfo
    {
        public Hashtable HTTargetObjectProperty { get; private set; }

        public Hashtable HTSourceObjectProperty { get; private set; }

        public PropertyMapObjectInfo(Hashtable htTargetObjectProperty, Hashtable htSourceObjectProperty)
        {
            this.HTTargetObjectProperty = htTargetObjectProperty;
            this.HTTargetObjectProperty = htSourceObjectProperty;
        }
    }

    internal class PropertyMapInfo
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyNameMapAttribute Map { get; private set; }

        public PropertyMapInfo(PropertyInfo propertyInfo, PropertyNameMapAttribute map)
        {
            this.PropertyInfo = propertyInfo;
            this.Map = map;
        }
    }
}
