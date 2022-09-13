using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientDemographicsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public int? Age { get; set; }
        public int Gender { get; set; }
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
        public int? Ethnicity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string RaceName { get; set; }
        public string StateName { get; set; }
        public string Zip { get; set; }
        public string Note { get; set; }
        public int LocationID { get; set; }
        public bool OptOut { get; set; }
        public int? Race { get; set; }
        public int? SecondaryRaceID { get; set; }
        public string PrimaryProvider { get; set; }
        public int? RenderingProviderID { get; set; }
        public string UserName { get; set; }
        public int ICDID { get; set; }
        public bool IsPortalRequired { get; set; }
        public string EmergencyContactFirstName { get; set; }
        public string EmergencyContactLastName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public int? EmergencyContactRelationship { get; set; }
        public string PhotoBase64 { get; set; }
        public string NewPassword { get; set; }
        public int StateId { get; set; }
        public string SecondaryEmail { get; set; }
        public int LinkedEncounterId { get; set; }
        public bool IsBBEnabled { get; set; }
        public string Identifier { get; set; }
        public List<PatientDiagnosisModel> PatientDiagnosis { get; set; }
        public List<PatientTagsModel> PatientTags { get; set; }
    }
}
