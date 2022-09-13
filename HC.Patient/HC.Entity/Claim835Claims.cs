using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claim835Claims : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Claim835Batch")]
        public int Claim835BatchId { get; set; }

        [ForeignKey("Claim837Claims")]
        public int Claim837ClaimId { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string ClaimStatusCode { get; set; } 
        public decimal AmountClaimed { get; set; } 
        public decimal AmountApproved { get; set; } 
        public decimal PatientResponsibilityAmount { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string ClaimFillingIndicatorCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PayerClaimControlNumber { get; set; }

        public bool Applied { get; set; }
        public virtual Claim837Claims Claim837Claims { get; set; }
        public virtual Claim835Batch Claim835Batch { get; set; }

    }
}
