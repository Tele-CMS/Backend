using HC.Patient.Entity;
using HC.Patient.Model.Claim;
using HC.Patient.Model.Common;
using System.Collections.Generic;

namespace HC.Patient.Model.MasterData
{
    public class MasterDataModel //: Identifiable<int>
    {
        public List<MasterCountry> MasterCountry { get; set; }
        public List<MasterEthnicity> MasterEthnicity { get; set; }
        public List<GlobalCode> MasterPhonePreferences { get; set; }
        //public List<MasterOccupation> MasterOccupation { get; set; }
        //public List<MasterPreferredLanguage> MasterPreferredLanguage { get; set; }
        public List<MasterRace> MasterRace { get; set; }
        public List<MasterState> MasterState { get; set; }
        public List<GlobalCode> MasterMaritalStatus { get; set; }
        public List<GlobalCode> MasterPatientStatus { get; set; }
        public List<GlobalCode> MasterSuffix { get; set; }
        public List<MasterRelationship> MasterRelationship { get; set; }
        // public List<Provider> Provider { get; set; }
        public List<MasterProgram> MasterProgram { get; set; }
        public List<InsuranceCompanies> InsuranceCompanies { get; set; }
        //public List<MasterPatientCommPreferences> MasterPatientCommPreferences { get; set; }
        public List<MasterReferral> MasterReferral { get; set; }
        public List<MasterGender> MasterGender { get; set; }
        public List<GlobalCode> MasterPhoneType { get; set; }
        public List<GlobalCode> InsurancePlanType { get; set; }




        public List<MasterServiceCode> MasterServiceCode { get; set; }

        public List<MasterICD> MasterICD { get; set; }


        //Following are the Immunization tables
        public List<MasterVFCEligibility> MasterVFC { get; set; }
        public List<MasterImmunization> MasterImmunization { get; set; }
        public List<MasterManufacture> MasterManufacture { get; set; }
        public List<MasterAdministrationSite> MasterAdministrationSite { get; set; }
        public List<MasterRouteOfAdministration> MasterRouteOfAdministration { get; set; }
        public List<MasterImmunityStatus> MasterImmunityStatus { get; set; }
        public List<MasterRejectionReason> MasterRejectionReason { get; set; }
        public List<GlobalCode> MasterSocialHistory { get; set; }
        public List<GlobalCode> MasterTravelHistory { get; set; }

        public List<GlobalCode> MasterAddressType { get; set; }

        public List<Staffs> Staffs { get; set; }
        public List<Staffs> MasterRenderingProvider { get; set; }
        public List<Staffs> AllStaffs { get; set; }
        public List<AppointmentType> AppointmentType { get; set; }
        public List<MasterDropDown> AppointmentStatus { get; set; }
        public List<MasterDropDown> AppointmentTimeGraphFilter { get; set; }

        // LAB
        public List<GlobalCode> MasterLabTestType { get; set; }
        public List<MasterLonic> MasterLonic { get; set; }
        public List<MasterLabs> MasterLabs { get; set; }
        public List<GlobalCode> MasterTimeType { get; set; }
        public List<GlobalCode> MasterFrequencyType { get; set; }
        public List<GlobalCode> MasterFrequencyDurationType { get; set; }
        public List<MasterAllergies> MasterAllergies { get; set; }
        public List<MasterReaction> MasterReaction { get; set; }

        public List<GlobalCode> MasterAuthorizedProcedure { get; set; }

        public List<GlobalCode> MasterCustomLabelType { get; set; }

        public List<MasterCustomLabels> MasterCustomLabels { get; set; }
        public List<MasterPatientLocation> MasterPatientLocation { get; set; }
        public List<MasterTags> MasterTags { get; set; }
        public List<MasterCustomLabels> MasterCustomLabelStaff { get; set; }
        public List<UserRoles> MasterRoles { get; set; }
        public List<MasterWeekDays> MasterWeekDays { get; set; }

        public List<MasterUnitType> MasterUnitType { get; set; }
        public List<MasterRoundingRules> MasterRoundingRules { get; set; }

        public List<Location> MasterLocation { get; set; }
        public List<MasterTemplates> MasterTemplates { get; set; }
        public List<OrganizationDatabaseDetail> OrganizationDatabaseDetail { get; set; }
        public List<MasterOrganization> MasterOrganization { get; set; }

