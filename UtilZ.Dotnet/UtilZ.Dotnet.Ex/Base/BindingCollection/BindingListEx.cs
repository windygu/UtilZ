using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UtilZ.Dotnet.Ex.Base
{
    [Serializable]
    public class BindingListEx<T> : BindingList<T>
    {
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (prop.PropertyType.GetInterface("IComparable") == null)
            {
                throw new Exception($"成员:{prop.Name}没有实现IComparable接口,该字段列不支持排序");
            }
            int count = base.Count;
            ComparerResultType equal = ComparerResultType.Equal;
            T local = default(T);
            object obj2 = null;
            object obj3 = null;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    obj2 = prop.GetValue(base.Items[i]);
                    obj3 = prop.GetValue(base.Items[j]);
                    if ((obj2 == null) && (obj3 == null))
                    {
                        equal = ComparerResultType.Equal;
                    }
                    else if ((obj2 == null))
                    {
                        equal = ComparerResultType.Less;
                    }
                    else if ((obj2 != null) && (obj3 == null))
                    {
                        equal = ComparerResultType.Greater;
                    }
                    else
                    {
                        num2 = ((IComparable)obj2).CompareTo((IComparable)obj3);
                        switch (num2)
                        {
                            case 1:
                                equal = ComparerResultType.Greater;
                                goto Label_013E;

                            case 0:
                                equal = ComparerResultType.Equal;
                                goto Label_013E;

                            case -1:
                                equal = ComparerResultType.Less;
                                break;
                        }
                        if (num2 != -1)
                        {
                            throw new Exception($"奇怪的比较结果:{num2}");
                        }
                        equal = ComparerResultType.Less;
                    }
                Label_013E:
                    if (equal != ComparerResultType.Equal)
                    {
                        if (direction == ListSortDirection.Ascending)
                        {
                            if (equal == ComparerResultType.Greater)
                            {
                                local = base.Items[i];
                                base.Items[i] = base.Items[j];
                                base.Items[j] = local;
                            }
                        }
                        else if (equal == ComparerResultType.Less)
                        {
                            local = base.Items[i];
                            base.Items[i] = base.Items[j];
                            base.Items[j] = local;
                        }
                    }
                }
            }
        }

        protected override bool SupportsSortingCore =>
            true;
    }
}
