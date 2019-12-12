using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.WindowEx.WPF.Controls.Chart
{
    public class AxesCollection : IEnumerable<ObservableCollection<AxisAbs>>
    {
        private ObservableCollection<AxisAbs> _axisYLeftList = null;
        public ObservableCollection<AxisAbs> AxisYLeftList
        {
            get { return _axisYLeftList; }
        }

        private ObservableCollection<AxisAbs> _axisYRightList = null;
        public ObservableCollection<AxisAbs> AxisYRightList
        {
            get { return _axisYRightList; }
        }

        private ObservableCollection<AxisAbs> _axisXTopList = null;
        public ObservableCollection<AxisAbs> AxisXTopList
        {
            get { return _axisXTopList; }
        }

        private ObservableCollection<AxisAbs> _axisXBottomList = null;
        public ObservableCollection<AxisAbs> AxisXBottomList
        {
            get { return _axisXBottomList; }
        }


        public AxesCollection()
        {

        }

        public IEnumerator<ObservableCollection<AxisAbs>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        internal void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
