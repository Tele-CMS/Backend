using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class InsuranceCompanies : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id  { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string value { get { return this.Name; } set { this.Name = value; } }

        [Required]
        [ForeignKey("InsuranceType")]
        public int InsuranceTypeId { get; set; }
        
        public string Address { get; set; }

        public string City { get; set; }
        
        [ForeignKey("MasterState")]
        public int StateID { get; set; }
        
        [ForeignKey("MasterCountry")]
        public int? CountryID { get; set; }
        public string Fax { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [StringLength(20)]
        public string CarrierPayerID { get; set; }
        public bool DayClubByProvider { get; set; }
        [StringLength(20)]
        public string TPLCode { get; set; }

        [StringLength(100)]
        public string InsOthers { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public bool IsEDIPayer { get; set; }
        public bool IsPractitionerIsRenderingProvider { get; set; }
        public int Form1500PrintFormat { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string AdditionalClaimInfo { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [StringLength(20)]
        public string ApartmentNumber { get; set; }

        public virtual Organization Organization { get; set; }
        
        public virtual MasterCountry MasterCountry { get; set; }
        
        public virtual MasterState MasterState { get; set; }
        
        public virtual MasterInsuranceType InsuranceType { get; set; }        
    }
}