using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Model
{
    public class FilterModel
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string sortColumn { get; set; } = string.Empty;
        public string sortOrder { get; set; } = string.Empty;
    }

    public class ListingFiltterModel : FilterModel
    {
        public string SearchKey { get; set; } = string.Empty;
        public string StartWith { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string LocationIDs { get; set; } = string.Empty;
        public string IsActive { get; set; }
        public string RoleIds { get; set; } = string.Empty;
    }
    public class PatientPayerServiceCodeFilterModel
    {
        public int PatientId { get; set; }
        public string PayerPreference { get; set; }
        public DateTime? Date { get; set; }
        public int PayerId { get; set; }
        public int PatientInsuranceId { get; set; }
    }

    public class SearchFilterModel : FilterModel
    {
        public int StaffId { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public int PayerId { get; set; } = 0;
        public int ActivityId { get; set; } = 0;
        public int AuthorizationId { get; set; } = 0;

    }
    public class PatientGuartdianFilterModel : FilterModel
    {
        public int PatientId { get; set; } = 0;
        public string SearchKey { get; set; } = string.Empty;
    }
    public class PatientFilterModel : SearchFilterModel
    {
        public int PatientId { get; set; } = 0;
    }
    public class CategoryCodesFilterModel : FilterModel
    {
        public int CategoryId { get; set; } = 0;
        public string SearchText { get; set; } = string.Empty;
    }
    public class CommonFilterModel : FilterModel
    {
        public string SearchText { get; set; } = string.Empty;
    }
    public class SectionFilterModel : FilterModel
    {
        public int DocumentId { get; set; } = 0;
    }
    public class PatientDocumentAnswerFilterModel
    {
        public int DocumentId { get; set; } = 0;
        public int PatientId { get; set; } = 0;
        public int PatientDocumentId { get; set; } = 0;
    }
    public class PatientDocumentFilterModel : FilterModel
    {
        public int PatientId { get; set; } = 0;
        public int Status { get; set; } = 0;
        public int DocumentId { get; set; } = 0;
        public string SearchText { get; set; } = string.Empty;
    }

    public class UserInvitationFilterModel : FilterModel
    {
        public string SearchText { get; set; } = string.Empty;
    }
    public class PaymentFilterModel : FilterModel
    {
        public string StaffId { get; set; } = string.Empty;
        public string PayDate { get; set; } = string.Empty;
        public string AppDate { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AppointmentType { get; set; } = string.Empty;
        public string RangeStartDate { get; set; } = string.Empty;
        public string RangeEndDate { get; set; } = string.Empty;

    }
    public class RefundFilterModel : FilterModel
    {
        public string StaffId { get; set; } = string.Empty;
        public string RefundDate { get; set; } = string.Empty;
        public string AppDate { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
    }
    public class MasterServiceFilterModel : FilterModel
    {
        public string SearchText { get; set; } = string.Empty;
    }
    public class GlobalCodeFilterModel : FilterModel
    {
        public string SearchText { get; set; } = string.Empty;
    }
    public class AppointmentDataFilterModel
    {
        public string StatusIds { get; set; }
        public int? AppointmentTimeIntervalId { get; set; }
        public string CareManagerIds { get; set; }
        public int? PatientId { get; set; }
    }

    public class EncounterFilterModel : FilterModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CareManagerIds { get; set; }
        public int? EnrollmentId { get; set; }
        public string EncounterTypeIds { get; set; }
        public int? Status { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool? NextAppointmentPresent { get; set; }
    }

    public class ProgramsFilterModel : FilterModel
    {
        public string ProgramIds { get; set; }
        public int? Status { get; set; }
        public int? EnrollmentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CareManagerIds { get; set; }
        public string ConditionIds { get; set; }
        public string IsEligible { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string SearchText { get; set; }
        public string Relationships { get; set; }
        public string GenderIds { get; set; }
        public int? StartAgeRange { get; set; }
        public int? EndAgeRange { get; set; }
        public bool? NextAppointmentPresent { get; set; }

        public class AlertsRedIndicatorFilterModel : FilterModel
        {
            public int PatientId { get; set; }
            public int AlertsDayDifference { get; set; }
            public string AlertTypeIds { get; set; }
            public string SearchText { get; set; }
            public string CareManagerIds { get; set; }
            public int? EnrollmentId { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
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
        public class AlertRepsoneModel
        {
            public int TotalRecords { get; set; }
            public DateTime LoadDate { get; set; }
            public string PatientName { get; set; }
            public string AlertType { get; set; }
            public string Details { get; set; }
            public int? PatientId { get; set; }
            public DateTime DOB { get; set; }
            public string MedicalId { get; set; }
            public string EligibilityStatus { get; set; }
            public string Risk { get; set; }
            public int? Age { get; set; }
            public string CareTeamNames { get; set; }
            public string ProgramNames { get; set; }
            public string Gender { get; set; }
            public string Relationship { get; set; }
            public string PrimaryCondition { get; set; }
            public string ComorbidConditions { get; set; }
            public DateTime? NextAppointmentDate { get; set; }
        }

        public class FilterModelForMemberHRA : FilterModel
        {
            public int? HealthPlanId { get; set; }
            public string ProgramTypeId { get; set; }
            public string ConditionId { get; set; }
            public int? DocumentId { get; set; }
            public DateTime? CompletionEndDate { get; set; }
            public DateTime? ExpirationStartDate { get; set; }
            public DateTime? CompletionStartDate { get; set; }
            public DateTime? ExpirationEndDate { get; set; }
            public DateTime? AssignedStartDate { get; set; }
            public DateTime? AssignedEndDate { get; set; }
            public DateTime? TerminationStartDate { get; set; }
            public DateTime? TerminationEndDate { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public DateTime? AssignedDate { get; set; }
            public string IsEligible { get; set; }
            public string SearchText { get; set; }
            public int StatusForUpdate { get; set; }
            public string Relationship { get; set; }
            public int? StatusId { get; set; }
            public int? EnrollmentId { get; set; }
            public int? AssessmentId { get; set; }
            public DateTime? EligibilityStartDate { get; set; }
            public DateTime? EligibilityEndDate { get; set; }
            public string Message { get; set; }
            public string Subject { get; set; }
            public string CareManagerIds { get; set; } = String.Empty;
            public bool? NextAppointmentPresent { get; set; }
        }


        public class FilterModelForHealtheScore : FilterModel
        {
            public int? HealthPlanId { get; set; }
            public int? EncounterTypeId { get; set; }
            public int? UpdatedStatusId { get; set; }
            public string ConditionId { get; set; }
            public int? DocumentId { get; set; }
            public DateTime? CompletionEndDate { get; set; }
            public DateTime? ExpirationStartDate { get; set; }
            public DateTime? CompletionStartDate { get; set; }
            public DateTime? ExpirationEndDate { get; set; }
            public DateTime? AssignedStartDate { get; set; }
            public DateTime? AssignedEndDate { get; set; }
            public DateTime? TerminationStartDate { get; set; }
            public DateTime? TerminationEndDate { get; set; }
            public DateTime? EncounterStartDate { get; set; }
            public DateTime? EncounterEndDate { get; set; }
            public DateTime? HealtheScoreDate { get; set; }
            public DateTime? AssignedDate { get; set; }
            public bool? IsEligible { get; set; }
            public string SearchText { get; set; }
            public int StatusForUpdate { get; set; }
            public string Relationship { get; set; }
            public int? StatusId { get; set; }
            public int? AssessmentId { get; set; }
            public DateTime? EligibilityStartDate { get; set; }
            public DateTime? EligibilityEndDate { get; set; }
            public string Message { get; set; }
            public string Subject { get; set; }
            public string ModuleType { get; set; }
            public int? HealthEScoreStatusFilterId { get; set; }
            public DateTime? HealthEScoreFilterStartDate { get; set; }
            public DateTime? HealthEScoreFilterEndDate { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public class MedicationDataFilterModel : FilterModel
        {
            public int PatientId { get; set; }
            public string NDCCode { get; set; }
            public string MedSource { get; set; }
            public int RecordId { get; set; }
            public int ClaimId { get; set; }
        }

        public class LabFilterModel : FilterModel
        {
            public int patientId { get; set; }
            public string LoincCode { get; set; }
            public string MedSource { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
        }

        public class HRALogFilterModel : FilterModel
        {
            public int ProviderId { get; set; }
            public int ReportTypeId { get; set; }
            public string SearchText { get; set; }
        }

        }
}
