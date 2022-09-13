using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterAdministrationSite : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AdministrationSiteID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string SNOMED { get; set; }
        
        
        public string HL7 { get; set; }

        [NotMapped]        
        public string value { get { return this.HL7 + this.Description; } set { this.HL7 = value; } }
        
        public string Description { get; set; }

        [Required]        
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
                
        public virtual Organization Organization { get; set; }
    }
}
