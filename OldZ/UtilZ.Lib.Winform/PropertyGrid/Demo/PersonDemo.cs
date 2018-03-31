using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UtilZ.Lib.Winform.PropertyGrid.Base;
using UtilZ.Lib.Winform.PropertyGrid.Interface;
using UtilZ.Lib.Winform.PropertyGrid.TypeConverters;

namespace UtilZ.Lib.Winform.PropertyGrid.Demo
{
    /// <summary>
    /// PropertyGrid排序Demo
    /// </summary>
    [TypeConverter(typeof(PropertyGridSortConverter))]
    [DefaultProperty("Name")]
    public class PersonDemo : IPropertyGridCategoryOrder, IPropertyGridCollection
    {
        private string _name = "Bob";
        private string _name1 = "Bob1";
        private DateTime _birthday = new DateTime(1975, 1, 1);

        /// <summary>
        /// Name
        /// </summary>
        [DisplayName("名称A"), Category("A"), PropertyGridOrder(10)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Name1
        /// </summary>
        [DisplayName("名称A1"), Category("A"), PropertyGridOrder(90)]
        public string Name1
        {
            get { return _name1; }
            set { _name1 = value; }
        }

        /// <summary>
        /// NameB
        /// </summary>
        [DisplayName("名称B"), Category("B"), PropertyGridOrder(11)]
        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        /// <summary>
        /// NameB2
        /// </summary>
        [DisplayName("名称B2"), Category("B"), PropertyGridOrder(6)]
        public DateTime Birthday2
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        /// <summary>
        /// NameC
        /// </summary>
        [DisplayName("名称C"), Category("C"), PropertyGridOrder(12)]
        public int Age
        {
            get
            {
                TimeSpan age = DateTime.Now - _birthday;
                return (int)age.TotalDays / 365;
            }
        }

        /// <summary>
        /// InnerPerson
        /// </summary>
        [DisplayName("定制属性顺序"), Category("其它"), PropertyGridOrder(29)]
        public Person InnerPerson { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        [Browsable(false)]
        public PropertyGridOrderType OrderType
        {
            get { return PropertyGridOrderType.Ascending; }
        }


        #region IPropertyGridCategoryOrder接口
        //private List<string> categorys = new List<string>() { "B", "A", "C" };//排序
        private List<string> categorys = new List<string>() { "B", "C", "A", "其它" };//排序

        /// <summary>
        /// 表格排序组名称列表
        /// </summary>
        [Browsable(false)]
        public List<string> PropertyGridCategoryNames
        {
            get { return categorys; }
        }
        #endregion

        /// <summary>
        /// 员工集合
        /// </summary>
        [DisplayName("员工")]
        public PropertyGridCollection<Employee> Employees { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonDemo()
        {
            InnerPerson = new Person();
            this.Employees = new PropertyGridCollection<Employee>();
            Employee emp1 = new Employee();
            emp1.FirstName = "Max";
            emp1.Age = 42;
            this.Employees.Add(emp1);

            Employee emp2 = new Employee();
            emp2.FirstName = "Lara";
            emp2.Age = 24;
            this.Employees.Add(emp2);
        }

        /// <summary>
        /// 获取集合显示信息
        /// </summary>
        public string GetCollectionDisplayInfo(string propertyName)
        {
            switch (propertyName)
            {
                case "Employees":
                    return "Employees集合显示信息";
                default:
                    return "XXX";
            }
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Employee is our sample business or domin object. It derives from the general base class Person.
    /// </summary>
    [TypeConverter(typeof(PropertyGridCollectionItemConverter))]
    public class Employee//: IPropertyGridCollectionItem
    {
        public Employee()
        {
        }

        private string firstName = "";
        private int age = 0;

        [Category("Required")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Category("Optional")]
        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        // Meaningful text representation
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.FirstName);
            sb.Append(",");
            sb.Append(this.Age);
            return sb.ToString();
        }

        public string GetItemInfo()
        {
            return string.Format("集合项显示信息:{0}", FirstName);
        }
    }
}
