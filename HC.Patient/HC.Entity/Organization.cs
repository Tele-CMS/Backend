using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Organization
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OrganizationID")]
        public int Id { get; set; }
        [MaxLength(60)]
        public string OrganizationName { get; set; }
        [MaxLength(100)]
        public string BusinessName { get; set; }

        [NotMapped]
        public string value { get { return this.OrganizationName; } set { this.OrganizationName = value; } }

        [MaxLength(1000)]
        public string Description { get; set; }

        [StringLength(1000)]
        public string Address1 { get; set; }

        [StringLength(1000)]
        public string Address2 { get; set; }

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
                    if(MasterState != null)
                    {
                        if (!string.IsNullOrEmpty(MasterState.StateName))
                        {
                            return MasterState.StateName;
                        }
                        return string.Empty;

                    } else
                    {
                        return null;
                    }
                   
                   
                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
            set { }
        }

        [StringLength(20)]
        public string Zip { get; set; }
        
        [StringLength(20)]
        public string Phone { get; set; }

        
        
        [ForeignKey("MasterCountry")]
        public int? CountryID { get; set; }

        
        [StringLength(40)]
        public string Fax { get; set; }

        
        [StringLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        
        public string Logo { get; set; }

        
        public string Favicon { get; set; }

        
        [StringLength(50)]
        public string ContactPersonFirstName { get; set; }

        
        [StringLength(50)]
        public string ContactPersonMiddleName { get; set; }

        
        [StringLength(50)]
        public string ContactPersonLastName { get; set; }

        
        [StringLength(15)]
        public string ContactPersonPhoneNumber { get; set; }

        
        [ForeignKey("MasterMaritalStatus")]
        public int? ContactPersonMaritalStatus { get; set; }

        [NotMapped]        
        public string UserName { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public int DatabaseDetailId { get; set; }

        
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [StringLength(20)]
        public string ApartmentNumber { get; set; }

        [StringLength(50)]
        public string VendorIdDirect { get; set; }

        [StringLength(50)]
        public string VendorIdIndirect { get; set; }

        [StringLength(250)]
        public string VendorNameDirect { get; set; }

        [StringLength(250)]
        public string VendorNameIndirect { get; set; }

        //relationship tables        
        public virtual MasterCountry MasterCountry { get; set; }
        public virtual MasterState MasterState { get; set; }
        public virtual GlobalCode MasterMaritalStatus { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        [Column(TypeName ="varchar(15)")]
        public string PayrollStartWeekDay { get; set; }
        [Column(TypeName = "varchar(15)")]
        public string PayrollEndWeekDay { get; set; }     
        
        public decimal? BookingCommision { get; set; }
        public string StripeKey { get; set; }
        public string PaymentMode { get; set; }
        public string StripeSecretKey { get; set; }
        public bool IsPersonalAccount { get; set; }
        public string ContactPersonEmail { get; set; }

    }
}
