using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffTags : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("StaffTagID")]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Staffs")]
        public int StaffID { get; set; }

        [Required]
        [ForeignKey("MasterTags")]
        public int TagID { get; set; }

        public virtual Staffs Staffs { get; set; }
 
        public virtual MasterTags MasterTags { get; set; }
    }
}
