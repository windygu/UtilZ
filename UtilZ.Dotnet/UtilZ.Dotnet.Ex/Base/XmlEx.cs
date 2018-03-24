using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UtilZ.Dotnet.Ex.Base
{
    /// <summary>
    /// Xml辅助类
    /// </summary>
    public static class XmlEx
    {
        /// <summary>
        /// 获取XElement元素属性值
        /// </summary>
        /// <param name="ele">XElement节点</param>
        /// <param name="attributeName">属性名称</param>
        /// <returns>值</returns>
        public static string GetXElementAttributeValue(this XElement ele, string attributeName)
        {
            if (ele == null)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentNullException(ObjectEx.GetVarName(p => attributeName));
            }

            XAttribute attri = ele.Attribute(attributeName);
            if (attri == null)
            {
                return string.Empty;
            }
            else
            {
                return attri.Value;
            }
        }

        /// <summary>
        /// 获取XElement元素节点值[节点为null时返回空字符串]
        /// </summary>
        /// <param name="ele">XElement节点</param>
        /// <returns>值</returns>
        public static string GetXElementValue(this XElement ele)
        {
            if (ele == null)
            {
                return string.Empty;
            }
            else
            {
                return ele.Value;
            }
        }

        /// <summary>
        /// 创建特性值xml元素节点
        /// </summary>
        /// <param name="eleName">节点名称</param>
        /// <param name="value">值</param>
        /// <param name="valueAttrName">特性名称</param>
        /// <returns>特性值xml元素节点</returns>
        public static XElement CreateXElement(string eleName, object value, string valueAttrName)
        {
            XElement ele = new XElement(eleName);
            ele.Add(new XAttribute(valueAttrName, value));
            return ele;
        }

        /// <summary>
        /// 创建CDataxml元素节点
        /// </summary>
        /// <param name="eleName">节点名称</param>
        /// <param name="value">值</param>
        /// <returns>CDataxml元素节点</returns>
        public static XElement CreateXCDataXElement(string eleName, string value)
        {
            XElement ele = new XElement(eleName);
            ele.Add(new XCData(value));
            return ele;
        }
    }
}
