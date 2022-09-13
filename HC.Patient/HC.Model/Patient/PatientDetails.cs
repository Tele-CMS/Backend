using HC.Patient.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientInfo
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string MRN { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoThumbnailPath { get; set; }
        public bool IsPortalActivate { get; set; }
        public bool IsBlock { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string Ethnicity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string RaceName { get; set; }
        public string StateName { get; set; }
        public string Zip { get; set; }
        public string Note { get; set; }
        public bool IsPortalRequired { get; set; }
        public int RenderingProviderId { get; set; }
        public string RenderingProviderName { get; set; }
        public string RenderingProviderThumbnail { get; set; }
        public bool IsPasscodeEnable { get; set; }
        public int StateId { get; set; }
        public string PatientEmergencyContactPhone { get; set; }
    }

    public class PatientVital
    {
        public int PatientVitalID { get; set; }
        public int? HeartRate { get; set; }
        public double? BMI { get; set; }
        public int? BPDiastolic { get; set; }
        public int? BPSystolic { get; set; }
        public int? Pulse { get; set; }
        public int? Respiration { get; set; }
        public double? Temperature { get; set; }
        public double? HeightIn { get; set; }
        public double? WeightLbs { get; set; }
        public DateTime VitalDate { get; set; }
    }

    public class LastAppointmentDetails
    {
        public string LastAppointmentStaff { get; set; }
        public DateTime LastAppointment { get; set; }
        public string StaffImageUrl { get; set; }

    }

    public class UpcomingAppointmentDetails
    {
        public string UpcomingAppointmentStaff { get; set; }
        public DateTime UpcomingAppointment { get; set; }
        public string StaffImageUrl { get; set; }
    }

    public class PatientDiagnosisDetails
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime DiagnosisDate { get; set; }
    }

    public class PatientInfoDetails
    {
        public List<PatientInfo> PatientInfo { get; set; }
        public List<PatientVital> PatientVitals { get; set; }
        public List<LastAppointmentDetails> LastAppointmentDetails { get; set; }
        public List<UpcomingAppointmentDetails> UpcomingAppointmentDetails { get; set; }
        public List<PatientDiagnosisDetails> PatientDiagnosisDetails { get; set; }
        public List<PhoneNumberModel> PhoneNumberModel { get; set; }
        public List<PatientTagsModel> PatientTagsModel { get; set; }
        public List<PatientAddressesModel> PatientAddressesModel { get; set; }
        public List<PatientsAllergyModel> PatientAllergyModel { get; set; }
        public List<PatientLabTestModel> PatientLabTestModel { get; set; }
        public List<PatientMedicalFamilyHistoryModelSet> PatientMedicalFamilyHistoryModel { get; set; }
        public List<PatientCustomLabelModel> PatientCustomLabelModel { get; set; }
        public List<PatientMedicationModel> PatientMedicationModel { get; set; }

        public List<Staffs> Staffs { get; set; }
        public List<PatientSocialHistory> PatientSocialHistory { get; set; }
        public List<PatientEncounter> PatientEncounter { get; set; }
        public List<PatientImmunization> PatientImmunization { get; set; }
        public PatientEligibilityModel PatientEligibilityModel { get; set; }
        public List<HC.Patient.Entity.PatientAppointment> PatientAppointment { get; set; }

        public List<Organization> Organization { get; set; }

        public List<ClaimServiceLine> ClaimServiceLine { get; set; }
    }
    //added additional data in API
    public class PhoneNumberModel
    {
        public int PhoneNumberId { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberTypeId { get; set; }
        public string PhoneNumberType { get; set; }
        public int PreferenceID { get; set; }
        public string Preference { get; set; }
        public string OtherPhoneNumberType { get; set; }
    }
    public class PatientTagsModel
    {
        public int PatientTagID { get; set; }
        public int TagID { get; set; }
        public string Tag { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public int RoleTypeID { get; set; }
        public string FontColorCode { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PatientAddressesModel
    {
        public int AddressID { get; set; }
        public bool IsMailingSame { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string StateAbbr { get; set; }
        public int CountryID { get; set; }
        public string Phone { get; set; }
        public string CountryName { get; set; }
        public string Zip { get; set; }
        public int AddressTypeID { get; set; }
        public string AddressType { get; set; }
        public bool IsPrimary { get; set; }
        public string Others { get; set; }
        public string ApartmentNumber { get; set; }
    }
    public class PatientsAllergyModel
    {
        public int PatientAllergyId { get; set; }
        public string Allergen { get; set; }
        public int AllergyTypeId { get; set; }
        public string AllergyType { get; set; }
        public int ReactionID { get; set; }
        public string Reaction { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class PatientLabTestModel
    {
        public int TestId { get; set; }
        public string ConditionOfSpecimen { get; set; }
        public string LabName { get; set; }
        public int LonicCodeID { get; set; }
        public string LonicCode { get; set; }
        public string Notes { get; set; }
        public string TestName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string HL7Result { get; set; }
        public DateTime TestDate { get; set; }
    }
    public class PatientMedicalFamilyHistoryModelSet
    {
        public int MedicalFamilyHistoryId { get; set; }
        public string CauseOfDeath { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string FirstName { get; set; }
        public int GenderID { get; set; }
        public string Gender { get; set; }
        public string LastName { get; set; }
        public string Observation { get; set; }
        public int RelationShipID { get; set; }
        public string RelationshipName { get; set; }
        public string OtherRelationshipName { get; set; }
        public string Others { get; set; }
    }
    public class PatientCustomLabelModel
    {
        public int PatientCustomLabelID { get; set; }
        public string CustomLabelDataType { get; set; }
        public int? CustomLabelID { get; set; }
        public string CustomLabelName { get; set; }
        public string CustomLabelValue { get; set; }        
    }
    public class PatientMedicationModel
    {
        public int PatientMedicationId { get; set; }
        public string Dose { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int FrequencyID { get; set; }
        public string Frequency { get; set; }
        public string Medicine { get; set; }
        public string Strength { get; set; }
    }
    public class PatientPrescriptionModel
    {
        public int Id { get; set; }
        public string DrugName { get; set; }
        public string Strength { get; set; }
    }

    public class PatientEligibilityModel
    {
        public int PatientID { get; set; }
        public string Company { get; set; }
        public string PlanName { get; set; }
        public DateTime IssuedDate { get; set; }
        public string Status { get; set; }
    }
}
