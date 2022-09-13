using System;
using System.Collections.Generic;

namespace HC.Patient.Model.Patient
{
    public class PatientHeaderModel
    {
        public PatientBasicHeaderInfoModel PatientBasicHeaderInfo { get; set; }
        public List<PatientTagsHeaderInfoModel> PatientTagsHeaderInfo { get; set; }
        public List<PatientPhoneHeaderInfoModel> PatientPhoneHeaderInfo { get; set; }
        public List<PatientAllergyHeaderInfoModel> PatientAllergyHeaderInfo { get; set; }        
    }
    public class PatientBasicHeaderInfoModel
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string MRN { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoThumbnailPath { get; set; }
        public string PrimaryProvider { get; set; }
        public int UserId { get; set; }
    }
    public class PatientTagsHeaderInfoModel
    {
        public string Tag { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string FontColorCode { get; set; }
    }
    public class PatientPhoneHeaderInfoModel
    {
        public string PhoneNumber { get; set; }
        public string PhoneNumberType { get; set; }
        public string Preference { get; set; }
    }
    public class PatientAllergyHeaderInfoModel
    {
        public string Allergen { get; set; }
        public string AllergyType { get; set; }
        public string Reaction { get; set; }
        public string Note { get; set; }
    }
}
