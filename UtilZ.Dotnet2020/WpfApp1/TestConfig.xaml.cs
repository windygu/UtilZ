using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UtilZ.DotnetStd.Ex.Base;
using UtilZ.DotnetStd.Ex.Log;

namespace WpfApp1
{
    /// <summary>
    /// TestConfig.xaml 的交互逻辑
    /// </summary>
    public partial class TestConfig : Window
    {
        public TestConfig()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private readonly string _configFilePath = @"config.xml";
        private readonly string _configFilePath2 = @"config2.xml";
        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = new ConfigDemo() { Age = 18, Names = new List<string>() { "zzx", "qaz" } };
                config.Child = new List<ConfigChildItem>()
                {
                    new ConfigChildItem(){ ID=1, Name="李莫愁", Child2=new ConfigChildItem2(){ ID=11, Bir=DateTime.Parse("2000-01-02 15:22:23")}},
                    new ConfigChildItem(){ ID=2, Name="yy", Child2=new ConfigChildItem2(){ ID=21, Bir=DateTime.Parse("1988-01-17 00:00:00")}},
                };




                var xdoc = ConfigHelper.WriteConfigToXDocument(config);
                xdoc.Save(this._configFilePath);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = ConfigHelper.ReadConfigFromFile<ConfigDemo>(this._configFilePath);
                ConfigHelper.WriteConfigToXmlFile(config, _configFilePath2);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
            }
        }
    }


    //[ConfigRoot("Config")]
    public class ConfigDemo
    {
       // [ConfigItem("年龄", "1-200")]
        public int Age { get; set; }

       // [ConfigItemPrimitiveCollection("名字列表", "好多名称", "Name")]
        public List<string> Names { get; set; }

       // [ConfigItemCollection("ChildList", null, "Child")]
        public List<ConfigChildItem> Child { get; set; }
    }

    public class ConfigChildItem
    {
        //[ConfigItem("ID", "编号1...9999")]
        public long ID { get; set; }

        //[ConfigItem("姓名", "人的代号")]
        public string Name { get; set; }

       // [ConfigComplexItem("ComplexChild", null, false)]
        public ConfigChildItem2 Child2 { get; set; }
    }

    public class ConfigChildItem2
    {
       // [ConfigItem("ChildItem2.ID", "编号1...9999")]
        public long ID { get; set; }

        //[ConfigItemAttribute("Bir", "生日", typeof(DateTimeConfigValueConverter), true)]
        public DateTime Bir { get; set; }
    }


    public class DateTimeConfigValueConverter : IConfigValueConverter
    {
        public object ConvertFrom(PropertyInfo propertyInfo, string value)
        {
            return DateTime.Parse(value);
        }

        public string ConvertTo(PropertyInfo propertyInfo, object value)
        {
            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
