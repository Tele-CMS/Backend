using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Staff
{
    public class StaffModel
    {

        public int StaffID { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }
        public string PhotoThumbnailPath { get; set; }
    }
    public class StaffDropDownModel
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
    }

    public class StaffModels : StaffModel
    {
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        //inherit
        //public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int OrganizationID { get; set; }
        public string PhotoPath { get; set; }
        //inherit
        //public string PhotoThumbnailPath { get; set; }
        public string Title { get; set; }
        public int UserID { get; set; }
        public DateTime DOJ { get; set; }
        public string NPINumber { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        //inherit
        //public string RoleName { get; set; }
        public string TaxId { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
        public bool IsBlock { get; set; }
        public decimal TotalRecords { get; set; }
    }

    public class UserLocationsModel
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int OrganizationId { get; set; }
    }
    public class StaffProfileSetupModel
    {
        public int IsProfileSetup { get; set; }
        public int BasicProfile { get; set; }
        public int StaffAvailability { get; set; }
        public int StaffExperiences { get; set; }
        public int StaffQualifications { get; set; }
        public int StaffAwards { get; set; }
        public int StaffTaxonomies { get; set; }
        public int StaffLocation { get; set; }
        public int StaffServices { get; set; }
        public int StaffSpecialities { get; set; }

    }
    public class StaffByNameModel
    {
        public int StaffID { get; set; }
        public string Email { get; set; }
        public string PhotoThumbnailPath { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }

    }
}

