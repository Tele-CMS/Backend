using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Users
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string UserType { get; set; }
        public string value { get { return this.RoleName; } }
        public bool IsActive { get; set; }
        public decimal TotalRecords { get; set; }
    }
}
