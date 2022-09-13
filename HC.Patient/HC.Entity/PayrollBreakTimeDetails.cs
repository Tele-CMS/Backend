using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PayrollBreakTimeDetails:BaseEntity
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PayrollBreakTime")]
        public int PayrollBreakTimeId { get; set; }
        [Required]
        public decimal StartRange { get; set; }
        [Required]
        public decimal EndRange { get; set; }
        [Required]
        public int NumberOfBreaks { get; set; }
        public virtual PayrollBreakTime PayrollBreakTime { get; set; }
    }
}
