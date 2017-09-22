using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("姓名")]
        [DBColumnAttribute("Name")]
        public string Name { get; set; }

        [DisplayName("年龄")]
        [DBColumnAttribute("Age")]
        public int Age { get; set; }

        [DisplayName("地址")]
        [DBColumnAttribute("Addr")]
        public string Addr { get; set; }
    }
}
