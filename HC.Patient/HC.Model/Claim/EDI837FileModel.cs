using System;
using System.Collections.Generic;

namespace HC.Patient.Model.Claim
{
    public class EDI837FileModel
    {
        public EDIInterchangeHeaders EDIInterchangeHeaders { get; set; }
        public List<EDIClaimModel> EDIClaims { get; set; }

        public List<EDIClaimsServiceLines> EDIClaimServiceLines { get; set; }

        public List<EDIDiagnosisCodesModel> EDIDiagnosisCodes { get; set; }

        public EDILocationAddressModel EDIServiceAddress { get; set; }

        public EDILocationAddressModel EDIBillingAddress { get; set; }

        public EDIGatewayModel EDIGateway { get; set; }

        public List<EDIOtherPayerInformationModel> EDIOtherPayerInformationModel { get; set; }
        public List<EDIPayerPaymentModel> EDIPayerPaymentModel { get; set; }
    }


    public class EDIInterchangeHeaders
    {
        //ISA
        public string InterChangeDate { get; set; }
        public string InterChangeTime { get; set; }
        public string InterChangeControlerNumber { get; set; }
        public string TranSetContNumber { get; set; }
        public string CurrentDate { get; set; }
        public string BatchId { get; set; }

        //submitter
        public string SubmitterName { get; set; }
        public string SubmitterAddress { get; set; }
        public string SubmitterCity { get; set; }
        public string SubmitterState { get; set; }
        public string SubmitterZip { get; set; }
        public string SubmitterNumber { get; set; }
        public string SubmitterPhone { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonMiddleName { get; set; }
        public string ContactPersonPhoneNumber { get; set; }
        public string OrganizationFaxNumber { get; set; }
        //Submitter
    }
    public class EDIClaimModel
    {

        public int ClaimId { get; set; }
        public int EDIClaimId { get; set; }
        public int PatientId { get; set; }
        public string DOS { get; set; }
        public DateTime DateOfService { get; set; }
        public string OnSetDate { get; set; }
        public string CurrentDate { get; set; }
        public string ClaimDate { get; set; }

        //Patient Details
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientMiddleName { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientPostalCode { get; set; }
        public string PatientGender { get; set; }
        public string PatientDOB { get; set; }
        //Patient


        //subscriber
        public string SubsFirstName { get; set; }
        public string SubsLastName { get; set; }
        public string SubsIndenCode { get; set; }        
        public string SubsAddress1 { get; set; }
        public string SubsAddress2 { get; set; }
        public string SubsCity { get; set; }
        public string SubsState { get; set; }
        public string SubsPostalCode { get; set; }
        public string SubsDateTime { get; set; }
        public string SubsGender { get; set; }
        public string SubsDOB { get; set; }
        public string SubsMiddleName { get; set; }
        public string SubsEmail { get; set; }
        public bool InsurancePersonSameAsPatient { get; set; }
        public string PatientRelationWithSubscriber { get; set; }
        public string RelationshipCode { get; set; }

        public string PayerResponsibilityCode { get; set; }

        public string InsuranceCode { get; set; }
        public string InsuranceReferenceNumber { get; set; }
        public string InsuranceGroupName { get; set; }
        public string InsuredIDNumber { get; set; }

        //subscriber

        //payer
        public string PayerIden { get; set; }
        public string CarrierPayerId { get; set; }
        public string PayerTPLCode { get; set; }
        public string PayerName { get; set; }
        public string PayerGroup { get; set; }
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZipCode { get; set; }
        public int PayerPreference { get; set; }
        

        //Billing Provider  ---- These fields will be used later may be.Currently we have no-person entity as billing provider
        public string BillingProviderLastName { get; set; }
        public string BillingProviderFirstName { get; set; }
        public string BillingProviderMiddleName { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZip { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProviderTaxId { get; set; }



        //Rendering Provider
        public string RenderingProviderLastName { get; set; }
        public string RenderingProviderFirstName { get; set; }
        public string RenderingProviderMiddleName { get; set; }
        public string RenderingProviderNPI { get; set; }
        public string RenderingProviderStateLicenseNumber { get; set; }
        public string RenderingProviderUPIN { get; set; }
        public string RenderingProviderCommercialNo { get; set; }
        public string RenderingProviderLocationNumber{ get; set; }

        //Facility Location
        public string ServiceFacilityCode { get; set; }


        ///Resubmission Parmaeters
        public string PayerControlReferenceNumber { get; set; }


        //Standard Value
        public int ClaimFrequencyCode { get; set; }
    }

    public class EDIClaimsServiceLines
    {
        public int ServiceLineId { get; set; }

        public int EDIClaimServiceLineId { get; set; }
        public int ClaimId { get; set; }
        public string ServiceCode { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Units { get; set; }
        public string Modifier1 { get; set; }
        public string Modifier2 { get; set; }
        public string Modifier3 { get; set; }
        public string Modifier4 { get; set; }
        public string DXPointer1 { get; set; }
        public string DXPointer2 { get; set; }
        public string DXPointer3 { get; set; }
        public string DXPointer4 { get; set; }
        public string ServiceFacilityCode { get; set; }
    }

    public class EDIGatewayModel
    {
        public string TaxId { get; set; }
        public string ClearingHouseName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string FTPURL { get; set; }
        public string FTPUserName { get; set; }
        public string FTPPassword { get; set; }
        public string FTPPort { get; set; }
        public string Path837 { get; set; }
        public string InterchangeQualId { get; set; }
        public bool IsTest { get; set; }
    }

    public class EDILocationAddressModel
    {
        public string BillingProviderLocationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string NPINumber { get; set; }

        public string TaxId { get; set; }
    }

    public class EDIDiagnosisCodesModel
    {
        public int ClaimDXCodeId { get; set; }
        public int ClaimId { get; set; }
        public string DiagnosisCode { get; set; }
        public string CodeType { get; set; }
    }

    public class EDIOtherPayerInformationModel
    {
        public int ClaimId { get; set; }
        public string SubsFirstName { get; set; }
        public string SubsLastName { get; set; }
        public string SubsIndenCode { get; set; }
        public string SubsAddress { get; set; }
        public string SubsCity { get; set; }
        public string SubsState { get; set; }
        public string SubsPostalCode { get; set; }
        public string SubsDateTime { get; set; }
        public string SubsGender { get; set; }
        public string SubsDOB { get; set; }
        public string SubsMiddleName { get; set; }
        public string SubsEmail { get; set; }
        public bool InsurancePersonSameAsPatient { get; set; }
        public string PatientRelationWithSubscriber { get; set; }
        public string RelationshipCode { get; set; }

        public string PayerResponsibilityCode { get; set; }

        public string InsuranceCode { get; set; }
        public string InsuranceReferenceNumber { get; set; }
        public string InsuranceGroupName { get; set; }

        //subscriber

        //payer
        public string PayerIden { get; set; }
        public string CarrierPayerId { get; set; }
        public string PayerTPLCode { get; set; }
        public string PayerName { get; set; }
        public string PayerGroup { get; set; }
        public string PayerAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZipCode { get; set; }
        public int PayerPreference { get; set; }

        public int PatientInsuranceId { get; set; }
    }

    public class EDIPayerPaymentModel
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
