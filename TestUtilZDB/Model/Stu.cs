using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilZ.Lib.DBModel.DBObject;

namespace TestUtilZDB.Model
{
    [DBTableAttribute("Stu")]
    public class Stu
    {
        [DBColumnAttribute("ID", true, DBFieldDataAccessType.R)]
        public int ID { get; set; }

        [DBColumnAttribute("Name")]
        public string Name { get; set; }

        [DBColumnAttribute("Age")]
        public int Age { get; set; }

        [DBColumnAttribute("Addr")]
        public string Addr { get; set; }
    }
}
