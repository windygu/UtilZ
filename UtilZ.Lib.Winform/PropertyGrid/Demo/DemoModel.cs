﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using UtilZ.Lib.Winform.PropertyGrid.Base;
using UtilZ.Lib.Winform.PropertyGrid.Interface;
using UtilZ.Lib.Winform.PropertyGrid.TypeConverters;
using UtilZ.Lib.Winform.PropertyGrid.UITypeEditors;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    /// <summary>
    /// Demo模型
    /// </summary>
    public class DemoModel : PropertyValueVerifyBase, IPropertyGridFile, IPropertyGridDirectory, IPropertyGridDropDown, IPropertyGridPassword
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DemoModel()
        {
            this._addrs.Add(new NAddress { Text = "NAddress1", Value = 1 });
            this._addrs.Add(new NAddress { Text = "NAddress1", Value = 1 });
            this._addrs.Add(new NAddress { Text = "NAddress2", Value = 2 });
            this._addrs.Add(new NAddress { Text = "NAddress3", Value = 3 });

            _pstrs.Add("Str1");
            _pstrs.Add("Str2");
            _pstrs.Add("Str3");
            _pstrs.Add("Str4");
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
        /// 数据库密码
        /// </summary>
        private string _password = string.Empty;

        /// <summary>
        /// 获取或设置数据库连接密码
        /// </summary>
        [Category("数据库")]
        [Description("数据库访问密码")]
        [DisplayName("密码")]
        [TypeConverter(typeof(PropertyGridPasswordConverter))]
        [PropertyGrid]
        public string Password
        {
            get { return _password; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    base.OnRaisePropertyValueVerifyResultNotify(false, "数据库密码不能为空");
                    return;
                }

                _password = value;
                base.OnRaisePropertyValueVerifyResultNotify(true, null);
            }
        }

        /// <summary>
        /// 自定义编辑控件
        /// </summary>
        [Category("个人信息")]
        [DisplayName("自定义编辑控件")]
        [Description("演示自定义编辑控件")]
        [DefaultValue(DirectionEnum.Left)]
        [Editor(typeof(PropertyGridCustomEnumEditor), typeof(UITypeEditor))]
        public DirectionEnum Direction { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Category("个人信息")]
        [DisplayName("枚举性别")]
        [Description("获取或设置年龄")]
        [TypeConverter(typeof(PropertyGridEnumConverter))]
        public SexEnum Sex { get; set; }

        /// <summary>
        /// 获取或设置围棋背景
        /// </summary>
        [Category("围棋")]
        [DisplayName("围棋背景")]
        [Description("获取或设置围棋背景")]
        [DefaultValue(null)]
        public System.Drawing.Image Img { get; set; }

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
        /// 基元类型
        /// </summary>
        [Category("基元类型")]
        [DisplayName("Primitive")]
        [Description("下拉基元类型")]
        [TypeConverter(typeof(PropertyGridDropDownListConverter))]
        public string PrimitiveType { get; set; }

        /// <summary>
        /// 地址信息
        /// </summary>
        [Category("地址")]
        [DisplayName("地址信息")]
        [Description("获取或设置地址信息")]
        [TypeConverter(typeof(PropertyGridDropDownListConverter))]
        public NAddress Addr { get; set; }

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
        [TypeConverter(typeof(DemoLonLatTypeConverter))]
        public double MapCenterLatitude
        {
            get { return _mapCenterLatitude; }
            set { _mapCenterLatitude = value; }
        }

        #region IPropertyGridPassword接口
        /// <summary>
        /// 获取密码显示字符
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>密码显示字符</returns>
        public char GetPasswordChar(string propertyName)
        {
            return '*';
        }
        #endregion

        #region IPropertyGridFile接口
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="propertyName">要获取扩展名的文件字段名称</param>
        public string GetFileExtension(string propertyName)
        {
            if (propertyName.Equals("File"))
            {
                return ".wav";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取初始包含目录的全路径文件名,默认请返回null[当GetFileName有返回值时,GetInitialDirectory不调用]
        /// </summary>
        /// <param name="propertyName">要获取扩展名的文件字段名称</param>
        public string GetFileName(string propertyName)
        {
            if (propertyName.Equals("File"))
            {
                return @"F:\Soft\feiq.rar";
            }
            else
            {
                return @"F:\Soft\fences_public.exe";
            }
        }

        /// <summary>
        /// 获取初始目录,默认请返回null[当GetFileName有返回值时,GetInitialDirectory不调用]
        /// </summary>
        /// <param name="propertyName">要获取扩展名的文件字段名称</param>
        public string GetInitialDirectory(string propertyName)
        {
            if (propertyName.Equals("File"))
            {
                return @"G:\Soft";
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region IPropertyGridDropDown接口
        private readonly List<NAddress> _addrs = new List<NAddress>();
        private readonly List<string> _pstrs = new List<string>();

        /// <summary>
        /// 获取表格下拉框选择列表集合
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>表格下拉框选择列表集合</returns>
        public System.Collections.ICollection GetPropertyGridDropDownItems(string propertyName)
        {
            switch (propertyName)
            {
                case "Addr":
                    return _addrs;
                case "PrimitiveType":
                    return _pstrs;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取下拉列表项对象显示项属性名称
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>下拉列表项对象显示项属性名称</returns>
        public string GetPropertyGridDisplayName(string propertyName)
        {
            switch (propertyName)
            {
                case "Addr":
                    return "Text";
                default:
                    return null;
            }
        }
        #endregion

        #region IPropertyGridDirectory接口
        /// <summary>
        /// 获取初始目录
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        public string GetInitialSelectedPath(string propertyName)
        {
            if (propertyName.Equals("Directory"))
            {
                return @"D:\";
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}