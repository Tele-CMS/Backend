using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterVFCEligibility :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("VFCID")]
        public int Id { get; set; }
        [StringLength(10)]
        public string ConceptCode { get; set; }
        public string ConceptName { get; set; }
        [NotMapped]
        public string value { get { return this.ConceptName; } set { this.ConceptName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}