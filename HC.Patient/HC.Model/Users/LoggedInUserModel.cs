using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Users
{
    public class LoggedInUserModel
    {
        public int RoleId { get; set; }
        public int StaffId { get; set; }
        public string UserType{ get; set; }
    }
}
