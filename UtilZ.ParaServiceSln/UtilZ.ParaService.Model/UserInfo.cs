using System;

namespace UtilZ.ParaService.Model
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
