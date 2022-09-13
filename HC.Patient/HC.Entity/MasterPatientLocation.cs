using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterPatientLocation :BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientLocationID")]
        public int Id { get; set; }
        public int Code { get; set; }

        [StringLength(100)]
        public string Location { get; set; }
        [NotMapped]
        public string value { get { return this.Location; } set { this.Location = value; } }
        public string Description { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}