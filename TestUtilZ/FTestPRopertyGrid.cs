using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UtilZ.Lib.Winform.PropertyGrid.Base;
using UtilZ.Lib.Winform.PropertyGrid.Demo;
using UtilZ.Lib.Winform.PropertyGrid.Interface;
using UtilZ.Lib.Winform.PropertyGrid.TypeConverters;

namespace TestUtilZ
{
    public partial class FTestPRopertyGrid : Form
    {
        public FTestPRopertyGrid()
        {
            InitializeComponent();
        }

        private readonly DemoModel _demoModel = new DemoModel();

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }

            propertyGrid1.SelectedObjectsChanged += new EventHandler(propertyGrid1_SelectedObjectsChanged);
            propertyGrid1.SelectedObject = new Person();
            //propertyGrid1.SelectedObject = _demoModel;
        }

        void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            propertyGrid1.Tag = propertyGrid1.PropertySort;
            propertyGrid1.PropertySort = PropertySort.CategorizedAlphabetical;
            propertyGrid1.Paint += new PaintEventHandler(propertyGrid1_Paint);
        }

        void propertyGrid1_Paint(object sender, PaintEventArgs e)
        {
            //try
            //{
            //    if (propertyGrid1.SelectedObject == null)
            //    {
            //        return;
            //    }

            //    if (propertyGrid1.SelectedObject.GetType().GetInterface(typeof(IPropertyGridCategoryOrder).FullName) == null)
            //    {
            //        return;
            //    }

            //    IPropertyGridCategoryOrder propertyGridCategoryOrder = (IPropertyGridCategoryOrder)propertyGrid1.SelectedObject;
            //    List<string> propertyGridCategoryNames = propertyGridCategoryOrder.PropertyGridCategoryNames;
            //    switch (propertyGridCategoryOrder.OrderType)
            //    {
            //        case PropertyGridOrderType.Ascending:
            //            propertyGridCategoryNames = (from tmpItem in propertyGridCategoryNames orderby tmpItem ascending select tmpItem).ToList();
            //            break;
            //        case PropertyGridOrderType.Descending:
            //            propertyGridCategoryNames = (from tmpItem in propertyGridCategoryNames orderby tmpItem descending select tmpItem).ToList();
            //            break;
            //        case PropertyGridOrderType.Custom:
            //            break;
            //    }

            //    GridItemCollection currentPropEntries = propertyGrid1.GetType().GetField("currentPropEntries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(propertyGrid1) as GridItemCollection;
            //    propertyGrid1.CollapseAllGridItems();
            //    var newarray = currentPropEntries.Cast<GridItem>().OrderBy((t) => propertyGridCategoryNames.IndexOf(t.Label)).ToArray();
            //    currentPropEntries.GetType().GetField("entries", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPropEntries, newarray);
            //    propertyGrid1.ExpandAllGridItems();
            //    propertyGrid1.PropertySort = (PropertySort)propertyGrid1.Tag;
            //}
            //catch (Exception ex)
            //{

            //}

            GridItemCollection currentPropEntries = propertyGrid1.GetType().GetField("currentPropEntries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(propertyGrid1) as GridItemCollection;

            var categorysinfo = propertyGrid1.SelectedObject.GetType().GetField("categorys", BindingFlags.NonPublic | BindingFlags.Instance);
            if (categorysinfo != null)
            {
                var categorys = categorysinfo.GetValue(propertyGrid1.SelectedObject) as List<String>;
                propertyGrid1.CollapseAllGridItems();
                var newarray = currentPropEntries.Cast<GridItem>().OrderBy((t) => categorys.IndexOf(t.Label)).ToArray();
                currentPropEntries.GetType().GetField("entries", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPropEntries, newarray);
                propertyGrid1.ExpandAllGridItems();
                propertyGrid1.PropertySort = (PropertySort)propertyGrid1.Tag;
            }
            propertyGrid1.Paint -= new PaintEventHandler(propertyGrid1_Paint);
        }

        void propertyGrid1_Paint_bk(object sender, PaintEventArgs e)
        {
            GridItemCollection currentPropEntries = propertyGrid1.GetType().GetField("currentPropEntries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(propertyGrid1) as GridItemCollection;
            var categorysinfo = propertyGrid1.SelectedObject.GetType().GetField("categorys", BindingFlags.NonPublic | BindingFlags.Instance);
            if (categorysinfo != null)
            {
                var categorys = categorysinfo.GetValue(propertyGrid1.SelectedObject) as List<String>;
                propertyGrid1.CollapseAllGridItems();
                var newarray = currentPropEntries.Cast<GridItem>().OrderBy((t) => categorys.IndexOf(t.Label)).ToArray();
                currentPropEntries.GetType().GetField("entries", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentPropEntries, newarray);
                propertyGrid1.ExpandAllGridItems();
                propertyGrid1.PropertySort = (PropertySort)propertyGrid1.Tag;
            }
            propertyGrid1.Paint -= new PaintEventHandler(propertyGrid1_Paint);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }
    }

    [TypeConverter(typeof(PropertySorter))]
    [DefaultProperty("Name")]
    public class Person
    {
        //private List<string> categorys = new List<string>() { "B", "A", "C" };//排序
        private List<string> categorys = new List<string>() { "B", "C", "A", "其它" };//排序
        private string _name = "Bob";
        private string _name1 = "Bob1";
        private DateTime _birthday = new DateTime(1975, 1, 1);

        [DisplayName("名称A"), Category("A"), PropertyOrder(10)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [DisplayName("名称A1"), Category("A"), PropertyOrder(9)]
        public string Name1
        {
            get { return _name1; }
            set { _name1 = value; }
        }

        [DisplayName("名称B"), Category("B"), PropertyOrder(11)]
        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        [DisplayName("名称B2"), Category("B"), PropertyOrder(6)]
        public DateTime Birthday2
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        [DisplayName("名称C"), Category("C"), PropertyOrder(12)]
        public int Age
        {
            get
            {
                TimeSpan age = DateTime.Now - _birthday;
                return (int)age.TotalDays / 365;
            }
        }

        [DisplayName("XXItem"), Category("其它"), PropertyOrder(29)]
        public XX XXItem { get; set; }

        public Person()
        {
            XXItem = new XX();
        }
    }

    //[TypeConverter(typeof(PropertySorter))]
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    [TypeConverter(typeof(PropertyGridSortConverter))]
    public class XX
    {
        public XX()
        {
            Age1 = 111;
            Age2 = 222;
        }

        [DisplayName("名称C1")]
        //[PropertyOrder(2)]
        [PropertyGridOrderAttribute(2)]
        public int Age1 { get; set; }

        [DisplayName("名称C2")]
        //[PropertyOrder(1)]
        [PropertyGridOrderAttribute(11)]
        public int Age2 { get; set; }
    }

    public class PropertySorter : ExpandableObjectConverter
    {
        #region Methods
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //
            // This override returns a list of properties in order
            //
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
            ArrayList orderedProperties = new ArrayList();
            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];
                if (attribute != null)
                {
                    //
                    // If the attribute is found, then create an pair object to hold it
                    //
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    //
                    // If no order attribute is specifed then given it an order of 0
                    //
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }
            //
            // Perform the actual order using the value PropertyOrderPair classes
            // implementation of IComparable to sort
            //
            orderedProperties.Sort();
            //
            // Build a string list of the ordered names
            //
            ArrayList propertyNames = new ArrayList();
            foreach (PropertyOrderPair pop in orderedProperties)
            {
                propertyNames.Add(pop.Name);
            }
            //
            // Pass in the ordered list for the PropertyDescriptorCollection to sort by
            //
            return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
        }
        #endregion
    }

    public class PropertyOrderPair : IComparable
    {
        private int _order;
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public PropertyOrderPair(string name, int order)
        {
            _order = order;
            _name = name;
        }

        public int CompareTo(object obj)
        {
            //
            // Sort the pair objects by ordering by order value
            // Equal values get the same rank
            //
            int otherOrder = ((PropertyOrderPair)obj)._order;
            if (otherOrder == _order)
            {
                //
                // If order not specified, sort by name
                //
                string otherName = ((PropertyOrderPair)obj)._name;
                return string.Compare(_name, otherName);
            }
            else if (otherOrder > _order)
            {
                return -1;
            }
            return 1;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        //
        // Simple attribute to allow the order of a property to be specified
        //
        private int _order;
        public PropertyOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get
            {
                return _order;
            }
        }
    }
}
