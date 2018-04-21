using System.ComponentModel;
using UtilZ.Dotnet.DBIBase.DBModel.DBObject;

namespace TestE.DB.Model
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
