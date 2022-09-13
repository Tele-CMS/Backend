using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffQualification : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        [Column(TypeName = "varchar(100)")]
        [Required]
        public string Course { get; set; }
        [Column(TypeName = "varchar(200)")]
        [Required]
        public string University { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Staffs Staff { get; set; }
    }
}
