using System;

namespace UtilZ.RouteService.DBModel
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public int RoleID { get; set; }

        public UserInfo()
        {

        }
    }
}
