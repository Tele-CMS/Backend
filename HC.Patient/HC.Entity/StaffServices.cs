using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffServices:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("MasterServices")]
        public int ServiceId { get; set; }
        public MasterServices MasterServices { get; set; }
        [Required]
        [ForeignKey("Staffs")]
        public int StaffId { get; set; }
        public Staffs Staffs { get; set; }
    }
}
