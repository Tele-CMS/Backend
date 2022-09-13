using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
   public class StaffExperience:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string OrganizationName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public Staffs Staff { get; set; }
    }
}
