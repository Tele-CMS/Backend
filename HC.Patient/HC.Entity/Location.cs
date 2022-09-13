using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Location : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("LocationID")]
        public int Id { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public int? Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string LocationName { get; set; }
        public DateTime? OfficeStartHour { get; set; }
        public DateTime? OfficeEndHour { get; set; }
        [ForeignKey("MasterPatientLocations")]
        public int? FacilityCode { get; set; }
        [NotMapped]
        public string FacilityCodeName
        {
            get
            {
                try
                {
                    return MasterPatientLocations!= null?MasterPatientLocations.Location: null;
                }
                catch (Exception)
                {
                    return "";                    
                }
            }
        }

        public long FacilityNPINumber { get; set; }
        public long FacilityProviderNumber { get; set; }
        public long BillingTaxId { get; set; }
        public long BillingNPINumber { get; set; }
        public decimal? MileageRate { get; set; }
        [MaxLength(1000)]
        public string LocationDescription { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [ForeignKey("MasterState")]
        public int? StateID { get; set; }
        [NotMapped]
        public string StateName
        {
            get
            {
                try
                {
                    return MasterState !=null ?MasterState.StateName:null;
                }
                catch (Exception)
                {
                    return "";
                    //throw;
                }
            }
        }
        [StringLength(20)]
        public string Zip { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [ForeignKey("MasterCountry")]
        public int? CountryID { get; set; }
        [NotMapped]
        public string CountryName
        {
            get
            {
                try
                {
                    return MasterCountry != null? MasterCountry.CountryName: null;
                }
                catch (Exception)
                {
                    return "";
                    //throw;
                }
            }
        }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [StringLength(20)]
        public string ApartmentNumber { get; set; }
        [MaxLength(1000)]
        public string BillingProviderInfo { get; set; }
        public decimal? StandardTime { get; set; }
        public decimal? DaylightSavingTime { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }
        public virtual MasterState MasterState { get; set; }
        public virtual MasterPatientLocation MasterPatientLocations { get; set; }
        [NotMapped]        
        public string OfficeEndHourStr { get; set; }
        [NotMapped]        
        public string OfficeStartHourStr { get; set; }
    }
}
