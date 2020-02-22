using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UtilZ.Dotnet.Ex.Base.Config
{
    /// <summary>
    /// 配置辅助类
    /// </summary>
    public static class ConfigHelper
    {
        private const string _VALUE = "Value";
        private const string _CHILD_ITEM_NAME = "Item";



        #region 写配置
        /// <summary>
        /// 写配置对象到xml文件
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="configFilePath">配置文件存放路径</param>
        public static void WriteConfigToXmlFile(object config, string configFilePath)
        {
            if (configFilePath == null)
            {
                throw new ArgumentNullException(nameof(configFilePath));
            }

            XDocument xdoc = WriteConfigToXDocument(config);
            DirectoryInfoEx.CheckFilePathDirectory(configFilePath);
            xdoc.Save(configFilePath);
        }

        /// <summary>
        /// 写配置到XDocument
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <returns>配置XDocument</returns>
        public static XDocument WriteConfigToXDocument(object config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            ConfigAttributeTypes configAttributeTypes = new ConfigAttributeTypes();
            XDocument xdoc = new XDocument();
            Type configType = config.GetType();

            ConfigRootAttribute configRootAttribute = GetRootConfigRootAttribute(configType, configAttributeTypes);
            XElement rootEle = new XElement(configRootAttribute.GetName(configType));
            AddDes(rootEle, configRootAttribute);


            PropertyInfo[] proInfoArr = GetTypePropertyInfos(configType);
            WriteConfigToXml(rootEle, proInfoArr, config, configAttributeTypes);
            xdoc.Add(rootEle);
            return xdoc;
        }

        private static void WriteConfigToXml(XElement ownerEle, PropertyInfo[] proInfoArr, object ownerObj, ConfigAttributeTypes configAttributeTypes)
        {
            ConfigAttribute attri;
            object proInfoValue;

            foreach (var proInfo in proInfoArr)
            {
                attri = proInfo.PropertyType.GetCustomAttribute(configAttributeTypes.IgnoreAttributeType, false) as ConfigAttribute;
                if (attri != null)
                {
                    //忽略该属性
                    continue;
                }

                proInfoValue = proInfo.GetValue(ownerObj, null);

                attri = proInfo.GetCustomAttribute(configAttributeTypes.CustomerAttribute, false) as ConfigAttribute;
                if (attri != null)
                {
                    //自定义
                    var configItemCustomerAttribute = (ConfigItemCustomerAttribute)attri;
                    configItemCustomerAttribute.CustomerConfig.Write(proInfo, proInfoValue, ownerEle, configItemCustomerAttribute);
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.CollectionAttributeType, false) as ConfigAttribute;
                if (attri != null)
                {
                    //集合
                    WriteCollection(ownerEle, (ConfigItemCollectionAttribute)attri, proInfoValue, proInfo, configAttributeTypes);
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.ComplexItemAttribute, false) as ConfigAttribute;
                if (attri != null)
                {
                    //复合对象
                    WriteComplex(ownerEle, configAttributeTypes, attri, proInfoValue, proInfo);
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.ItemAttributeType, false) as ConfigAttribute;
                if (attri != null)
                {
                    //基元项
                    WriteItem(ownerEle, (ConfigItemAttribute)attri, proInfoValue, proInfo);
                    continue;
                }


                //未标记项                
                if (proInfo.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null &&
                    Type.GetTypeCode(proInfo.PropertyType) != TypeCode.String)
                {
                    //集合
                    Type colletionItemType = proInfo.PropertyType.GenericTypeArguments.First();
                    ConfigItemCollectionAttribute configItemCollectionAttribute;
                    if (colletionItemType.IsClass &&
                        Type.GetTypeCode(colletionItemType) != TypeCode.String)
                    {
                        //非string类型的类
                        configItemCollectionAttribute = new ConfigItemCollectionAttribute(proInfo.Name, null, _CHILD_ITEM_NAME);
                    }
                    else
                    {
                        configItemCollectionAttribute = new ConfigItemPrimitiveCollectionAttribute(proInfo.Name, null, _CHILD_ITEM_NAME, null, false);
                    }
                    WriteCollection(ownerEle, configItemCollectionAttribute, proInfoValue, proInfo, configAttributeTypes);
                }
                else if (proInfo.PropertyType.IsClass && Type.GetTypeCode(proInfo.PropertyType) != TypeCode.String)
                {
                    //复合对象
                    WriteComplex(ownerEle, configAttributeTypes, new ConfigComplexItemAttribute(proInfo.Name, null), proInfoValue, proInfo);
                }
                else
                {
                    //基元项
                    WriteItem(ownerEle, new ConfigItemAttribute(proInfo.Name, null), proInfoValue, proInfo);
                }
            }
        }

        private static void WriteItem(XElement ownerEle, ConfigItemAttribute attri, object proInfoValue, PropertyInfo proInfo)
        {
            if (!attri.AllowNullValueElement && proInfoValue == null)
            {
                return;
            }

            string value;
            if (attri.Converter != null)
            {
                value = attri.Converter.ConvertTo(proInfo, proInfoValue);
            }
            else
            {
                if (proInfoValue is string)
                {
                    value = (string)proInfoValue;
                }
                else
                {
                    value = proInfoValue.ToString();
                }
            }

            XElement itemEle = XmlEx.CreateXElement(attri.GetName(proInfo), _VALUE, value);
            AddDes(itemEle, attri);
            ownerEle.Add(itemEle);
        }

        private static void WriteComplex(XElement ownerEle, ConfigAttributeTypes configAttributeTypes, ConfigAttribute attri, object proInfoValue, PropertyInfo proInfo)
        {
            if (!attri.AllowNullValueElement && proInfoValue == null)
            {
                return;
            }

            XElement complexEle = new XElement(attri.GetName(proInfo));
            AddDes(complexEle, attri);

            if (proInfoValue != null)
            {
                PropertyInfo[] complexProInfoArr = GetTypePropertyInfos(proInfo.PropertyType);
                WriteConfigToXml(complexEle, complexProInfoArr, proInfoValue, configAttributeTypes);
            }
            ownerEle.Add(complexEle);
        }

        private static void WriteCollection(XElement ownerEle, ConfigItemCollectionAttribute attri, object proInfoValue, PropertyInfo proInfo, ConfigAttributeTypes configAttributeTypes)
        {
            if (!attri.AllowNullValueElement && proInfoValue == null)
            {
                return;
            }

            XElement collectionEle = new XElement(attri.GetName(proInfo));
            AddDes(collectionEle, attri);

            IEnumerable enumerable = (IEnumerable)proInfoValue;
            if (enumerable != null)
            {
                if (attri is ConfigItemPrimitiveCollectionAttribute)
                {
                    var configItemPrimitiveCollectionAttribute = (ConfigItemPrimitiveCollectionAttribute)attri;
                    string value;
                    foreach (var item in enumerable)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (configItemPrimitiveCollectionAttribute.Converter != null)
                        {
                            value = configItemPrimitiveCollectionAttribute.Converter.ConvertTo(proInfo, item);
                        }
                        else
                        {
                            if (item is string)
                            {
                                value = (string)item;
                            }
                            else
                            {
                                value = item.ToString();
                            }
                        }

                        XElement childEle = XmlEx.CreateXElement(attri.ChildName, _VALUE, value);
                        collectionEle.Add(childEle);
                    }
                }
                else
                {
                    Type colletionItemType = proInfo.PropertyType.GenericTypeArguments.First();
                    PropertyInfo[] collectionProInfoArr = GetTypePropertyInfos(colletionItemType);

                    foreach (var item in enumerable)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        XElement childEle = new XElement(attri.ChildName);
                        WriteConfigToXml(childEle, collectionProInfoArr, item, configAttributeTypes);
                        collectionEle.Add(childEle);
                    }
                }
            }
            ownerEle.Add(collectionEle);
        }

        private static void AddDes(XElement ele, ConfigAttribute attri)
        {
            if (attri.Des != null)
            {
                ele.Add(new XAttribute(nameof(attri.Des), attri.Des));
            }
        }
        #endregion






        private static PropertyInfo[] GetTypePropertyInfos(Type type)
        {
            return type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        private static ConfigRootAttribute GetRootConfigRootAttribute(Type configType, ConfigAttributeTypes configAttributeTypes)
        {
            ConfigRootAttribute configAttribute;
            Attribute attri = configType.GetCustomAttribute(configAttributeTypes.RootAttributeType, false);
            if (attri == null)
            {
                //未标记特性,创建默认值
                configAttribute = new ConfigRootAttribute(configType.Name, null);
            }
            else
            {
                configAttribute = (ConfigRootAttribute)attri;
            }

            return configAttribute;
        }









        #region 读配置
        /// <summary>
        /// 从文件读取配置
        /// </summary>
        /// <typeparam name="T">配置对象类型</typeparam>
        /// <param name="configFilePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        public static T ReadConfigFromFile<T>(string configFilePath) where T : class, new()
        {
            T config = new T();
            XDocument xdoc = XDocument.Load(configFilePath);
            PrimitiveReadConfigFromXml(xdoc, config);
            return config;
        }

        /// <summary>
        /// 从文件读取配置到指定的配置对象
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="config">指定的配置对象</param>
        public static void ReadConfigFromFile(string configFilePath, object config)
        {
            XDocument xdoc = XDocument.Load(configFilePath);
            PrimitiveReadConfigFromXml(xdoc, config);
        }

        /// <summary>
        /// 从xml配置中读取配置
        /// </summary>
        /// <typeparam name="T">配置对象类型</typeparam>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>配置对象</returns>
        public static T ReadConfigFromXml<T>(string xmlStr) where T : class, new()
        {
            T config = new T();
            XDocument xdoc = XDocument.Parse(xmlStr);
            PrimitiveReadConfigFromXml(xdoc, config);
            return config;
        }

        /// <summary>
        /// 从xml配置中读取配置到指定的配置对象中
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <param name="config">指定的配置对象</param>
        public static void ReadConfigFromXml(string xmlStr, object config)
        {
            XDocument xdoc = XDocument.Parse(xmlStr);
            PrimitiveReadConfigFromXml(xdoc, config);
        }

        /// <summary>
        /// 从XDocument配置中读取配置
        /// </summary>
        /// <typeparam name="T">配置对象类型</typeparam>
        /// <param name="xdoc">XDocument配置</param>
        /// <returns>配置对象</returns>
        public static T ReadConfigFromXml<T>(XDocument xdoc) where T : class, new()
        {
            T config = new T();
            PrimitiveReadConfigFromXml(xdoc, config);
            return config;
        }

        /// <summary>
        /// 从xml配置中读取配置到指定的配置对象中
        /// </summary>
        /// <param name="xdoc">xml配置</param>
        /// <param name="config">指定的配置对象</param>
        public static void ReadConfigFromXml(XDocument xdoc, object config)
        {
            PrimitiveReadConfigFromXml(xdoc, config);
        }

        /// <summary>
        /// 从xml配置中读取配置到指定的配置对象中
        /// </summary>
        /// <param name="xdoc">xml配置</param>
        /// <param name="config">指定的配置对象</param>
        private static void PrimitiveReadConfigFromXml(XDocument xdoc, object config)
        {
            ConfigAttributeTypes configAttributeTypes = new ConfigAttributeTypes();
            Type configType = config.GetType();
            ConfigRootAttribute configRootAttribute = GetRootConfigRootAttribute(configType, configAttributeTypes);
            XElement rootEle = xdoc.XPathSelectElement(configRootAttribute.GetName(configType));
            if (rootEle == null)
            {
                return;
            }

            PropertyInfo[] proInfoArr = GetTypePropertyInfos(configType);
            ReadConfigToXml(rootEle, proInfoArr, config, configAttributeTypes);
        }

        private static void ReadConfigToXml(XElement ownerEle, PropertyInfo[] proInfoArr, object ownerObj, ConfigAttributeTypes configAttributeTypes)
        {
            ConfigAttribute attri;
            Dictionary<string, XElement> eleDic = ownerEle.Elements().ToDictionary(t => { return t.Name.LocalName; });
            string eleName;
            XElement ele;

            foreach (var proInfo in proInfoArr)
            {
                attri = proInfo.PropertyType.GetCustomAttribute(configAttributeTypes.IgnoreAttributeType, false) as ConfigAttribute;
                if (attri != null)
                {
                    //忽略该属性
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.CustomerAttribute, false) as ConfigAttribute;
                if (attri != null)
                {
                    //自定义
                    eleName = attri.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }

                    var configItemCustomerAttribute = (ConfigItemCustomerAttribute)attri;
                    configItemCustomerAttribute.CustomerConfig.Read(proInfo, ownerEle, configItemCustomerAttribute);
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.CollectionAttributeType, false) as ConfigAttribute;
                if (attri != null)
                {
                    //集合
                    eleName = attri.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }

                    ReadCollection(ele, (ConfigItemCollectionAttribute)attri, proInfo, configAttributeTypes, ownerObj);
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.ComplexItemAttribute, false) as ConfigAttribute;
                if (attri != null)
                {
                    //复合对象
                    eleName = attri.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }

                    ReadComplex(ele, configAttributeTypes, attri, proInfo, ownerObj);
                    continue;
                }

                attri = proInfo.GetCustomAttribute(configAttributeTypes.ItemAttributeType, false) as ConfigAttribute;
                if (attri != null)
                {
                    //基元项
                    eleName = attri.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }

                    ReadItem(ele, (ConfigItemAttribute)attri, proInfo, ownerObj);
                    continue;
                }


                //未标记项                
                if (proInfo.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null &&
                    Type.GetTypeCode(proInfo.PropertyType) != TypeCode.String)
                {
                    //集合
                    Type colletionItemType = proInfo.PropertyType.GenericTypeArguments.First();
                    ConfigItemCollectionAttribute configItemCollectionAttribute;
                    if (colletionItemType.IsClass &&
                        Type.GetTypeCode(colletionItemType) != TypeCode.String)
                    {
                        //非string类型的类
                        configItemCollectionAttribute = new ConfigItemCollectionAttribute(proInfo.Name, null, _CHILD_ITEM_NAME);
                    }
                    else
                    {
                        configItemCollectionAttribute = new ConfigItemPrimitiveCollectionAttribute(proInfo.Name, null, _CHILD_ITEM_NAME, null, false);
                    }

                    eleName = configItemCollectionAttribute.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }
                    ReadCollection(ele, configItemCollectionAttribute, proInfo, configAttributeTypes, ownerObj);
                }
                else if (proInfo.PropertyType.IsClass && Type.GetTypeCode(proInfo.PropertyType) != TypeCode.String)
                {
                    //复合对象
                    attri = new ConfigComplexItemAttribute(proInfo.Name, null);
                    eleName = attri.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }

                    ReadComplex(ele, configAttributeTypes, attri, proInfo, ownerObj);
                }
                else
                {
                    //基元项
                    attri = new ConfigItemAttribute(proInfo.Name, null);
                    eleName = attri.GetName(proInfo);
                    if (!eleDic.TryGetValue(eleName, out ele))
                    {
                        //属性对应的配置项不存在,忽略
                        continue;
                    }

                    ReadItem(ele, (ConfigItemAttribute)attri, proInfo, ownerObj);
                }
            }
        }

        private static void ReadItem(XElement ele, ConfigItemAttribute attri, PropertyInfo proInfo, object ownerObj)
        {
            string valueStr = XmlEx.GetXElementAttributeValue(ele, _VALUE);
            object value;
            if (attri.Converter != null)
            {
                value = attri.Converter.ConvertFrom(proInfo, valueStr);
            }
            else
            {
                value = valueStr;
                if (Type.GetTypeCode(proInfo.PropertyType) != TypeCode.String)
                {
                    value = ConvertEx.ToObject(proInfo.PropertyType, value);
                }
            }

            proInfo.SetValue(ownerObj, value, null);
        }

        private static void ReadComplex(XElement ele, ConfigAttributeTypes configAttributeTypes, ConfigAttribute attri, PropertyInfo proInfo, object ownerObj)
        {
            object value = proInfo.GetValue(ownerObj, null);
            if (value == null)
            {
                value = Activator.CreateInstance(proInfo.PropertyType);
                proInfo.SetValue(ownerObj, value, null);
            }

            PropertyInfo[] complexProInfoArr = GetTypePropertyInfos(proInfo.PropertyType);
            ReadConfigToXml(ele, complexProInfoArr, value, configAttributeTypes);
        }

        private static void ReadCollection(XElement ele, ConfigItemCollectionAttribute attri, PropertyInfo proInfo, ConfigAttributeTypes configAttributeTypes, object ownerObj)
        {
            var configItemEleArr = ele.Elements(attri.ChildName);
            if (configItemEleArr.Count() == 0)
            {
                return;
            }

            object collection = proInfo.GetValue(ownerObj, null);
            if (collection == null)
            {
                collection = Activator.CreateInstance(proInfo.PropertyType);
                proInfo.SetValue(ownerObj, collection, null);
            }
            var list = (IList)collection;

            Type colletionItemType = proInfo.PropertyType.GenericTypeArguments.First();
            object value;
            if (attri is ConfigItemPrimitiveCollectionAttribute)
            {
                bool noString = Type.GetTypeCode(colletionItemType) != TypeCode.String;
                foreach (XElement configItemEle in configItemEleArr)
                {
                    value = XmlEx.GetXElementAttributeValue(configItemEle, _VALUE);
                    if (noString)
                    {
                        value = ConvertEx.ToObject(colletionItemType, value);
                    }

                    list.Add(value);
                }
            }
            else
            {
                PropertyInfo[] configItemProInfoArr = GetTypePropertyInfos(colletionItemType);
                foreach (XElement configItemEle in configItemEleArr)
                {
                    value = Activator.CreateInstance(colletionItemType);
                    ReadConfigToXml(configItemEle, configItemProInfoArr, value, configAttributeTypes);
                    list.Add(value);
                }
            }
        }
        #endregion
    }
}
