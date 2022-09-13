using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.eHealthScore
{
   public class PatientHealtheScoreModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int AssignedBy { get; set; }
        public DateTime HealtheScoreDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string StaffName { get; set; }
        public string PatientName { get; set; }
        public int TotalRecords { get; set; }
    }
    public class PatientHealtheScoreListModel
    {
        public int PatientHealtheScoreId { get; set; }
        public int? PatientId { get; set; }
        public int? PatientDocumentId { get; set; }
        public int? DocumentId { get; set; }
        public int StatusId { get; set; }
        public string MemberName { get; set; }
        public DateTime? EligibilityStartDate { get; set; }
        public string AssessmentName { get; set; }
        public string DiseaseConditions { get; set; }
        public string Score { get; set; }
        public string Risk { get; set; }
        public string HealthPlan { get; set; }
        public string EligibilityStatus { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? EncounterDate { get; set; }
        public string ReportName { get; set; }
        public string Status { get; set; }
        public string Relationship { get; set; }
        public int TotalRecords { get; set; }
        public int DistinctPatients { get; set; }
        public int DistinctAssessments { get; set; }
        public string EncounterType { get; set; }
        public int? DistinctHealtheScore { get; set; }
        public DateTime? HealtheScoreDate { get; set; }
        public string HealtheScoreStatus { get; set; }
    }
    public class PatientHealtheScoreUpdateModel
    {
        public int Id { get; set; }
        public int UpdatedStatusId { get; set; }
    }
}
