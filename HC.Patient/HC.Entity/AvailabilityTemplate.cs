using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class StaffAvailability : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("StaffAvailabilityId")]
        public int Id { get; set; }

        [ForeignKey("MasterWeekDays")]
        public int? DayId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? Date { get; set; }

        [ForeignKey("StaffAvailabilityType")]
        public int StaffAvailabilityTypeID { get; set; }

        [ForeignKey("Staffs")]
        public int StaffID { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Staffs Staffs { get; set; }
        public virtual MasterWeekDays MasterWeekDays { get; set; }
        public virtual GlobalCode StaffAvailabilityType { get; set; }
    }
}