using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterImmunityStatus : BaseEntity
    {  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ImmunityStatusID")]
        public int Id { get; set; }
        [StringLength(10)]
        public string ConceptCode { get; set; }
        public string ConceptName { get; set; }
        [NotMapped]
        public string value { get { return this.ConceptName; } set { this.ConceptName = value; } }
        [StringLength(300)]
        public string Defination { get; set; }
        [StringLength(50)]
        public string HL7Code { get; set; }
        public int? NIP004 { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
