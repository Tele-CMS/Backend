using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claim835ServiceLine : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Claim835Claims")]
        public int Claim835ClaimId { get; set; }

        [ForeignKey("Claim837ServiceLine")]
        public int Claim837ServiceLineId { get; set; }
        public decimal AmountCharged { get; set; } //SVC02
        public decimal AmountApproved { get; set; } //SVC03
        public decimal UnitApproved { get; set; } //SVC05
        public decimal UnitCharged { get; set; } //SVC07
        public string ServiceCode { get; set; }

        public bool Applied { get; set; }
        public virtual Claim835Claims Claim835Claims { get; set; }
        public virtual Claim837ServiceLine Claim837ServiceLine { get; set; }
    }
}