        //public List<Organization> MasterOrganization { get; set; }
        public List<UserRoleType> UserRoleType { get; set; }
        public List<MasterInsuranceType> MasterInsuranceType { get; set; }
        public List<GlobalCode> StaffAvailability { get; set; }
        public List<GlobalCode> EncounterStatus { get; set; }
        public List<GlobalCode> MasterCancelType { get; set; }
        public List<MasterPaymentType> MasterPaymentType { get; set; }
        public List<MasterPaymentDescription> MasterPaymentDescription { get; set; }
        public List<GlobalCode> EmployeeType { get; set; }
        public List<GlobalCode> ReferralSource { get; set; }
        public List<GlobalCode> Employeement { get; set; }
        public List<MasterDocumentTypes> MasterDocumentTypes { get; set; }
        public List<MasterDocumentTypesStaff> MasterDocumentTypesStaff { get; set; }
        public List<MasterDegree> MasterDegree { get; set; }
        public List<ClaimResubmissionReasonModel> ClaimResubmissionReason { get; set; }
        public List<AdjustmentGroupCodeModel> AdjustmentGroupCodeModel { get; set; }
        public List<GlobalCode> LeaveType { get; set; }
        public List<GlobalCode> LeaveReason { get; set; }
        public List<GlobalCode> LeaveStatus { get; set; }
        public List<GlobalCode> ClaimPaymentStatus { get; set; }
        public List<GlobalCode> ClaimPaymentStatusForLedger { get; set; }
        public List<MasterTags> MasterTagsforStaff { get; set; }
        public List<MasterTags> MasterTagsforPatient { get; set; }
        public List<GlobalCode> TimesheetStatus { get; set; }
        public List<GlobalCode> PayPeriod { get; set; }
        public List<GlobalCode> WorkWeek { get; set; }
        public List<MasterDropDown> PayrollGroup { get; set; }
        public List<MasterDropDown> PayrollBreakTime { get; set; }
        public List<MasterDropDown> Categories { get; set; }
        public List<MasterDropDown> Documents { get; set; }
        public List<MasterDropDown> DocumentStatus { get; set; }

        public List<MasterDropDown> MasterSpeciality { get; set; }
        public List<MasterDropDown> MASTERPROVIDERCARECATEGORY { get; set; }
        public List<MasterDropDown> MASTERPROVIDERSQUESTIONAREQUESTIONS { get; set; }
        public List<MasterDropDown> MasterTaxonomy { get; set; }
        public List<MasterDropDown> MasterStaffServices { get; set; }
        public List<MasterDropDown> MasterTemplateTypes { get; set; }
        public List<MasterDropDown> MasterGlobalCategories { get; set; }

        public List<PrescriptionDrugs> MasterPrescriptionDrugs { get; set; }
        public List<MasterCity> MasterCity { get; set; }
        public List<MasterPharmacy> MasterPharmacy { get; set; }
        public List<MasterQuestionnaireTypes> MasterQuestionnaireTypes { get; set; }
        public List<MasterDropDown> MasterHRACategoryRisk { get; set; }
        public List<MasterDropDown> GenderCriteriaForHRA { get; set; }
        public List<MasterBenchmark> MasterBenchmark { get; set; }
        public List<MasterDropDown> EncounterTypes { get; set; }
        public List<MasterDropDown> EncounterMethods { get; set; }
        public List<MasterDropDown> MasterDocumentStatus { get; set; }
        public List<MasterDropDown> PatientProgramFrequency { get; set; }
        public List<MasterDropDown> PatientProgramStatus { get; set; }
        public List<MasterDropDown> MasterLoadAlerts { get; set; }
        public List<MasterDropDown> MasterEnrollmentTypeFilter { get; set; }
        public List<MasterDropDown> MemberRelationship { get; set; }
        public List<MasterDropDown> MasterRiskIndicator { get; set; }
        public List<MasterDropDown> MasterDiseaseConditionMappedWithDMP { get; set; }
        public List<MasterDropDown> MasterChronicCondition { get; set; }
    }
    public class MasterCustomLabelData
    {
        public MasterCustomLabels MasterCustomLabels { get; set; }

        //public MasterType MasterCustomLabelType { get; set; }
    }

    public class AutoCompleteSearchModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class MasterChronicConditionListModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

}
