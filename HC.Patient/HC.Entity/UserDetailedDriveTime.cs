using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserDetailedDriveTime : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public decimal DriveTime { get; set; }
        public decimal Distance { get; set; }

        [ForeignKey("GlobalCode")]
        public int StatusId { get; set; }

        [ForeignKey("PatientEncounter")]
        public Nullable<int> PatientEncounterId { get; set; }

        [ForeignKey("AppointmentType")]
        public Nullable<int> AppointmentTypeId { get; set; }
        [ForeignKey("Staffs")]
        public int StaffId { get; set; }
        public virtual PatientEncounter PatientEncounter { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual AppointmentType AppointmentType { get; set; }
        public virtual Staffs Staffs { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
