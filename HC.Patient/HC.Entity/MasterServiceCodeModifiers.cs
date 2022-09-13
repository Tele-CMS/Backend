using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterServiceCodeModifiers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ModifierID")]
        public int Id { get; set; }
        
        public int ServiceCodeID { get; set; }

        public string Modifier { get; set; }

        public decimal Rate { get; set; }

        public virtual MasterServiceCode MasterServiceCode { get; set; }
    }
}
