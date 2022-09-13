using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientAlertModel
    {
        public int TotalRecords { get; set; }
        public DateTime LoadDate { get; set; }
        public string PatientName { get; set; }
        public string AlertType { get; set; }
        public string Details { get; set; }
    }
    public class PatientAlertFilterModel: FilterModel
    {
        public int PatientId { get; set; }
        public string AlertTypeIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CareManagerIds { get; set; }
        public int? EnrollmentId { get; set; }
        public string SearchText { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public DateTime? DOB { get; set; }
        public string EligibilityStatus { get; set; } = string.Empty;
        public string MedicalID { get; set; } = string.Empty;
        public string LocationIDs { get; set; } = string.Empty;
        public string RoleIds { get; set; } = string.Empty;
        public int? StartAge { get; set; }
        public int? EndAge { get; set; }
        public string RiskIds { get; set; }
        public string ProgramIds { get; set; }
        public string GenderIds { get; set; }
        public string RelationshipIds { get; set; }
        public int? PrimaryConditionId { get; set; }
        public string ComorbidConditionIds { get; set; }
        public bool? NextAppointmentPresent { get; set; }
    }
    public class PatientAlertUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Dob { get; set; }
        public bool IsActive { get; set; }
        public bool IsMobileUser { get; set; }
        public int PatientId { get; set; }
        public int UserId { get; set; }
    }
}

