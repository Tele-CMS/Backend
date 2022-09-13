using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Bcpg;

namespace HC.Patient.Model.Patient
{
    public class PatientReminderModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MasterReminderMessageTypeIDs { get; set; }
        public List<PatientReminderMessageModel> MessageTypeIds { get; set; }
        public int MasterReminderFrequencyTypeID { get; set; }
        public bool IsActive { get; set; }
        public int? EnrollmentId { get; set; }
        public bool? IsSendReminderToCareManager { get; set; }
        public string CareManagerMessage { get; set; }
        public string Message { get; set; }
        public string Notes { get; set; }


        //Filters
        public string LocationIDs { get; set; } = string.Empty;
        public string SearchText { get; set; } = string.Empty;
        public string EligibilityStatus { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public string MedicalID { get; set; } = string.Empty;
        public int? StartAge { get; set; }
        public int? EndAge { get; set; }
        public string CareManagerIds { get; set; }
        public string ProgramIds { get; set; }
        public string GenderIds { get; set; }
        public string RelationshipIds { get; set; }
        public int? PrimaryConditionId { get; set; }
        public string ComorbidConditionIds { get; set; }
        public string RiskIds { get; set; }
    }

    public class PatientReminderMessageModel
    {
        public int MessageTypeID { get; set; }
        public string MessageName { get; set; }
    }

    public class TaskReminderModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MasterReminderMessageTypeIDs { get; set; }
        public List<PatientReminderMessageModel> MessageTypeIds { get; set; }
        public int MasterReminderFrequencyTypeID { get; set; }
        public bool IsActive { get; set; }
        public int? EnrollmentId { get; set; }
        public bool? IsSendReminderToCareManager { get; set; }
        public string CareManagerMessage { get; set; }
        public string Message { get; set; }
        public string Notes { get; set; }

        //Filters
        public string LocationIDs { get; set; } = string.Empty;
        public string SearchText { get; set; } = string.Empty;
        public bool? IsCompleted { get; set; }
        public bool? IsMemberCompleted { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string CareManagerIds { get; set; } //array of ids to string
        public DateTime? ApptStartDate { get; set; }
        public DateTime? ApptEndDate { get; set; }
        public DateTime? CurrentDateTime { get; set; }
        public int? TimeIntervalFilterId { get; set; }
        public bool? AllTasks { get; set; }
        //public string FilterMessage { get; set; }
        //public string Subject { get; set; }
    }

    public class AlertReminderModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MasterReminderMessageTypeIDs { get; set; }
        public List<PatientReminderMessageModel> MessageTypeIds { get; set; }
        public int MasterReminderFrequencyTypeID { get; set; }
        public bool IsActive { get; set; }
        //public int? EnrollmentId { get; set; }
        public bool? IsSendReminderToCareManager { get; set; }
        public string CareManagerMessage { get; set; }
        public string Message { get; set; }
        public string Notes { get; set; }

        //Filters
        public string LocationIDs { get; set; } = string.Empty;
        public string AlertTypeIds { get; set; }
        public string CareManagerIds { get; set; }
        public string ComorbidConditionIds { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public int? EnrollmentId { get; set; }
        public string GenderIds { get; set; }
        public int? PrimaryConditionId { get; set; }
        public string ProgramIds { get; set; }
        public string RelationshipIds { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? Dob { get; set; }
        public string EligibilityStatus { get; set; }
        public int? EndAge { get; set; }
        public string MedicalID { get; set; }
        public string riskIds { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public int? StartAge { get; set; }

    }
}
