using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientHRAModel
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? PatientDocumentId { get; set; }
        public string ProgramName { get; set; }
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
        public int LinkedEncounterId { get; set; }
        public DateTime? NextAppointmentDate { get; set; }
    }
    public class PatientHealthPlanHRAModel
    {
        public int Id { get; set; }
        public int InsuranceCompanyId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
    public class UpdateStatusModel
    {
        public int StatusId { get; set; }
        public int PatientDocumentId { get; set; }
    }
    //Email templates
    public class EmailTemplatesModel
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
    }
    public class PatientHRAReportModel
    {
        public List<PatientIndividualReportModel> PatientIndividualReportModel { get; set; }
        public PatientIndividualReportModel PatientBMISectionModel { get; set; }
        public List<BenchmarkModel> BenchmarkModel { get; set; }
        public List<MasterHRACategoryRiskReferralLinksModel> MasterHRACategoryRiskReferralLinksModel { get; set; }
        public List<VitalDetailModelForReport> VitalDetailModelForReport { get; set; }
        public List<LabDetailModelForReport> LabDetailModelForReport { get; set; }
        public PatientDetailsModelForIndReport PatientDetailsModel { get; set; }
        public List<BMIRangeModel> BMIRangeList { get; set; }
    }
    public class PatientIndividualReportModel
    {
        public int MasterHRACategoryRiskId { get; set; }
        public string Category { get; set; }
        public string CategoryDescription { get; set; }
        public string RiskDescription { get; set; }
        public string ReferralLinks { get; set; }
        public string Benchmark { get; set; }
        public decimal QScore { get; set; }
        public decimal BMIScore { get; set; }
        public string BMIBenchmark { get; set; }
        public DateTime? CompletionDate { get; set; }

    }
    public class BMIRangeModel
    {
        public int Id { get; set; }
        public string Risk { get; set; }
        public decimal MinRange { get; set; }
        public decimal MaxRange { get; set; }
    }
    public class PatientDetailsModelForIndReport
    {
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
    public class BenchmarkModel
    {
        public string Name { get; set; }
        public decimal MinRange { get; set; }
        public decimal MaxRange { get; set; }

    }
    public class MasterHRACategoryRiskReferralLinksModel
    {
        public int Id { get; set; }
        public int MasterHRACategoryRiskId { get; set; }
        public string RefLink { get; set; }

    }
    public class VitalDetailModelForReport
    {
        public string Vital { get; set; }
        public string VitalValue { get; set; }
        public DateTime LastDatePerformed { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }

    }
    public class LabDetailModelForReport
    {
        public string Test { get; set; }
        public string LabAnalyte { get; set; }
        public DateTime Date { get; set; }
        public string TestValue { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
    }
    public class PatientExecutiveReportModel
    {
        public List<PatientExecutiveReportDataModel> PatientExecutiveReportDataModel { get; set; }
        public List<PercentageByBenchmarkModel> PercentageByBenchmark { get; set; }
        public List<CategoryPercentageModel> CategoryPercentage { get; set; }
        public List<QuestionModel> Questions { get; set; }
        public List<BenchmarkModel> Benchmark { get; set; }
        public List<CategoryCodeWithPercentageModel> CategoryCodeWithPercentage { get; set; }
        public List<CategoryPercentageModel> BMICategoryPercentageModel { get; set; }
        public List<BMIBenchmarkModel> BMIBenchmarkModel { get; set; }

    }
    public class PatientExecutiveReportDataModel
    {
        public string TotalHRACompleted { get; set; }
        public int TotalHRAAssigned { get; set; }
        public decimal PercentageParticipationInHRA { get; set; }
        public string TotalEligibleMembers { get; set; }
        public string TotalFemaleParticipation { get; set; }
        public string TotalMaleParticipation { get; set; }
        public decimal TotalParticipations { get; set; }
        public decimal AverageScore { get; set; }
        public string Benchmark { get; set; }
        public string ScoreDescription { get; set; }
        public string BenchmarkDescription { get; set; }
        public string HeaderDescription { get; set; }
        public string HealthAssessmentDescription { get; set; }
        public string AggregateRiskDescription { get; set; }
        public string OrganizationName { get; set; }
        public int InfantsAgeGroup { get; set; }
        public int AdultAgeGroup { get; set; }
        public int SeniorCitizenAgeGroup { get; set; }


    }

    public class PercentageByBenchmarkModel
    {
        public decimal PercentageByBenchmark { get; set; }
        public string Benchmark { get; set; }
        public int TotalMembers { get; set; }
    }

    public class CategoryPercentageModel
    {
        public string Category { get; set; }
        public string Benchmark { get; set; }
        public decimal CategoryPercentage { get; set; }
        public string CategoryDescription { get; set; }
    }
    public class BMIBenchmarkModel
    {
        public int Id { get; set; }
        public string Risk { get; set; }
        public decimal MinRange { get; set; }
        public decimal MaxRange { get; set; }

    }
    public class QuestionModel
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public string Question { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class CategoryCodeWithPercentageModel
    {
        public int CategoryId { get; set; }
        public int SectionItemId { get; set; }
        public bool IsFavorite { get; set; }
        public string Option { get; set; }
        public int DisplayOrder { get; set; }
        public decimal IndividualCategoryCodePercentage { get; set; }
    }
    //HRA Assessment Models
    public class HRAAssessmentModel
    {
        public PatientDetailForAssessmentModel PatientDetailForAssessmentModel { get; set; }
        public List<SectionForPDFModel> SectionForPDFModel { get; set; }
        public List<SectionItemForPDFModel> SectionItemForPDFModel { get; set; }
        public List<SectionItemCodesForPDFModel> SectionItemCodesForPDFModel { get; set; }
        public List<SectionItemAnswerForPDFModel> SectionItemAnswerForPDFModel { get; set; }
    }
    public class PatientDetailForAssessmentModel
    {
        public string PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
    public class SectionForPDFModel
    {
        public int? Id { get; set; }
        public string SectionName { get; set; }
    }
    public class SectionItemForPDFModel
    {
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public int? SectionId { get; set; }
        public string InputType { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsMandatory { get; set; }
        public bool? IsNumber { get; set; }
        public string Placeholder { get; set; }
        public string Question { get; set; }
        public int? TotalRecords { get; set; }
    }
    public class SectionItemCodesForPDFModel
    {
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public string Option { get; set; }
        public int? DisplayOrder { get; set; }
    }
    public class SectionItemAnswerForPDFModel
    {
        public int? Id { get; set; }
        public int? AnswerId { get; set; }
        public int? SectionItemId { get; set; }
        public string TextAnswer { get; set; } = String.Empty;
    }
    public class MemberHRAModelForPDF
    {
        public string MemberName { get; set; }
        public string AssessmentName { get; set; }
        public string DiseaseConditions { get; set; }
        public string AssignedDate { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public string Status { get; set; }
        public string EligibilityStatus { get; set; }
        public string EligibilityStartDate { get; set; }
        public string ProgramName { get; set; }
        public string Relationship { get; set; }
        public string NextAppointmentDate { get; set; }
    }

    public class PatientAssessmentModel
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int OrganizationId { get; set; }
        public int PatientId { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentName { get; set; }
    }
    public class PatientWHOReportModel
    {
        public PatientIndividualReportModel PatientIndividualReportModel { get; set; }
        public List<MasterHRACategoryRiskReferralLinksModel> MasterHRACategoryRiskReferralLinksModel { get; set; }
        public PatientDetailsModelForIndReport PatientDetailsModel { get; set; }
    }

    public class PatientCardiovascularReportModel
    {
        public PatientIndividualReportModel PatientIndividualReportModel { get; set; }
        public List<MasterHRACategoryRiskReferralLinksModel> MasterHRACategoryRiskReferralLinksModel { get; set; }
        public PatientDetailsModelForIndReport PatientDetailsModel { get; set; }
    }
    public class PatientAsthmaReportModel
    {
        public PatientIndividualReportModel PatientIndividualReportModel { get; set; }
        public List<MasterHRACategoryRiskReferralLinksModel> MasterHRACategoryRiskReferralLinksModel { get; set; }
        public PatientDetailsModelForIndReport PatientDetailsModel { get; set; }
    }
    public class PatientCOPDReportModel
    {
        public PatientIndividualReportModel PatientIndividualReportModel { get; set; }
        public List<MasterHRACategoryRiskReferralLinksModel> MasterHRACategoryRiskReferralLinksModel { get; set; }
        public PatientDetailsModelForIndReport PatientDetailsModel { get; set; }
    }
    public class PatientDiabetesReportModel
    {
        public PatientIndividualReportModel PatientIndividualReportModel { get; set; }
        public List<MasterHRACategoryRiskReferralLinksModel> MasterHRACategoryRiskReferralLinksModel { get; set; }
        public PatientDetailsModelForIndReport PatientDetailsModel { get; set; }
    }
}
