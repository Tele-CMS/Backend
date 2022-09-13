using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimDiagnosisCode : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public string ICDCode { get; set; }

        [ForeignKey("Claims")]
        public int ClaimId { get; set; }

        public virtual Claims Claims { get; set; }
    }
}
