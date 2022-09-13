using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
    public class StaffProfileModel
    {
        public int StaffId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoThumbnailPath { get; set; }
        public string NPINumber { get; set; }
        public string TaxId { get; set; }        
        public DateTime DOJ { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffFullAddress { get; set; }
        public string Gender { get; set; }
        public int UserId { get; set; }
        public string DegreeName { get; set; }
        public List<StaffProfileTagsModel> StaffTags { get; set; }
        public List<StaffProfileLocationModel> StaffLocations { get; set; }
        public List<StaffProfileSpecialityModel> StaffSpecialities { get; set; }
        public List<StaffProfileTaxonomyModel> StaffTaxonomies { get; set; }
        public List<StaffProfileServiceModel> StaffServices { get; set; }

    }
    public class StaffProfileTagsModel
    {
        public string Tag { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string FontColorCode { get; set; }
    }
    public class StaffProfileLocationModel
    {
        public bool IsDefault { get; set; }
        public string LocationDescription { get; set; }
        public string LocationName { get; set; }
        public string StateName { get; set; }
        public string LocationFullAddress { get; set; }
    }
    public class StaffHeaderDataModel
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    public class StaffProfileSpecialityModel
    {
        public string Speciality { get; set; }
        public string SpecialityValue { get; set; }
    }
    public class StaffProfileTaxonomyModel
    {
        public string Taxonomy { get; set; }
        public string TaxonomyValue { get; set; }
    }
    public class StaffProfileServiceModel
    {
        public string ServiceName { get; set; }
        public string Id { get; set; }
    }
}
