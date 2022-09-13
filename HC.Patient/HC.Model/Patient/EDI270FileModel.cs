using HC.Patient.Model.Claim;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class EDI270FileModel
    {
        public EDI270InterchangeHeaders EDI270InterchangeHeaders { get; set; }
        public EDI270EligibilityEnquiryDetailsModel EDI270EligibilityEnquiryDetails { get; set; }
        public List<EDI270EligibilityEnquiryServiceTypesModel> EDI270EligibilityEnquiryServiceTypesList { get; set; }
        public List<EDI270EligibilityEnquiryServiceCodesModel> EDI270EligibilityEnquiryServiceCodesList { get; set; }
        public EDIGatewayModel EDIGateway { get; set; }
    }
    public class EDI270InterchangeHeaders
    {
        public string InterChangeDate { get; set; }
        public string InterChangeTime { get; set; }
        public string InterChangeControlerNumber { get; set; }
        public string TranSetContNumber { get; set; }
        public string CurrentDate { get; set; }
        public string EligibilityEnquiry270MasterId { get; set; }
    }
    public class EDI270EligibilityEnquiryDetailsModel
    {
        public string SubsFirstName { get; set; }
        public string SubsLastName { get; set; }
        public string SubsMiddleName { get; set; }
        public string SubsAddress1 { get; set; }
        public string SubsAddress2 { get; set; }
        public string SubsCity { get; set; }
        public string SubsState { get; set; }
        public string SubsPostalCode { get; set; }
        public string SubsDateTime { get; set; }
        public string SubsGender { get; set; }
        public string SubsDOB { get; set; }
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
        public string NPINumber { get; set; }
    }
    public class EDI270EligibilityEnquiryServiceTypesModel
    {
        public int EligibilityEnquiry270ServiceTypeDetailsId { get; set; }
        public int EligibilityEnquiry270MasterId { get; set; }
        public string ServiceTypeCode { get; set; }
    }
    public class EDI270EligibilityEnquiryServiceCodesModel
    {
        public int EligibilityEnquiry270ServiceCodesDetailsId { get; set; }
        public int EligibilityEnquiry270MasterId { get; set; }
        public string ServiceCode { get; set; }
    }
}
