using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Winform.PropertyGrid.Base;
using UtilZ.Lib.Winform.PropertyGrid.Interface;

namespace UtilZ.Lib.Winform.PropertyGrid.TypeConverters
{
    public class PropertyGridSortConverter : ExpandableObjectConverter
    {
        #region Methods
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc = base.GetProperties(context, value, attributes);
            string[] orderedPropertyNames;
            if (context.PropertyDescriptor.PropertyType.GetInterface(typeof(IPropertyGridOrder).FullName) != null)
            {
                IPropertyGridOrder propertyGridOrder = (IPropertyGridOrder)context.Instance;
                if (propertyGridOrder.OrderType == PropertyGridOrderType.Custom)
                {
                    var srcOrderedPropertyNames = new List<string>();
                    foreach (PropertyDescriptor pd in pdc)
                    {
                        srcOrderedPropertyNames.Add(pd.Name);
                    }

                    orderedPropertyNames = propertyGridOrder.GetCustomSortPropertyName(srcOrderedPropertyNames);
                }
                else
                {
                    orderedPropertyNames = this.GetOrderedPropertyNames(pdc, propertyGridOrder.OrderType == PropertyGridOrderType.Ascending);
                }
            }
            else
            {
                orderedPropertyNames = this.GetOrderedPropertyNames(pdc, true);
            }

            return pdc.Sort(orderedPropertyNames);
        }

        /// <summary>
        /// 获取属性名称排序列表
        /// </summary>
        /// <param name="pdc">PropertyDescriptorCollection</param>
        /// <param name="orderFlag">true:升序;false:降序</param>
        /// <returns></returns>
        private string[] GetOrderedPropertyNames(PropertyDescriptorCollection pdc, bool orderFlag)
        {
            Type orderType = typeof(PropertyGridOrderAttribute);
            //PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
            var orderPropertyDescriptors = new List<Tuple<int, PropertyDescriptor>>();
            var noOrderPropertyNames = new List<string>();
            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attribute = pd.Attributes[orderType];
                if (attribute != null)
                {
                    PropertyGridOrderAttribute poa = (PropertyGridOrderAttribute)attribute;
                    orderPropertyDescriptors.Add(new Tuple<int, PropertyDescriptor>(poa.Order, pd));
                }
                else
                {
                    noOrderPropertyNames.Add(pd.Name);
                }
            }

            List<string> orderedProperties;
            if (orderFlag)
            {
                orderedProperties = (from tmpItem in orderPropertyDescriptors orderby tmpItem.Item1 ascending select tmpItem.Item2.Name).ToList();
            }
            else
            {
                orderedProperties = (from tmpItem in orderPropertyDescriptors orderby tmpItem.Item1 descending select tmpItem.Item2.Name).ToList();
            }

            if (noOrderPropertyNames.Count > 0)
            {
                noOrderPropertyNames = (from tmpItem in noOrderPropertyNames orderby tmpItem ascending select tmpItem).ToList();
                orderedProperties.AddRange(noOrderPropertyNames);
            }

            return orderedProperties.ToArray();
        }

        //public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        //{
        //    PropertyDescriptorCollection pdc = base.GetProperties(context, value, attributes);
        //    //return pdc;
        //    Type orderType = typeof(PropertyGridOrderAttribute);

        //    //PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
        //    var orderPropertyDescriptors = new List<Tuple<int, PropertyDescriptor>>();
        //    var noOrderPropertyNames = new List<string>();

        //    foreach (PropertyDescriptor pd in pdc)
        //    {
        //        Attribute attribute = pd.Attributes[orderType];
        //        if (attribute != null)
        //        {
        //            PropertyGridOrderAttribute poa = (PropertyGridOrderAttribute)attribute;
        //            orderPropertyDescriptors.Add(new Tuple<int, PropertyDescriptor>(poa.Order, pd));
        //        }
        //        else
        //        {
        //            noOrderPropertyNames.Add(pd.Name);
        //        }
        //    }

        //    var orderedProperties = (from tmpItem in orderPropertyDescriptors orderby tmpItem.Item1 ascending select tmpItem.Item2.Name).ToList();
        //    orderedProperties.AddRange(noOrderPropertyNames);
        //    return pdc.Sort(orderedProperties.ToArray());
        //}
        #endregion
    }
}
