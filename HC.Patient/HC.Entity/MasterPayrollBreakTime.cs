using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterPayrollBreakTime : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }       
        [Required]
        public decimal BreakDuration { get; set; }
        [Required]
        public decimal StartRange { get; set; }
        [Required]
        public decimal EndRange { get; set; }
        [Required]
        public int NumberOfBreaks { get; set; }       
        [Column(TypeName = "varchar(10)")]
        public string StateAbbr { get; set; }
    }
}