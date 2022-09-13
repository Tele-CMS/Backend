using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PayerServiceCodeModifiers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PayerModifierId")]
        public int Id { get; set; }
        
        public int PayerServiceCodeId { get; set; }

        public string Modifier { get; set; }

        public decimal Rate { get; set; }

        public virtual PayerServiceCodes PayerServiceCodes { get; set; }
    }
}
