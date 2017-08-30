using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using UtilZ.Lib.Base;
using UtilZ.Lib.Base.NLog;

namespace UtilZ.Lib.Winform.PropertyGrid
{
    /// <summary>
    /// Demo模型
    /// </summary>
    internal class ModelDemo
    {
        /// <summary>
        /// 获取或设置文件
        /// </summary>
        [Category("文件系统")]
        [DisplayName("文件")]
        [Description("获取或设置文件")]
        [EditorAttribute(typeof(PropertyGridFileEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string File { get; set; }

        /// <summary>
        /// 获取或设置目录
        /// </summary>
        [Category("文件系统")]
        [DisplayName("目录")]
        [Description("获取或设置目录")]
        [DefaultValue(@"E:\Tmp\Test")]
        [EditorAttribute(typeof(PropertyGridDirectoryEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Directory { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Category("个人信息")]
        [DisplayName("枚举性别")]
        [Description("获取或设置年龄")]
        public PropertyGridSexEnum SexEnum { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Category("个人信息")]
        [DisplayName("性别")]
        [Description("获取或设置性别")]
        [DefaultValue(PropertyGridSexEnum.F)]
        [Editor(typeof(PropertyGridCustomEnumEditor), typeof(UITypeEditor))]
        public PropertyGridSexEnum Sex { get; set; }

        /// <summary>
        /// 地图居中坐标纬度
        /// </summary>
        private double _mapCenterLatitude = 35d;

        /// <summary>
        /// 获取或设置地图居中坐标纬度
        /// </summary>
        [Category("常规")]
        [Description("态势启动时地图地图居中坐标纬度;-999取当前地图居中坐标纬度")]
        [DisplayName("地图居中坐标纬度")]
        [PropertyGridAttribute]
        [TypeConverter(typeof(LonLatPropertyTypeConverter))]
        public double MapCenterLatitude
        {
            get { return _mapCenterLatitude; }
            set { _mapCenterLatitude = value; }
        }
    }

    /// <summary>
    /// 经度纬度表格属性转换器[此类暂时不使用]
    /// </summary>
    [Serializable]
    internal class LonLatPropertyTypeConverter : TypeConverter
    {
        /// <summary>
        /// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型[是否从显示文本转换为真实对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="sourceType">表示要转换的类型</param>
        /// <returns>如果该转换器能够执行转换，则为 true；否则为 false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (context.PropertyDescriptor.PropertyType == NClrSystemType.DoubleType)
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的对象转换为此转换器的类型[从显示文本转换为真实对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="culture">用作当前区域性的 CultureInfo</param>
        /// <param name="value">要转换的 Object</param>
        /// <returns>表示转换的 value 的 Object</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (context.PropertyDescriptor.PropertyType == NClrSystemType.DoubleType)
            {
                double realValue;
                if (PostureCommon.ConvertCoordStrToCoord(value, out realValue))
                {
                    return realValue;
                }
                else
                {
                    return base.ConvertFrom(context, culture, value);
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        /// <summary>
        /// 返回此转换器是否可以使用指定的上下文将该对象转换为指定的类型[是否能转换为显示对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="destinationType">表示要转换到的类型</param>
        /// <returns>如果该转换器能够执行转换，则为 true；否则为 false</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (context.PropertyDescriptor.PropertyType == NClrSystemType.DoubleType)
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的值对象转换为指定的类型[转换为显示对象]
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="culture">如果传递 null，则采用当前区域性</param>
        /// <param name="value">要转换的 Object</param>
        /// <param name="destinationType">value 参数要转换成的 Type</param>
        /// <returns>表示转换的 value 的 Object</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (context.PropertyDescriptor.PropertyType == NClrSystemType.DoubleType)
            {
                return PostureCommon.ConvertToCoorString(Convert.ToDouble(value));
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }

    /// <summary>
    /// 态势公共方法类
    /// </summary>
    internal class PostureCommon
    {
        /// <summary>
        /// 经度或纬度度分秒格式化字符串
        /// </summary>
        private readonly static string _longitudeLatitudeAMSStr = @"{0}°{1}′{2}″";

        /// <summary>
        /// 经度或纬度坐标字符串(80°20′52″格式)拆分字符数组
        /// </summary>
        private readonly static char[] _longitudeLatitudeSplitArr = new char[] { '°', '′', '″' };

        #region 无效坐标判断
        /// <summary>
        /// 判断经度或纬度是否是无效的[true:无效,false:有效]
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="invalidLongitudeLatitude">无效经度值</param>
        /// <param name="invalidLatitude">无效纬度值</param>
        /// <returns>经度或纬度是否是无效的[true:无效,false:有效]</returns>
        public static bool IsNoValid(double longitude, double latitude, double invalidLongitude, double invalidLatitude)
        {
            return PostureCommon.LongitudeIsNoValid(longitude, invalidLongitude) || PostureCommon.LatitudeIsNoValid(latitude, invalidLatitude);
        }

        /// <summary>
        /// 判断经度是否是无效的[true:无效,false:有效]
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="invalidLongitudeLatitude">无效经度值</param>
        /// <returns>经度是否是无效的[true:无效,false:有效]</returns>
        public static bool LongitudeIsNoValid(double longitude, double invalidLongitudeLatitude)
        {
            //若经度为PostureConstants.InvalidLongitudeLatitude度度则为无效的经度
            return Math.Abs(longitude - invalidLongitudeLatitude) < 0.0001;
        }

        /// <summary>
        /// 判断纬度是否是无效的[true:无效,false:有效]
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="invalidLatitude">无效纬度值</param>
        /// <returns>纬度是否是无效的[true:无效,false:有效]</returns>
        public static bool LatitudeIsNoValid(double latitude, double invalidLatitude)
        {
            //若纬度为PostureConstants.InvalidLongitudeLatitude度则为无效的纬度
            return Math.Abs(latitude - invalidLatitude) < 0.0001;
        }
        #endregion

        /// <summary>
        /// 转换坐标值转换为坐标显示字符串,小数格式坐标为度分秒格式字符串(110.54->110°45′00″)
        /// </summary>
        /// <param name="longitudeStr">经度字符串</param>
        /// <param name="latitudeStr">纬度字符串</param>
        /// <returns>度分秒格式字符串</returns>
        public static string ConvertToCoorString(double longitude)
        {
            int a = 0, m = 0, s = 0;//度分秒
            a = (int)longitude;
            double md = (longitude - a) * 60;
            m = (int)md;
            s = (int)((md - m) * 60);
            return string.Format(PostureCommon._longitudeLatitudeAMSStr, a, m, s);
        }

        #region 坐标显示字符串转换为坐标值
        /// <summary>
        /// 转换经纬度坐标字符串为经纬度坐标值[转换成功返回true,否则返回false]
        /// </summary>
        /// <param name="objLongitude">经度坐标字符串值</param>
        /// <param name="objLatitude">纬度坐标字符串值</param>
        /// <param name="longitude">经度值</param>
        /// <param name="latitude">纬度值</param>
        /// <returns>转换成功返回true,否则返回false</returns>
        public static bool ConvertCoordStrToCoord(object objLongitude, object objLatitude, out double longitude, out double latitude)
        {
            longitude = 0;
            latitude = 0;

            try
            {
                //80°20′52″
                if (!PostureCommon.ConvertCoordStrToCoord(objLongitude, out longitude))
                {
                    return false;
                }

                return PostureCommon.ConvertCoordStrToCoord(objLatitude, out latitude);
            }
            catch (Exception ex)
            {
                Loger.Debug(ex);
                return false;
            }
        }

        /// <summary>
        /// 转换坐标字符串为坐标值[转换成功返回true,否则返回false]
        /// </summary>
        /// <param name="objValue">坐标字符串值</param>
        /// <param name="value">坐标值</param>
        /// <returns>转换成功返回true,否则返回false</returns>
        public static bool ConvertCoordStrToCoord(object objValue, out double value)
        {
            value = 0d;
            //如果经度值或纬度值为null则获取失败,返回false
            if (objValue == null || objValue is System.DBNull)
            {
                return false;
            }

            double a = 0d, m = 0d, s = 0d;
            //坐标字符串
            string coordStr = objValue.ToString();
            string[] longitudeArr = coordStr.Split(PostureCommon._longitudeLatitudeSplitArr, StringSplitOptions.RemoveEmptyEntries);
            if (longitudeArr.Length > 3)
            {
                return false;
            }

            if (longitudeArr.Length > 0)
            {
                a = double.Parse(longitudeArr[0]);
            }

            if (longitudeArr.Length > 1)
            {
                m = double.Parse(longitudeArr[1]);
            }

            if (longitudeArr.Length > 2)
            {
                s = double.Parse(longitudeArr[2]);
            }

            value = a + m / 60 + s / 3600;
            return true;
        }
        #endregion
    }

    /// <summary>
    /// PropertyGrid枚举
    /// </summary>
    [TypeConverter(typeof(PropertyGridEnumConverter))]
    internal enum PropertyGridSexEnum
    {
        /// <summary>
        /// M
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "男")]
        M,

        /// <summary>
        /// F
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "女")]
        F,

        /// <summary>
        /// O
        /// </summary>
        [NDisplayNameAttribute(DisplayName = "其它")]
        O
    }

    /*DEMO
     [DefaultProperty("Age")]
    public class PropertyGridStu : NBasePropertyValueVerifyModel, IPropertyGridFile, IPropertyGridDirectory, IPropertyGridDropDown
    {
        public PropertyGridStu()
        {
            this._addr.Text = "一级";
            this._addr.Value = 234;

            this.AddAddr(this._addr);
            this.AddAddr(new NAddress { Text = "NAddress1", Value = 1 });
            this.AddAddr(new NAddress { Text = "NAddress2", Value = 2 });
            this.AddAddr(new NAddress { Text = "NAddress3", Value = 3 });
        }

        private string _name = "张晓蝶";
        /// <summary>
        /// 获取或设置姓名
        /// </summary>
        [Category("个人信息")]
        [DisplayName("姓名")]
        [Description("获取或设置姓名")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                base.OnRaisePropertyChanged("Name");
            }
        }

        private int _age = 28;
        /// <summary>
        /// 获取或设置年龄
        /// </summary>
        [Category("个人信息")]
        [DisplayName("年龄")]
        [Description("获取或设置年龄")]
        [DefaultValue(28)]
        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0 || value > 200)
                {
                    base.OnRaisePropertyValueVerifyResultNotify(false, string.Format("年龄不能为负数或大于200,值:{0}无效", value));
                    return;
                }

                _age = value;
                base.OnRaisePropertyValueVerifyResultNotify(true, null);
            }
        }

        [Category("个人信息")]
        [DisplayName("枚举性别")]
        [Description("获取或设置年龄")]
        public SexEnum SexEnum { get; set; }

        /// <summary>
        /// 获取或设置性别
        /// </summary>
        [Category("个人信息")]
        [DisplayName("性别")]
        [Description("获取或设置性别")]
        [DefaultValue(SexEnum.F)]
        [Editor(typeof(PropertyGridCustomEnumEditor), typeof(UITypeEditor))]
        public SexEnum Sex { get; set; }

        /// <summary>
        /// 展开
        /// </summary>
        private Person p = new Person();
        [Category("人配置")]
        [DisplayName("人Person人")]
        public Person Person
        {
            get { return p; }
            set { this.p = value; }
        }

        /// <summary>
        /// 获取或设置文件
        /// </summary>
        [Category("文件系统")]
        [DisplayName("文件")]
        [Description("获取或设置文件")]
        //[DefaultValue(@"E:\Tmp\Test\file1.wav")]
        [EditorAttribute(typeof(PropertyGridFileEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string File { get; set; }

        /// <summary>
        /// 获取或设置文件
        /// </summary>
        [Category("文件系统")]
        [DisplayName("文件22")]
        [Description("获取或设置文件")]
        //[DefaultValue(@"E:\Tmp\Test\file1.wav")]
        [EditorAttribute(typeof(PropertyGridFileEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string File222 { get; set; }

        /// <summary>
        /// 获取或设置目录
        /// </summary>
        [Category("文件系统")]
        [DisplayName("目录")]
        [Description("获取或设置目录")]
        //[EditorBrowsable(EditorBrowsableState.Advanced)]
        [DefaultValue(@"E:\Tmp\Test")]
        [EditorAttribute(typeof(PropertyGridDirectoryEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Directory { get; set; }

        /// <summary>
        /// 获取或设置围棋背景
        /// </summary>
        [Category("围棋")]
        [DisplayName("围棋背景")]
        [Description("获取或设置围棋背景")]
        [DefaultValue(null)]
        public System.Drawing.Image Img { get; set; }

        #region IPropertyGridFile接口
        public string GetFileExtension(string fileFieldName)
        {
            if (fileFieldName.Equals("File"))
            {
                return ".wav";
            }
            else if (fileFieldName.Equals("File222"))
            {
                return ".exe";
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetFileName(string fileFieldName)
        {
            if (fileFieldName.Equals("File"))
            {
                return @"E:\Tmp\Test\file1.wav";
            }
            else
            {
                return @"G:\Soft\fences_public.exe";
            }
        }

        public string GetInitialDirectory(string fileFieldName)
        {
            if (fileFieldName.Equals("File"))
            {
                return @"G:\Soft";
            }
            else if (fileFieldName.Equals("File222"))
            {
                return @"G:\Soft\Nero10";
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region IPropertyGridDropDown接口
        private NAddress _addr = new NAddress();
        /// <summary>
        /// 地址信息
        /// </summary>
        [Category("地址")]
        [DisplayName("地址信息")]
        [Description("获取或设置地址信息")]
        [TypeConverter(typeof(PropertyGridDropDownListConverter))]
        public NAddress Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        private string _proTestStr = "Str1";
        [Category("下拉字符串")]
        [DisplayName("字符串")]
        [Description("下拉字符串")]
        [TypeConverter(typeof(PropertyGridDropDownListConverter))]
        public string ProTestStr
        {
            get { return _proTestStr; }
            set { _proTestStr = value; }
        }

        private readonly List<string> _pstrs = new List<string>();
        private readonly List<NAddress> _addrs = new List<NAddress>();

        public void AddAddr(NAddress addr)
        {
            _addrs.Add(addr);
        }

        public System.Collections.ICollection GetPropertyGridDropDownItems(string fileFieldName)
        {
            switch (fileFieldName)
            {
                case "Addr":
                    return _addrs;
                case "ProTestStr":
                    if (_pstrs.Count == 0)
                    {
                        _pstrs.Add(_proTestStr);
                        _pstrs.Add("Str2");
                        _pstrs.Add("Str3");
                        _pstrs.Add("Str4");
                    }

                    return _pstrs;
                default:
                    return null;
            }

        }

        public string GetPropertyGridDisplayName(string fileFieldName)
        {
            switch (fileFieldName)
            {
                case "Addr":
                    return "Text";
                default:
                    return null;
            }
        }
        #endregion

        #region IPropertyGridDirectory接口
        public string GetInitialSelectedPath(string fileFieldName)
        {
            if (fileFieldName.Equals("Directory"))
            {
                return @"C:\Users\zhanghn\Desktop\Breeze.Lib\Breeze.Lib.Base\PropertyGrid";
            }
            else if (fileFieldName.Equals("File222"))
            {
                return @"D:\Soft";
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }

    [TypeConverter(typeof(PropertyGridDropDownListConverter))]
    public class NAddress
    {
        public string Text { get; set; }

        public int Value { get; set; }
    }

    [TypeConverter(typeof(PropertyGridEnumConverter))]
    public enum SexEnum
    {
        [NDisplayNameAttribute(DisplayName = "男")]
        M,

        [NDisplayNameAttribute(DisplayName = "女")]
        F,

        [NDisplayNameAttribute(DisplayName = "其它")]
        O
    }

    internal class PersonConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            if (t == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo info, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    // parse the format "Last, First (Age)"
                    //
                    int comma = s.IndexOf(',');
                    if (comma != -1)
                    {
                        // now that we have the comma, get
                        // the last name.
                        string last = s.Substring(0, comma);
                        int paren = s.LastIndexOf('(');
                        if (paren != -1 && s.LastIndexOf(')') == s.Length - 1)
                        {
                            // pick up the first name
                            string first = s.Substring(comma + 1, paren - comma - 1);
                            // get the age
                            int age = Int32.Parse(
                            s.Substring(paren + 1,
                            s.Length - paren - 2));
                            Person p = new Person();
                            p.Age = age;
                            p.LastName = last.Trim();
                            p.FirstName = first.Trim();
                            return p;
                        }
                    }
                }
                catch { }
                // if we got this far, complain that we
                // couldn't parse the string
                //
                throw new ArgumentException("Can not convert '" + (string)value + "' to type Person");
            }

            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is Person)
            {
                Person p = (Person)value;
                // simply build the string as "Last, First (Age)"
                return p.LastName + ", " + p.FirstName + " (" + p.Age.ToString() + ")";
            }

            return base.ConvertTo(context, culture, value, destType);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Person
    {
        [DisplayName("年龄")]
        public int Age { get; set; }

        [DisplayName("第一个名字")]
        public string FirstName { get; set; }

        [DisplayName("最后一个名字")]
        public string LastName { get; set; }

        public override string ToString()
        {
            return this.LastName + ", " + this.FirstName + " (" + this.Age.ToString() + ")";
            //return base.ToString();
        }
    }
     **/
}
