using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claim835ServiceLineAdjustmentDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("Claim835ServiceLine")]
        public int Claim835ServiceLineId { get; set; }
        public decimal AmountAdjusted { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string AdjustmentGroupCode { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string AdjustmentReasonCode { get; set; }
        public decimal UnitApproved { get; set; }

        public bool Applied { get; set; }
        public virtual Claim835ServiceLine Claim835ServiceLine { get; set; }
    }
}

