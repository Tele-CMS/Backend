using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterImmunization : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ImmunizationID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string CvxCode { get; set; }
        [StringLength(50)]
        public string ShortDesc { get; set; }
        public string VaccineName { get; set; }
        [NotMapped]
        public string value { get { return this.VaccineName; } set { this.VaccineName = value; } }
        public string Note { get; set; }
        [StringLength(50)]
        public string VaccineStatus { get; set; }
        public int? InternalID { get; set; }
        public bool? NonVaccine { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}