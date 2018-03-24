using UtilZ.Dotnet.Ex.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UtilZ.Dotnet.Ex.Model
{
    /// <summary>
    /// 模型基类
    /// </summary>
    [Serializable]
    public class BaseModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// 属性值改变通知事件
        /// </summary>
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性值改变通知方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected void OnRaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        /// <summary>
        /// 不显示特性类型
        /// </summary>
        private static readonly Type _browsableType = typeof(BrowsableAttribute);

        /// <summary>
        /// 显示名称特性类型
        /// </summary>
        private static readonly Type _displayNameType = typeof(DisplayNameAttribute);

        /// <summary>
        /// 显示格式特性类型
        /// </summary>
        private static readonly Type _formatAttributeType = typeof(FormatAttribute);

        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="lenth">显示字符串总长度</param>
        /// <param name="dicIndexPropertes">索引属性字典集合</param>
        /// <returns>字符串</returns>
        public string ToString(int lenth, Dictionary<string, object[]> dicIndexPropertes = null)
        {
            if (dicIndexPropertes == null)
            {
                dicIndexPropertes = new Dictionary<string, object[]>();
            }

            StringBuilder sb = new StringBuilder();
            Type type = this.GetType();
            object[] attrs = null;
            string title = null;//显示标题
            object value = null;//显示值
            string line = null;//当前行字符串
            int chineseCharCount = 0;//已计算的中文字符数
            int lineChineseCharCount = 0;//行中文字符数

            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];
                attrs = propertyInfo.GetCustomAttributes(BaseModel._browsableType, false);
                if (attrs.Length != 0)
                {
                    if (!((BrowsableAttribute)attrs[0]).Browsable)
                    {
                        //不可见,无视
                        continue;
                    }
                }

                //显示名称
                attrs = propertyInfo.GetCustomAttributes(BaseModel._displayNameType, false);
                if (attrs.Length == 0)
                {
                    title = propertyInfo.Name;
                }
                else
                {
                    title = ((DisplayNameAttribute)attrs[0]).DisplayName;
                }

                //值
                if (dicIndexPropertes.ContainsKey(propertyInfo.Name))
                {
                    value = propertyInfo.GetValue(this, dicIndexPropertes[propertyInfo.Name]);
                }
                else
                {
                    value = propertyInfo.GetValue(this, null);
                }

                //生成行字符串
                attrs = propertyInfo.GetCustomAttributes(BaseModel._formatAttributeType, false);
                if (attrs.Length != 0)
                {
                    try
                    {
                        value = string.Format(((FormatAttribute)attrs[0]).FormatString, value);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("属性:{0}上的格式化显示特性参数无效,错误信息:{1}", propertyInfo.Name, ex.Message));
                    }
                }

                line = string.Format("{0}:{1}", title, value == null ? string.Empty : value.ToString());
                //计算行字符串中中文字符数
                lineChineseCharCount = StringEx.CalculateChineseCharCount(line);

                //当超过lenth个长度时,则中断循环
                //(i+1):换行符数
                if (sb.Length + chineseCharCount + (i + 1) + lineChineseCharCount + line.Length > lenth)
                {
                    break;
                }

                //如果不是第一行,则先添加一个换行符
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }

                //添加行
                sb.Append(line);
                //累加已添加的详情中的中文字符数
                chineseCharCount += lineChineseCharCount;
            }

            return sb.ToString();
        }

        /// <summary>
        /// 数据模型用于绑定时允许编辑的列集合
        /// </summary>
        /// <returns>允许编辑的列集合</returns>
        public virtual IEnumerable<string> GetAllowEditColumns()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var browsableType = typeof(BrowsableAttribute);
            object[] attrObj;
            var allowEditColumns = new List<string>();
            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }

                attrObj = propertyInfo.GetCustomAttributes(browsableType, true);
                if (attrObj.Length > 0 && ((BrowsableAttribute)attrObj[0]).Browsable == false)
                {
                    continue;
                }

                allowEditColumns.Add(propertyInfo.Name);
            }

            return allowEditColumns;
        }
    }
}
