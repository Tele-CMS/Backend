using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HC.Patient.Entity
{
    public class StaffAward : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        [Column(TypeName = "varchar(100)")]
        [Required]
        public string AwardType { get; set; }
        public DateTime? AwardDate { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Description { get; set; }
        public Staffs Staff { get; set; }

    }
}
