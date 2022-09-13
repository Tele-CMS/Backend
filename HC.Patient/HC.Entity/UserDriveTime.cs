using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserDriveTime :BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public  int Id { get; set; }

        [ForeignKey("Staffs")]
        public int StaffId { get; set; }
        public DateTime DateOfService { get; set; }
        public decimal TotalDriveTime { get; set; }
        public decimal TotalDistance { get; set; }
        public virtual Staffs Staffs { get; set; }

    }
}
