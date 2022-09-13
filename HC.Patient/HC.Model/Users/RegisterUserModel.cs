using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class RegisterUserModel
    {
        public string InvitationId { get; set; }
        public string Username { get; set; }
        public int OrganizationId { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int LocationId { get; set; }
        public int StaffId { get; set; } = 0;
        public int Gender { get; set; } = 0;
        public string WebUrl { get; set; }
    }
}
