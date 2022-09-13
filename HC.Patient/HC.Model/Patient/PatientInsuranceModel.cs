using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientInsuranceModel
    {
        public int Id { get; set; }
        public int PatientID { get; set; }
        public int InsuranceCompanyID { get; set; }
        public string InsuranceCompanyAddress { get; set; }
        public string InsuranceIDNumber { get; set; }
        public string InsuranceGroupName { get; set; }
        public string InsurancePlanName { get; set; }
        public int? InsurancePlanTypeID { get; set; }
        public int? InsuranceTypeID { get; set; }
        public string InsuranceClaimOfficeNumber { get; set; }
        public int? VisitsAllowedPerYear { get; set; }
        public DateTime? CardIssueDate { get; set; }
        public string Notes { get; set; }
        public string InsurancePhotoPathFront { get; set; }
        public string InsurancePhotoPathThumbFront { get; set; }
        public string Base64Front { get; set; }
        public string InsurancePhotoPathBack { get; set; }
        public string InsurancePhotoPathThumbBack { get; set; }
        public string Base64Back { get; set; }
        public bool? InsurancePersonSameAsPatient { get; set; }
        public string CarrierPayerID { get; set; }
        public bool? IsVerified { get; set; }
        public string InsuranceGroupNumber { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalRecords { get; set; }
        //
        public string InsurancePlanTypeName { get; set; }
        public string InsuranceCompanyName { get; set; }
        public InsuredPersonModel InsuredPerson { get; set; }
    }

    public class InsuredPersonModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public int PatientID { get; set; }
        public int PatientInsuranceID { get; set; }
        public int RelationshipID { get; set; }
        public string OtherRelationshipName { get; set; }
        public DateTime? Dob { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
        public string Zip { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public bool IsDeleted { get; set; }        
    }
}
