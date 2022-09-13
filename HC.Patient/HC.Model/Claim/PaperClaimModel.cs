using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Claim
{
    public class PaperClaimModel
    {
        public List<ClaimDetailsModel> ClaimDetails { get; set; }
        public List<InsuredModel> InsuredDetails { get; set; }
        public List<InsuredModel> OtherInsuredDetails { get; set; }
        public List<ServiceCodesModel> ServiceCodes { get; set; }
        public List<DiagnosisCodesModel> DiagnosisCodes { get; set; }
        public OrganizationDetailsModel OrganizationDetails { get; set; }
        public LocationAddressModel ServiceLocation { get; set; }
        public LocationAddressModel BillingLocation { get; set; }

        public List<PayerPaymentModel> PayerPayments { get; set; }
    }

    public class ClaimDetailsModel
    {
        public string ClaimIds { get; set; }
        public int ClaimId { get; set; }
        public DateTime DOS { get; set; }
        public int PatientId { get; set; }
        public string SubsLastName { get; set; }
        public string SubsFirstName { get; set; }
        public string SubsMiddleName { get; set; }
        public string SubsGender { get; set; }
        public string SubsAddress1 { get; set; }
        public string SubsAddress2 { get; set; }
        public string SubsApartmentNumber { get; set; }
        public string SubsCity { get; set; }
        public string SubsState { get; set; }
        public string SubsPostalCode { get; set; }
        public DateTime SubsDOB { get; set; }
        public string SubsPhoneMobile { get; set; }
        public string SubsPhoneHome { get; set; }
        public string SubsAccountNumber { get; set; }
        public int SubmitterId { get; set; }
        public string SubmitterLastName { get; set; }
        public string SubmitterFirstName { get; set; }
        public string NPINumber { get; set; }
        public string TaxId { get; set; }
        public int PayerIden { get; set; }
        public string PayerName { get; set; }

        public string PayerAddress { get; set; }
        public string PayerApartmentNumber { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerPostalCode { get; set; }

        public string ServiceFacilityCode { get; set; }
        public string InsurancePlanType { get; set; }
        public string InsuredIDNumber { get; set; }
        public string SubsEmail { get; set; }
        public string AdditionalClaimInfo { get; set; }
        public bool InsurancePersonSameAsPatient { get; set; }
        public DateTime ClientRecordInsertionDate { get; set; }
        public DateTime AppointmentStartTime { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public bool IncludeServiceTimeWithServiceCode { get; set; }
    }
    public class InsuredModel
    {
        public string ClaimIds { get; set; }
        public int PatientId { get; set; }
        public int ClaimId { get; set; }
        public string InsuredLastName { get; set; }
        public string InsuredFirstName { get; set; }
        public string InsuredMiddleName { get; set; }
        public string InsuredGender { get; set; }
        public DateTime InsuredDOB { get; set; }
        public string InsuredAddress1 { get; set; }
        public string InsuredApartmentNumber { get; set; }
        public string InsuredAddress2 { get; set; }
        public string InsuredCity { get; set; }
        public string InsuredState { get; set; }
        public string InsuredCountry { get; set; }
        public string InsuredZip { get; set; }
        public string InsuredPhone { get; set; }
        public string InsurancePlanName { get; set; }
        public string InsuredIDNumber { get; set; }
        public string InsuranceGroupName { get; set; }
        public string InsuredRelation { get; set; }
        public string AuthorizationNumber { get; set; }
        public bool InsurancePersonSameAsPatient { get; set; }        
    }
    public class ServiceCodesModel
    {
        public int ClaimId { get; set; }
        public DateTime DOS { get; set; }

        public int ServiceLineId { get; set; }

        public string ServiceCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }

        public string DiagnosisCodePointer1 { get; set; }
        public string DiagnosisCodePointer2 { get; set; }
        public string DiagnosisCodePointer3 { get; set; }
        public string DiagnosisCodePointer4 { get; set; }
        public string ServiceFacilityCode { get; set; }
        public string NPINumber { get; set; }
        public string TaxId { get; set; }
        public string AuthorizationNumber { get; set; }

        public DateTime AppointmentStartTime { get; set; }
        public DateTime AppointmentEndTime { get; set; }
    }
    public class DiagnosisCodesModel
    {
        public int ClaimDXCodeId { get; set; }
        public int ClaimId { get; set; }
        public string DiagnosisCode { get; set; }
    }
    public class OrganizationDetailsModel
    {
        public string OrganizationName { get; set; }
        public string Address1 { get; set; }
        public string ApartmentNumber { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
    public class LocationAddressModel
    {   
        public string ServiceFacilityLocationName { get; set; }
        public string BillingProviderInfomation { get; set; }
        public string Address1 { get; set; }
        public string ApartmentNumber { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string NPINumber { get; set; }
        public string TaxId { get; set; }
    }

    public class PayerPaymentModel
    {
        public int ClaimId { get; set; }
        public int ClaimServiceLineId { get; set; }
        public string AdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public decimal Amount { get; set; }
        public int PatientInsuranceId { get; set; }
        public string DescriptionType { get; set; }
    }
}
