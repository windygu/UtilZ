using System;
using System.Collections.Generic;
using System.Text;

namespace UtilZ.ParaService.Model
{
    public class DBStatusAttribute : Attribute
    {
        public DBStatusAttribute()
        {

        }
    }

    public class ParaServiceConstant
    {
        private readonly static HashSet<int> _hsConstantValues = new HashSet<int>();

        static ParaServiceConstant()
        {
            var fileds = typeof(ParaServiceConstant).GetFields();
            Type attiType = typeof(DBStatusAttribute);
            foreach (var filed in fileds)
            {
                if (filed.GetCustomAttributes(attiType, true).Length > 0)
                {
                    _hsConstantValues.Add((int)filed.GetValue(null));
                }
            }
        }

        [DBStatusAttribute]
        public const int DB_SUCESS = 1;

        [DBStatusAttribute]
        public const int DB_FAIL_NONE = -1;

        [DBStatusAttribute]
        public const int DB_FAIL = -2;

        [DBStatusAttribute]
        public const int DB_EIXST = -3;

        [DBStatusAttribute]
        public const int DB_NOT_EIXST = -4;

        public static bool ContainsDBStatus(int status)
        {
            return _hsConstantValues.Contains(status);
        }
    }
}
