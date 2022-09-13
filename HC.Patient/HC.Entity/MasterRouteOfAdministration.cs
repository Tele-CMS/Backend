using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterRouteOfAdministration : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RouteOfAdministrationID")]
        public int Id { get; set; }
        [StringLength(10)]
        public string FDANCI { get; set; }
        [StringLength(10)]
        public string HL7 { get; set; }
        [NotMapped]
        public string value { get { return this.HL7 + this.Description; } set { this.HL7 = value; } }
        [StringLength(100)]
        public string Description { get; set; }
        [StringLength(500)]
        public string Definition { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        
        public virtual Organization Organization { get; set; }
    }
}
