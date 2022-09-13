using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class UserInvitationResponseModel
    {
        public int InvitationId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime InvitationSendDate { get; set; }
        public int TotalRecords { get; set; }
        public string InvitationStatus { get; set; }
        public int RoleId { get; set; }
    }
    public class UserInvitationRegistrationModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int InvitationStatus { get; set; }
    }
}
