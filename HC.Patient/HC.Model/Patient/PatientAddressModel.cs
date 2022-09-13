using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Patient
{
    public class PatientAddressModel
    {
        public int RowNo { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string FullAddress { get; set; }
        public string Key { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }        
    }

    public class AddressModel
    {
        public int Id { get; set; }        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ApartmentNumber { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public int PatientLocationId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsMailingSame { get; set; }
        public int PatientId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsDeleted { get; set; }
        public string Others { get; set; }
    }
    public class PhoneModel
    {
        public int Id { get; set; }
        public int PatientID { get; set; }
        public int PhoneNumberTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public int PreferenceID { get; set; }
        public string OtherPhoneNumberType { get; set; }
        public bool IsDeleted { get; set; }
    } 

    public class PhoneAddressModel
    {
        public int PatientId { get; set; }
        public List<AddressModel> PatientAddress { get; set; }
        public List<PhoneModel> PhoneNumbers { get; set; }
    }
}
