using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientTags : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientTagID")]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        [Required]
        [ForeignKey("MasterTags")]
        public int TagID { get; set; }
        public virtual Patients Patient { get; set; }
        public virtual MasterTags MasterTags { get; set; }
    }
}
