using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffSpeciality : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Staffs")]
        public int StaffID { get; set; }

        [Required]
        [ForeignKey("GlobalCode")]
        public int GlobalCodeId { get; set; }

        public virtual Staffs Staffs { get; set; }

        public virtual GlobalCode GlobalCode { get; set; }
    }
}
