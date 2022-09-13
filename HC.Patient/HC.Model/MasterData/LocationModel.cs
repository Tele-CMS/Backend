using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.MasterData
{
    public class LocationModel
    {
        public int Id { get; set; }
        public int OrganizationID { get; set; }
        public int? Code { get; set; }
        public string LocationName { get; set; }
        public DateTime? OfficeStartHour { get; set; }
        public DateTime? OfficeEndHour { get; set; }
        public int? FacilityCode { get; set; }
        public long FacilityNPINumber { get; set; }
        public long FacilityProviderNumber { get; set; }
        public long BillingTaxId { get; set; }
        public long BillingNPINumber { get; set; }
        public decimal? MileageRate { get; set; }
        public string LocationDescription { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? StateID { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public int? CountryID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ApartmentNumber { get; set; }
        public string BillingProviderInfo { get; set; }
        public decimal? StandardTime { get; set; }
        public decimal? DaylightSavingTime { get; set; }

        //
        public string FacilityType { get; set; }
        public decimal TotalRecords { get; set; }

        public decimal DaylightOffset { get; set; }
        public decimal StandardOffset { get; set; }
        public string TimeZoneName { get; set; }
    }

    public class OfficeTimeModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    } 
}
