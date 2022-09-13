using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class RoundingRuleDetails : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RuleDetailID")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("MasterRoundingRules")]
        public int RuleID { get; set; }

        [Required]
        public decimal StartRange { get; set; }

        [Required]
        public decimal EndRange { get; set; }

        [Required]
        public int Unit { get; set; }
        public virtual MasterRoundingRules MasterRoundingRules { get; set; }

    }
}