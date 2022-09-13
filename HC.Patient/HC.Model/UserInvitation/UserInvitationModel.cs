using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class UserInvitationModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int LocationId { get; set; }
        public int OrganizationId { get; set; }
        public bool IsSendEmail { get; set; }
        public int RoleId { get; set; }
        public string WebUrl { get; set; }
    }
}
