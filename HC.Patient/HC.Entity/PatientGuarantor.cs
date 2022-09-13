using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientGuarantor:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string MiddleName { get; set; }
        [Column(TypeName ="varchar(100)")]
        public string LastName { get; set; }

        [ForeignKey("Patients")]
        public int PatientId { get; set; }

        public virtual Patients Patients { get; set; }
    }
}
